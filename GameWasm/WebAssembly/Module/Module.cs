using System;
using System.Collections.Generic;
using System.Linq;
using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly.Module
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
        public List<Webassembly.Global> Globals = new List<Webassembly.Global>();

        Function startFunction = null;

        public bool Debug = false;

        int functionIndex = 0;

        public Dictionary<string, object> Exports = new Dictionary<string, object>();

        public Module(string name, Store store)
        {
            Name = name;
            Store = store;
        }

        public Module(string name, Store store, byte[] bytes)
        {
            Name = name;
            Store = store;
            parser = new Parser(bytes, this);

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
                        UInt32 sectionSize = parser.GetUInt32();
                        parser.Skip(sectionSize);
                        break;
                    case 0x01:
                        loadTypes();
                        continue;
                    case 0x02:
                        loadImports();
                        continue;
                    case 0x03:
                        loadFunctions();
                        continue;
                    case 0x04:
                        loadTables();
                        continue;
                    case 0x05:
                        loadMemory();
                        continue;
                    case 0x06:
                        loadGlobals();
                        continue;
                    case 0x07:
                        loadExports();
                        continue;
                    case 0x08:
                        loadStart();
                        continue;
                    case 0x09:
                        loadElements();
                        continue;
                    case 0x0A:
                        loadCode();
                        continue;
                    case 0x0B:
                        loadData();
                        continue;
                    default:
                        throw new Exception("Invalid module section ID: 0x" + section.ToString("X"));
                }
            }

            if(startFunction != null)
            {
                startFunction.NativeCall();

                while (Store.Step(1000, Debug)) { }
            }
        }

        private void loadTypes()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint fType = 0; fType < vectorSize; fType++)
            {
                if(parser.GetByte() != 0x60)
                {
                    throw new Exception("Invalid function type.");
                }

                UInt32 pTypeLength = parser.GetUInt32();
                byte[] parameters = new byte[pTypeLength];
                for (uint pType = 0; pType < pTypeLength; pType++)
                {
                    byte valType = parser.GetValType();
                    parameters[pType] = valType;
                }

                UInt32 rTypeLength = parser.GetUInt32();
                byte[] results = new byte[rTypeLength];
                for (uint rType = 0; rType < rTypeLength; rType++)
                {
                    byte valType = parser.GetValType();
                    results[rType] = valType;
                }

                types.Add(new Type(parameters, results));
            }
        }

        private void loadImports()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                var mod = parser.GetName();
                var nm = parser.GetName();

                var type = parser.GetByte();

                if (!Store.Modules.ContainsKey(mod))
                {
                    throw new Exception("Import module not found: " + mod + "@" + nm);
                }
                else if (!Store.Modules[mod].Exports.ContainsKey(nm))
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
                            int funcTypeIdx = (int)parser.GetIndex();
                            if (funcTypeIdx >= types.Count())
                            {
                                throw new Exception("Import function type does not exist: " + mod + "@" + nm + " " + types[funcTypeIdx]);
                            }
                            else if (!types[funcTypeIdx].SameAs(((Function)Store.Modules[mod].Exports[nm]).Type))
                            {
                                throw new Exception("Import function type mismatch: " + mod + "@" + nm + " - " + types[funcTypeIdx] + " != " + ((Function)Store.Modules[mod].Exports[nm]).Type);
                            }
                            else
                            {
                                Functions.Add((Function)Store.Modules[mod].Exports[nm]);
                                functionIndex++;
                            }
                            break;
                        case 0x01: // tt:tabletype => table tt
                            Table t = parser.GetTableType();
                            if (!t.CompatibleWith((Table)Store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import table type mismatch: " + mod + "@" + nm + " " + t + " != " + (Table)Store.Modules[mod].Exports[nm]);
                            }
                            Tables.Add((Table)Store.Modules[mod].Exports[nm]);
                            break;
                        case 0x02: // mt:memtype => mem mt
                            Memory m = parser.GetMemType();
                            if (!m.CompatibleWith((Memory)Store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import memory type mismatch: " + mod + "@" + nm + " " + m + " != " + (Memory)Store.Modules[mod].Exports[nm]);
                            }
                            Console.WriteLine("Memory import found.");
                            Memory.Add((Memory)Store.Modules[mod].Exports[nm]);
                            break;
                        case 0x03: // gt:globaltype => global gt
                            byte gType;
                            bool mutable;
                            parser.GetGlobalType(out gType, out mutable);

                            if(((Webassembly.Global)Store.Modules[mod].Exports[nm]).Type != gType)
                            {
                                throw new Exception("Import global type mismatch: " + mod + "@" + nm);
                            }

                            var global = (Webassembly.Global)Store.Modules[mod].Exports[nm];
                            global.SetName(mod + "." + nm);
                            Globals.Add(global);
                            break;
                        default:
                            throw new Exception("Invalid import type: 0x" + type.ToString("X"));
                    }
                }
            }
        }

        private void loadFunctions()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint function = 0; function < vectorSize; function++)
            {
                int index = (int)parser.GetIndex();
                if(index < types.Count())
                {
                    Functions.Add(new Function(this, (uint)(Functions.Count()), types[index]));
                }
                else
                {
                    throw new Exception("Function type " + index + "/" + types.Count() + " does not exist.");
                }

            }
        }

        private void loadTables()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint table = 0; table < vectorSize; table++)
            {
                Tables.Add(parser.GetTableType());
            }
        }

        private void loadMemory()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                Memory.Add(parser.GetMemType());
            }
        }

        private void loadGlobals()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                byte type;
                bool mutable;
                parser.GetGlobalType(out type, out mutable);

                var expr = parser.GetExpr();
                Store.CurrentFrame = new Frame(Store, this, null, expr);
                do
                {
                }
                while (Store.Step(1000, Debug));
                Store.CurrentFrame = null;

                Globals.Add(new Webassembly.Global(type, mutable, Store.Stack.Pop(), (UInt32)Globals.Count()));
            }
        }

        private void loadExports()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint export = 0; export < vectorSize; export++)
            {
                var nm = parser.GetName();

                var type = parser.GetByte();
                int index = (int)parser.GetIndex();
                switch (type)
                {
                    case 0x00: // x:typeidx => func x
                        if (index < Functions.Count())
                        {
                            Functions[index].SetName(nm);
                            Exports.Add(nm, Functions[index]);
                        }
                        else
                        {
                            throw new Exception("Function export \"" + nm + "\" index " + index + "/" + Functions.Count() + " does not exist.");
                        }
                        break;
                    case 0x01: // tt:tabletype => table tt
                        if (index < Tables.Count())
                        {
                            Exports.Add(nm, Tables[index]);
                        }
                        else
                        {
                            throw new Exception("Table export \"" + nm + "\" index " + index + "/" + Tables.Count() + " does not exist.");
                        }
                        break;
                    case 0x02: // mt:memtype => mem mt
                        if(index < Memory.Count())
                        {
                            Exports.Add(nm, Memory[index]);
                        }
                        else
                        {
                            throw new Exception("Memory export \" + nm + \" index " + index + "/" + Memory.Count() + " does not exist.");
                        }
                        break;
                    case 0x03: // gt:globaltype => global gt
                        if (index < Globals.Count())
                        {
                            Globals[index].SetName(Name + "." + nm);
                            Exports.Add(nm, Globals[index]);
                        }
                        else
                        {
                            throw new Exception("Global export \"" + nm + "\" index " + index + "/" + Globals.Count() + " does not exist.");
                        }
                        break;
                    default:
                        throw new Exception("Invalid export type: 0x" + type.ToString("X"));
                }
            }
        }

        private void loadStart()
        {
            UInt32 sectionSize = parser.GetUInt32();
            int index = (int)parser.GetIndex();

            if (index < Functions.Count())
            {
                startFunction = Functions[index];
            }
            else
            {
                throw new Exception("Memory export \" + nm + \" index does not exist.");
            }
        }

        private void loadElements()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint element = 0; element < vectorSize; element++)
            {
                int tableidx = (int)parser.GetUInt32();
                if(tableidx >= Tables.Count())
                {
                    throw new Exception("Element table index does not exist");
                }

                var expr = parser.GetExpr();
                Store.CurrentFrame = new Frame(Store, this, null, expr);
                do
                {
                }
                while (Store.Step(1000, Debug));
                Store.CurrentFrame = null;

                UInt32 offset = Store.Stack.PopI32();

                UInt32 funcVecSize = parser.GetUInt32();
                for (uint func = 0; func < funcVecSize; func++)
                {
                    UInt32 funcidz = parser.GetUInt32();

                    Tables[tableidx].Set(offset + func, funcidz);
                }
            }
        }

        private void loadCode()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint funcidx = 0; funcidx < vectorSize; funcidx++)
            {
                if(funcidx >= Functions.Count())
                {
                    throw new Exception("Missing function in code segment.");
                }

                UInt32 size = parser.GetUInt32();
                UInt32 end = parser.GetPointer() + size;
                UInt32 numLocals = parser.GetUInt32();
                for(uint local = 0; local < numLocals; local++)
                {
                    UInt32 count = parser.GetUInt32();
                    byte type = parser.GetValType();
                    for(uint n = 0; n < count; n++)
                        Functions[(int)functionIndex].AddLocal(type);
                }

                Functions[(int)functionIndex].SetInstruction(parser.GetExpr());
                functionIndex++;

                if(parser.GetPointer() != end)
                {
                    throw new Exception("Invalid position: 0x" + parser.GetPointer().ToString("X") + " after loading code.  Should be: 0x" + end.ToString("X") + " at 0x" + parser.PeekByte());
                }
            }
        }

        private void loadData()
        {
            UInt32 sectionSize = parser.GetUInt32();
            UInt32 vectorSize = parser.GetUInt32();

            for (uint data = 0; data < vectorSize; data++)
            {
                int memidx = (int)parser.GetUInt32();
                if (memidx >= Memory.Count())
                {
                    throw new Exception("Data memory index does not exist");
                }

                var expr = parser.GetExpr();
                do
                {
                    expr = expr.Run(Store);
                }
                while (expr != null);

                UInt64 offset;
                if (Store.Stack.Peek() is UInt32)
                {
                    offset = (UInt64)Store.Stack.PopI32();
                }
                else
                {
                    offset = Store.Stack.PopI64();
                }
                
                UInt32 memVecSize = parser.GetUInt32();
                for (uint mem = 0; mem < memVecSize; mem++)
                {
                    byte b = parser.GetByte();

                    Memory[memidx].Set(offset + mem, b);
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

            func.SetName(Name + "@" + name);

            Exports.Add(name, func);
        }

        public void AddExportFunc(string name, Function f)
        {
            f.SetName(Name + "@" + name);

            Exports.Add(name, f);
        }

        public Webassembly.Global AddExportGlob(string name, byte type, bool mutable, object v)
        {
            var global = new Webassembly.Global(type, mutable, v, (UInt32)Exports.Count());
            Exports.Add(name, global);
            return global;
        }

        public void AddExportMemory(string name, Memory m)
        {
            Exports.Add(name, m);
        }

        public void AddExportTable(string name, Table t)
        {
            Exports.Add(name, t);
        }

        public void DumpExports()
        {
            Console.WriteLine(Name + " exports:");
            foreach(var export in Exports)
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
            if(!Exports.ContainsKey(function) || (Exports[function] as Function) == null)
            {
                throw new Exception("Function \"" + function + "\" does not exist in " + Name + ".");
            }

            foreach(var p in parameters)
            {
                Store.Stack.Push(p);
            }

            Function f = Exports[function] as Function;
            f.NativeCall();
        }

        public object Call(string function, params object[] parameters)
        {
            CallVoid(function, parameters);

            return Store.Stack.Pop();
        }

        public void CallVoid(string function, params object[] parameters)
        {
            Execute(function, parameters);

            while (Store.Step(1000, Debug))
            {

            }
        }
    }
}
