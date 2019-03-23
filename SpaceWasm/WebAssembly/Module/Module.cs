using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module
{
    public class Module
    {
        public string Name;
        public Store Store;

        Parser parser;

        List<Type> types = new List<Type>();
        public List<Function> Functions = new List<Function>();
        public List<Memory> Memory = new List<Memory>();
        public List<Table> Tables = new List<Table>();
        public List<WebAssembly.Global> Globals = new List<WebAssembly.Global>();

        Function startFunction = null;

        public bool Debug = false;

        int functionIndex = 0;

        public Dictionary<string, object> Exports = new Dictionary<string, object>();

        public Module(string name, Store store)
        {
            this.Name = name;
            this.Store = store;
        }

        public Module(string name, Store store, byte[] bytes)
        {
            this.Name = name;
            this.Store = store;
            this.parser = new Parser(bytes, this);

            if (parser.GetByte() != 0x00 || parser.GetByte() != 0x61 || parser.GetByte() != 0x73 || parser.GetByte() != 0x6D)
            {
                throw new Exception("Invalid magic number.");
            }

            UInt32 version = parser.GetVersion();

            while(!parser.Done())
            {
                byte section = parser.GetByte();
                switch (section)
                {
                    case 0x00:
                        UInt32 sectionSize = this.parser.GetUInt32();
                        this.parser.Skip(sectionSize);
                        break;
                    case 0x01:
                        this.loadTypes();
                        continue;
                    case 0x02:
                        this.loadImports();
                        continue;
                    case 0x03:
                        this.loadFunctions();
                        continue;
                    case 0x04:
                        this.loadTables();
                        continue;
                    case 0x05:
                        this.loadMemory();
                        continue;
                    case 0x06:
                        this.loadGlobals();
                        continue;
                    case 0x07:
                        this.loadExports();
                        continue;
                    case 0x08:
                        this.loadStart();
                        continue;
                    case 0x09:
                        this.loadElements();
                        continue;
                    case 0x0A:
                        this.loadCode();
                        continue;
                    case 0x0B:
                        this.loadData();
                        continue;
                    default:
                        throw new Exception("Invalid module section ID: 0x" + section.ToString("X"));
                }
            }

            if(this.startFunction != null)
            {
                this.startFunction.NativeCall();

                while (this.Store.Step(this.Debug)) { }
            }
        }

        private void loadTypes()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint fType = 0; fType < vectorSize; fType++)
            {
                if(this.parser.GetByte() != 0x60)
                {
                    throw new Exception("Invalid function type.");
                }

                UInt32 pTypeLength = this.parser.GetUInt32();
                byte[] parameters = new byte[pTypeLength];
                for (uint pType = 0; pType < pTypeLength; pType++)
                {
                    byte valType = this.parser.GetValType();
                    parameters[pType] = valType;
                }

                UInt32 rTypeLength = this.parser.GetUInt32();
                byte[] results = new byte[rTypeLength];
                for (uint rType = 0; rType < rTypeLength; rType++)
                {
                    byte valType = this.parser.GetValType();
                    results[rType] = valType;
                }

                this.types.Add(new Type(parameters, results));
            }
        }

        private void loadImports()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                var mod = this.parser.GetName();
                var nm = this.parser.GetName();

                var type = this.parser.GetByte();

                if (!this.Store.Modules.ContainsKey(mod))
                {
                    throw new Exception("Import module not found: " + mod + "@" + nm);
                }
                else if (!this.Store.Modules[mod].Exports.ContainsKey(nm))
                {
                    string typeString = "unknown";
                    switch (type)
                    {
                        case 0x00:
                            typeString = "function"; break;
                        case 0x01:
                            typeString = "table"; break;
                        case 0x02:
                            typeString = "memory"; break;
                        case 0x03:
                            typeString = "global"; break;
                        default:
                            break;
                    }

                    throw new Exception("Import (" + typeString + ") name not found: " + mod + "@" + nm);
                }
                else
                {
                    switch (type)
                    {
                        case 0x00: // x:typeidx => func x
                            int funcTypeIdx = (int)this.parser.GetIndex();
                            if (funcTypeIdx >= this.types.Count())
                            {
                                throw new Exception("Import function type does not exist: " + mod + "@" + nm + " " + this.types[funcTypeIdx]);
                            }
                            else if (!this.types[funcTypeIdx].SameAs(((Function)this.Store.Modules[mod].Exports[nm]).Type))
                            {
                                throw new Exception("Import function type mismatch: " + mod + "@" + nm + " - " + this.types[funcTypeIdx] + " != " + ((Function)this.Store.Modules[mod].Exports[nm]).Type);
                            }
                            else
                            {
                                this.Functions.Add((Function)this.Store.Modules[mod].Exports[nm]);
                                this.functionIndex++;
                            }
                            break;
                        case 0x01: // tt:tabletype => table tt
                            Table t = this.parser.GetTableType();
                            if (!t.CompatibleWith((Table)this.Store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import table type mismatch: " + mod + "@" + nm + " " + t + " != " + (Table)this.Store.Modules[mod].Exports[nm]);
                            }
                            this.Tables.Add((Table)this.Store.Modules[mod].Exports[nm]);
                            break;
                        case 0x02: // mt:memtype => mem mt
                            Memory m = this.parser.GetMemType();
                            if (!m.CompatibleWith((Memory)this.Store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import memory type mismatch: " + mod + "@" + nm + " " + m + " != " + (Memory)this.Store.Modules[mod].Exports[nm]);
                            }
                            this.Memory.Add((Memory)this.Store.Modules[mod].Exports[nm]);
                            break;
                        case 0x03: // gt:globaltype => global gt
                            byte gType;
                            bool mutable;
                            this.parser.GetGlobalType(out gType, out mutable);

                            if(((WebAssembly.Global)this.Store.Modules[mod].Exports[nm]).Type != gType)
                            {
                                throw new Exception("Import global type mismatch: " + mod + "@" + nm);
                            }

                            var global = (WebAssembly.Global)this.Store.Modules[mod].Exports[nm];
                            global.SetName(mod + "." + nm);
                            this.Globals.Add(global);
                            break;
                        default:
                            throw new Exception("Invalid import type: 0x" + type.ToString("X"));
                    }
                }
            }
        }

        private void loadFunctions()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint function = 0; function < vectorSize; function++)
            {
                int index = (int)this.parser.GetIndex();
                if(index < this.types.Count())
                {
                    this.Functions.Add(new Function(this, (uint)(this.Functions.Count()), this.types[index]));
                }
                else
                {
                    throw new Exception("Function type " + index + "/" + this.types.Count() + " does not exist.");
                }

            }
        }

        private void loadTables()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint table = 0; table < vectorSize; table++)
            {
                this.Tables.Add(this.parser.GetTableType());
            }
        }

        private void loadMemory()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                this.Memory.Add(this.parser.GetMemType());
            }
        }

        private void loadGlobals()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                byte type;
                bool mutable;
                this.parser.GetGlobalType(out type, out mutable);

                var expr = this.parser.GetExpr();
                this.Store.CurrentFrame = new WebAssembly.Stack.Frame(this.Store, this, null, expr);
                do
                {
                }
                while (this.Store.CurrentFrame.Step(this.Debug));
                this.Store.CurrentFrame = null;

                this.Globals.Add(new WebAssembly.Global(type, mutable, this.Store.Stack.Pop(), (UInt32)this.Globals.Count()));
            }
        }

        private void loadExports()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint export = 0; export < vectorSize; export++)
            {
                var nm = this.parser.GetName();

                var type = this.parser.GetByte();
                int index = (int)this.parser.GetIndex();
                switch (type)
                {
                    case 0x00: // x:typeidx => func x
                        if (index < this.Functions.Count())
                        {
                            this.Functions[index].SetName(nm);
                            this.Exports.Add(nm, this.Functions[index]);
                        }
                        else
                        {
                            throw new Exception("Function export \"" + nm + "\" index " + index + "/" + this.Functions.Count() + " does not exist.");
                        }
                        break;
                    case 0x01: // tt:tabletype => table tt
                        if (index < this.Tables.Count())
                        {
                            this.Exports.Add(nm, this.Tables[index]);
                        }
                        else
                        {
                            throw new Exception("Table export \"" + nm + "\" index " + index + "/" + this.Tables.Count() + " does not exist.");
                        }
                        break;
                    case 0x02: // mt:memtype => mem mt
                        if(index < this.Memory.Count())
                        {
                            this.Exports.Add(nm, this.Memory[index]);
                        }
                        else
                        {
                            throw new Exception("Memory export \" + nm + \" index " + index + "/" + this.Memory.Count() + " does not exist.");
                        }
                        break;
                    case 0x03: // gt:globaltype => global gt
                        if (index < this.Globals.Count())
                        {
                            this.Globals[index].SetName(this.Name + "." + nm);
                            this.Exports.Add(nm, this.Globals[index]);
                        }
                        else
                        {
                            throw new Exception("Global export \"" + nm + "\" index " + index + "/" + this.Globals.Count() + " does not exist.");
                        }
                        break;
                    default:
                        throw new Exception("Invalid export type: 0x" + type.ToString("X"));
                }
            }
        }

        private void loadStart()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            int index = (int)this.parser.GetIndex();

            if (index < this.Functions.Count())
            {
                this.startFunction = this.Functions[index];
            }
            else
            {
                throw new Exception("Memory export \" + nm + \" index does not exist.");
            }
        }

        private void loadElements()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint element = 0; element < vectorSize; element++)
            {
                int tableidx = (int)this.parser.GetUInt32();
                if(tableidx >= this.Tables.Count())
                {
                    throw new Exception("Element table index does not exist");
                }

                var expr = this.parser.GetExpr();
                this.Store.CurrentFrame = new WebAssembly.Stack.Frame(this.Store, this, null, expr);
                do
                {
                }
                while (this.Store.CurrentFrame.Step(this.Debug));
                this.Store.CurrentFrame = null;

                UInt32 offset = this.Store.Stack.PopI32();

                UInt32 funcVecSize = this.parser.GetUInt32();
                for (uint func = 0; func < funcVecSize; func++)
                {
                    UInt32 funcidz = this.parser.GetUInt32();

                    this.Tables[tableidx].Set(offset + func, funcidz);
                }
            }
        }

        private void loadCode()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint funcidx = 0; funcidx < vectorSize; funcidx++)
            {
                if(funcidx >= this.Functions.Count())
                {
                    throw new Exception("Missing function in code segment.");
                }

                UInt32 size = this.parser.GetUInt32();
                UInt32 end = this.parser.GetPointer() + size;
                UInt32 numLocals = this.parser.GetUInt32();
                for(uint local = 0; local < numLocals; local++)
                {
                    UInt32 count = this.parser.GetUInt32();
                    byte type = this.parser.GetValType();
                    for(uint n = 0; n < count; n++)
                        this.Functions[(int)this.functionIndex].AddLocal(type);
                }

                this.Functions[(int)this.functionIndex].SetInstruction(this.parser.GetExpr());
                this.functionIndex++;

                if(this.parser.GetPointer() != end)
                {
                    throw new Exception("Invalid position: 0x" + this.parser.GetPointer().ToString("X") + " after loading code.  Should be: 0x" + end.ToString("X") + " at 0x" + this.parser.PeekByte());
                }
            }
        }

        private void loadData()
        {
            UInt32 sectionSize = this.parser.GetUInt32();
            UInt32 vectorSize = this.parser.GetUInt32();

            for (uint data = 0; data < vectorSize; data++)
            {
                int memidx = (int)this.parser.GetUInt32();
                if (memidx >= this.Memory.Count())
                {
                    throw new Exception("Data memory index does not exist");
                }

                var expr = this.parser.GetExpr();
                do
                {
                    expr = expr.Run(this.Store);
                }
                while (expr != null);

                UInt64 offset;
                if (this.Store.Stack.Peek().GetType().ToString() == "System.UInt32")
                {
                    offset = (UInt64)this.Store.Stack.PopI32();
                }
                else
                {
                    offset = this.Store.Stack.PopI64();
                }
                
                UInt32 memVecSize = this.parser.GetUInt32();
                for (uint mem = 0; mem < memVecSize; mem++)
                {
                    byte b = this.parser.GetByte();

                    this.Memory[memidx].Set(offset + mem, b);
                }
            }
        }


        public void AddExportFunc(string name, byte[] parameters = null, byte[] results = null, Func<object[], object[]> action = null)
        {
            if (parameters == null)
                parameters = new byte[] { };
            if (results == null)
                results = new byte[] { };

            Function func;

            if (action == null)
                func = new Function(this, new Type(parameters, results));
            else
                func = new Function(this, action, new Type(parameters, results));

            func.SetName(this.Name + "@" + name);

            this.Exports.Add(name, func);
        }

        public WebAssembly.Global AddExportGlob(string name, byte type, bool mutable, object v)
        {
            var global = new WebAssembly.Global(type, mutable, v, (UInt32)this.Exports.Count());
            this.Exports.Add(name, global);
            return global;
        }

        public void AddExportMemory(string name, Memory m)
        {
            this.Exports.Add(name, m);
        }

        public void AddExportTable(string name, Table t)
        {
            this.Exports.Add(name, t);
        }

        public void DumpExports()
        {
            Console.WriteLine(this.Name + " exports:");
            foreach(var export in this.Exports)
            {
                var f = export.Value as Function;
                var t = export.Value as Table;
                var m = export.Value as Memory;

                if (f != null)
                {
                    Console.WriteLine("Function " + export.Key + " " + f.Type);
                }
                else if (t != null)
                {
                    Console.WriteLine("Table " + export.Key);
                }
                else if (m != null)
                {
                    Console.WriteLine("Memory " + export.Key);
                }
                else
                {
                    Console.WriteLine("Global " + export.Key);
                }
            }
        }

        public void Execute(string function, params object[] parameters)
        {
            if(!this.Exports.ContainsKey(function) || (this.Exports[function] as Function) == null)
            {
                throw new Exception("Function \"" + function + "\" does not exist in " + this.Name + ".");
            }

            foreach(var p in parameters)
            {
                this.Store.Stack.Push(p);
            }

            Function f = this.Exports[function] as Function;
            f.NativeCall();
        }

        public object Call(string function, params object[] parameters)
        {
            this.CallVoid(function, parameters);

            return this.Store.Stack.Pop();
        }

        public void CallVoid(string function, params object[] parameters)
        {
            this.Execute(function, parameters);

            while (this.Store.Step(this.Debug))
            {

            }
        }
    }
}
