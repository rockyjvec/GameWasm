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
        Store store;

        UInt32 IP = 0;
        byte[] bytes;

        List<Type> types = new List<Type>();
        List<Function> functions = new List<Function>();
        List<Memory> memories = new List<Memory>();
        List<Table> tables = new List<Table>();
        List<WebAssembly.Value> globals = new List<WebAssembly.Value>();

        public Dictionary<string, object> Exports = new Dictionary<string, object>();

        public Module(string name, Store store)
        {
            this.Name = name;
            this.store = store;
        }

        public Module(string name, Store store, byte[] bytes)
        {
            this.Name = name;
            this.store = store;
            this.bytes = bytes;

            if (this.bytes[0] != 0x00 || this.bytes[1] != 0x61 || this.bytes[2] != 0x73 || this.bytes[3] != 0x6D)
            {
                throw new Exception("Invalid magic number.");
            }

            UInt32 version = BitConverter.ToUInt32(this.bytes, 4);

            this.IP = 8;

            while(this.IP < this.bytes.Length)
            {
                switch(this.GetByte())
                {
                    case 0x00:
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
                        throw new Exception("Invalid module section ID: " + bytes[IP]);
                }

                UInt32 sectionSize = this.GetUInt32();
                IP += sectionSize;
            }
        }

        private void loadTypes()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint fType = 0; fType < vectorSize; fType++)
            {
                if(this.GetByte() != 0x60)
                {
                    throw new Exception("Invalid function type.");
                }

                UInt32 pTypeLength = this.GetUInt32();
                byte[] parameters = new byte[pTypeLength];
                for (uint pType = 0; pType < pTypeLength; pType++)
                {
                    byte valType = this.GetValType();
                    parameters[pType] = valType;
                }

                UInt32 rTypeLength = this.GetUInt32();
                byte[] results = new byte[rTypeLength];
                for (uint rType = 0; rType < rTypeLength; rType++)
                {
                    byte valType = this.GetValType();
                    results[rType] = valType;
                }

                this.types.Add(new Type(parameters, results));
            }
        }

        private void loadImports()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                var mod = this.GetName();
                var nm = this.GetName();

                var type = this.GetByte();

                if (!this.store.Modules.ContainsKey(mod))
                {
                    throw new Exception("Import module not found: " + mod + "@" + nm);
                }
                else if (!this.store.Modules[mod].Exports.ContainsKey(nm))
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
                            int funcTypeIdx = (int)this.GetIndex();
                            if (funcTypeIdx >= this.types.Count())
                            {
                                throw new Exception("Import function type does not exist: " + mod + "@" + nm + " " + this.types[funcTypeIdx]);
                            }
                            else if (!this.types[funcTypeIdx].SameAs(((Function)this.store.Modules[mod].Exports[nm]).Type))
                            {
                                throw new Exception("Import function type mismatch: " + mod + "@" + nm + " - " + this.types[funcTypeIdx] + " != " + ((Function)this.store.Modules[mod].Exports[nm]).Type);
                            }
                            else
                            {
                                this.functions.Add((Function)this.store.Modules[mod].Exports[nm]);
                            }
                            break;
                        case 0x01: // tt:tabletype => table tt
                            Table t = this.GetTableType();
                            if (!t.CompatibleWith((Table)this.store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import table type mismatch: " + mod + "@" + nm + " " + t + " != " + (Table)this.store.Modules[mod].Exports[nm]);
                            }
                            this.tables.Add((Table)this.store.Modules[mod].Exports[nm]);
                            break;
                        case 0x02: // mt:memtype => mem mt
                            Memory m = this.GetMemType();
                            if (!m.CompatibleWith((Memory)this.store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import memory type mismatch: " + mod + "@" + nm + " " + m + " != " + (Memory)this.store.Modules[mod].Exports[nm]);
                            }
                            this.memories.Add((Memory)this.store.Modules[mod].Exports[nm]);
                            break;
                        case 0x03: // gt:globaltype => global gt
                            Value v = this.GetGlobalType();
                            if(!v.CompatibleWith((Value)this.store.Modules[mod].Exports[nm]))
                            {
                                throw new Exception("Import global type mismatch: " + mod + "@" + nm + " - 0x" + v.Type.ToString("X") + " != 0x" + ((Value)this.store.Modules[mod].Exports[nm]).Type.ToString("X"));
                            }                            
                            this.globals.Add((Value)this.store.Modules[mod].Exports[nm]);
                            break;
                        default:
                            throw new Exception("Invalid import type: 0x" + type.ToString("X"));
                    }
                }
            }
        }

        private void loadFunctions()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint function = 0; function < vectorSize; function++)
            {
                int index = (int)this.GetIndex();
                if(index < this.types.Count())
                {
                    this.functions.Add(new Function(function, this.types[index]));
                }
                else
                {
                    throw new Exception("Function type " + index + "/" + this.types.Count() + " does not exist.");
                }

            }
        }

        private void loadTables()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint table = 0; table < vectorSize; table++)
            {
                this.tables.Add(this.GetTableType());
            }
        }

        private void loadMemory()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                this.memories.Add(this.GetMemType());
            }
        }

        private void loadGlobals()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint import = 0; import < vectorSize; import++)
            {
                var global = this.GetGlobalType();

                while (this.Step())
                {

                }

                global.Set(this.store.PopValue());

                this.globals.Add(global);
            }
        }

        private void loadExports()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint export = 0; export < vectorSize; export++)
            {
                var nm = this.GetName();

                var type = this.GetByte();
                int index = (int)this.GetIndex();
                switch (type)
                {
                    case 0x00: // x:typeidx => func x
                        if (index < this.functions.Count())
                        {
                            this.Exports.Add(nm, this.functions[index]);
                        }
                        else
                        {
                            throw new Exception("Function export \"" + nm + "\" index " + index + "/" + this.functions.Count() + " does not exist.");
                        }
                        break;
                    case 0x01: // tt:tabletype => table tt
                        if (index < this.tables.Count())
                        {
                            this.Exports.Add(nm, this.tables[index]);
                        }
                        else
                        {
                            throw new Exception("Table export \"" + nm + "\" index " + index + "/" + this.tables.Count() + " does not exist.");
                        }
                        break;
                    case 0x02: // mt:memtype => mem mt
                        if(index < this.memories.Count())
                        {
                            this.Exports.Add(nm, this.memories[index]);
                        }
                        else
                        {
                            throw new Exception("Memory export \" + nm + \" index " + index + "/" + this.memories.Count() + " does not exist.");
                        }
                        break;
                    case 0x03: // gt:globaltype => global gt
                        if (index < this.globals.Count())
                        {
                            this.Exports.Add(nm, this.globals[index]);
                        }
                        else
                        {
                            throw new Exception("Global export \"" + nm + "\" index " + index + "/" + this.globals.Count() + " does not exist.");
                        }
                        break;
                    default:
                        throw new Exception("Invalid export type: 0x" + type.ToString("X"));
                }
            }
        }

        private void loadStart()
        {
            UInt32 sectionSize = this.GetUInt32();
            int index = (int)this.GetIndex();

            if (index < this.functions.Count())
            {
                this.functions[index].Execute();
            }
            else
            {
                throw new Exception("Memory export \" + nm + \" index does not exist.");
            }
        }

        private void loadElements()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint element = 0; element < vectorSize; element++)
            {
                int tableidx = (int)this.GetUInt32();
                if(tableidx >= this.tables.Count())
                {
                    throw new Exception("Element table index does not exist");
                }

                while (this.Step())
                {

                }

                UInt32 offset = this.store.PopValue().GetI32();

                UInt32 funcVecSize = this.GetUInt32();
                for (uint func = 0; func < funcVecSize; func++)
                {
                    UInt32 funcidz = this.GetUInt32();

                    this.tables[tableidx].Set(offset + func, funcidz);
                }
            }
        }

        private void loadCode()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint funcidx = 0; funcidx < vectorSize; funcidx++)
            {
                if(funcidx >= this.functions.Count())
                {
                    throw new Exception("Missing function in code segment.");
                }

                UInt32 size = this.GetUInt32();
                UInt32 localStart = this.IP;
                UInt32 numLocals = this.GetUInt32();
                for(uint local = 0; local < numLocals; local++)
                {
                    UInt32 count = this.GetUInt32();
                    byte type = this.GetValType();
                    for(uint n = 0; n < count; n++)
                        this.functions[(int)funcidx].AddLocal(type);
                }

                UInt32 codeOffset = this.IP;
                UInt32 codeLength = size - (this.IP - localStart);

                this.functions[(int)funcidx].SetCode(this.bytes, codeOffset, codeLength);

                this.IP += codeLength;
            }
        }

        private void loadData()
        {
            UInt32 sectionSize = this.GetUInt32();
            UInt32 vectorSize = this.GetUInt32();

            for (uint data = 0; data < vectorSize; data++)
            {
                int memidx = (int)this.GetUInt32();
                if (memidx >= this.memories.Count())
                {
                    throw new Exception("Data memory index does not exist");
                }

                while (this.Step())
                {

                }

                UInt32 offset = this.store.PopValue().GetI32();

                UInt32 memVecSize = this.GetUInt32();
                for (uint mem = 0; mem < memVecSize; mem++)
                {
                    byte b = this.GetByte();

                    this.memories[memidx].Set(offset + mem, b);
                }
            }
        }

        private byte GetValType()
        {
            byte valType = this.GetByte();

            switch (valType)
            {
                case 0x7F:
                case 0x7E:
                case 0x7D:
                case 0x7C:
                    break;
                default:
                    throw new Exception("Invalid value type: 0x" + valType.ToString("X"));
            }

            return valType;
        }

        private string GetName()
        {
            var length = this.GetUInt32();
            byte[] sub = new byte[length];
            Array.Copy(this.bytes, this.IP, sub, 0, length);
            string result = System.Text.Encoding.UTF8.GetString(sub);
            this.IP += length;

            return result;
        }

        private byte GetByte()
        {
            return this.bytes[this.IP++];
        }

        private UInt32 GetIndex()
        {
            return this.GetUInt32();
        }

        private UInt32 GetUInt32()
        {
            UInt32 result = 0;
            byte shift = 0;
            while (true)
            {
                byte b = this.GetByte();
                result |= (UInt32)(b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }

            return result;
        }

        private UInt64 GetUInt64()
        {
            UInt64 result = 0;
            byte shift = 0;
            while (true)
            {
                byte b = this.GetByte();
                result |= (UInt64)(b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }

            return result;
        }

        private float GetF32()
        {
            float result = BitConverter.ToSingle(this.bytes, (int)this.IP);
            this.IP += 4;
            return result;
        }

        private double GetF64()
        {
            double result = BitConverter.ToDouble(this.bytes, (int)this.IP);
            this.IP += 8;
            return result;
        }

        private Table GetTableType()
        {
            byte elemType = this.GetElemType();
            UInt32 min, max;
            bool hasMax = this.GetLimits(out min, out max);

            return new Table(elemType, min, max);
        }

        private Memory GetMemType()
        {
            UInt32 min = 0, max = 0;
            bool hasMax = this.GetLimits(out min, out max);

            return new Memory(min, max);
        }

        private bool GetLimits(out UInt32 min, out UInt32 max)
        {            
            if (this.GetBoolean())
            {
                min = this.GetUInt32();
                max = this.GetUInt32();
                return true;
            }
            else
            {
                min = max = this.GetUInt32();
                return false;
            }
        }

        private byte GetElemType()
        {
            var type = this.GetByte();

            switch(type)
            {
                case 0x70: //funcref
                    break;
                default:
                    throw new Exception("Invalid element type: 0x" + type.ToString("X"));
            }

            return type;
        }

        private bool GetBoolean()
        {
            byte b = this.GetByte();

            switch (b)
            {
                case 0x00:
                    return false;
                case 0x01:
                    return true;
                default:
                    throw new Exception("Invalid boolean value: 0x" + b.ToString("X"));
            }
        }

        private WebAssembly.Value GetGlobalType()
        {
            var t = this.GetValType();
            var mut = this.GetBoolean();

            return new WebAssembly.Value(t, mut);
        }

        private byte GetBlockType()
        {
            if(this.bytes[this.IP] == 0x40)
            {
                return 0x40;
            }
            else
            {
                return this.GetValType();
            }
        }

        public bool Step()
        {
            byte code = this.GetByte();
            switch (code)
            {
                /* Control Instructions */

                case 0x00: // unreachable
                case 0x01: // nop
                    break;
                case 0x02: // block
                    break;
                case 0x03: // loop
                case 0x04: // if
                case 0x05: // else
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
                case 0x0B: // end
                    return false;
                case 0x0C: // br
                case 0x0D: // br_if
                case 0x0E: // br_table
                case 0x0F: // return
                case 0x10: // call
                case 0x11: // call_indirect

                /* Parametric Instructions */

                case 0x1A: // drop
                case 0x1B: // select

                /* Variable Instructions */

                case 0x20: // local.get
                case 0x21: // local.set
                case 0x22: // local.tee
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
                case 0x23: // global.get
                    int globalidx = (int)this.GetUInt32();
                    this.store.PushValue(this.globals[globalidx]);
                    break;
                case 0x24: // global.set

                /* Memory Instructions */

                case 0x28: // i32.load
                case 0x29: // i64.load
                case 0x2A: // f32.load
                case 0x2B: // f64.load
                case 0x2C: // i32.load8_s
                case 0x2D: // i32.load8_u
                case 0x2E: // i32.load16_s
                case 0x2F: // i32.load16_u
                case 0x30: // i64.load8_s
                case 0x31: // i64.load8_u
                case 0x32: // i64.load16_s
                case 0x33: // i64.load16_u
                case 0x34: // i64.load32_s
                case 0x35: // i64.load32_u
                case 0x36: // i32.store
                case 0x37: // i64.store
                case 0x38: // f32.store
                case 0x39: // f64.store
                case 0x3A: // i32.store8
                case 0x3B: // i32.store16
                case 0x3C: // i64.store8
                case 0x3D: // i64.store16
                case 0x3E: // i64.store32
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
                case 0x3F: // memory.size
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
                case 0x40: // memory.grow
                    byte zero = this.GetByte(); // May be used in future version of WebAssembly to address additional memories
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));

                /* Numeric Instructions */

                case 0x41: // i32.const
                    this.store.PushValue(new Value(Type.i32, false, this.GetUInt32()));
                    break;
                case 0x42: // i64.const
                    this.store.PushValue(new Value(Type.i64, false, this.GetUInt64()));
                    break;
                case 0x43: // f32.const
                    this.store.PushValue(new Value(Type.f32, false, this.GetF32()));
                    break;
                case 0x44: // f64.const
                    this.store.PushValue(new Value(Type.f64, false, this.GetF64()));
                    break;

                case 0x45: // i32.eqz
                case 0x46: // i32.eq
                case 0x47: // i32.ne
                case 0x48: // i32.lt_s
                case 0x49: // i32.lt_u
                case 0x4A: // i32.gt_s
                case 0x4B: // i32.gt_u
                case 0x4C: // i32.le_s
                case 0x4D: // i32.le_u
                case 0x4E: // i32.ge_s
                case 0x4F: // i32.ge_u

                case 0x50: // i64.eqz
                case 0x51: // i64.eq
                case 0x52: // i64.ne
                case 0x53: // i64.lt_s
                case 0x54: // i64.lt_u
                case 0x55: // i64.gt_s
                case 0x56: // i64.gt_u
                case 0x57: // i64.le_s
                case 0x58: // i64.le_u
                case 0x59: // i64.ge_s
                case 0x5A: // i64.ge_u

                case 0x5B: // f32.eq
                case 0x5C: // f32.ne
                case 0x5D: // f32.lt
                case 0x5E: // f32.gt
                case 0x5F: // f32.le
                case 0x60: // f32.ge

                case 0x61: // f64.eq
                case 0x62: // f64.ne
                case 0x63: // f64.lt
                case 0x64: // f64.gt
                case 0x65: // f64.le
                case 0x66: // f64.ge

                case 0x67: // i32.clz
                case 0x68: // i32.ctz
                case 0x69: // i32.popcnt
                case 0x6A: // i32.add
                case 0x6B: // i32.sub
                case 0x6C: // i32.mul
                case 0x6D: // i32.div_s
                case 0x6E: // i32.div_u
                case 0x6F: // i32.rem_s
                case 0x70: // i32.rem_u
                case 0x71: // i32.and
                case 0x72: // i32.or
                case 0x73: // i32.xor
                case 0x74: // i32.shl
                case 0x75: // i32.shr_s
                case 0x76: // i32.shr_u
                case 0x77: // i32.rotl
                case 0x78: // i32.rotr

                case 0x79: // i64.clz
                case 0x7A: // i64.ctz
                case 0x7B: // i64.popcnt
                case 0x7C: // i64.add
                case 0x7D: // i64.sub
                case 0x7E: // i64.mul
                case 0x7F: // i64.div_s
                case 0x80: // i64.div_u
                case 0x81: // i64.rem_s
                case 0x82: // i64.rem_u
                case 0x83: // i64.and
                case 0x84: // i64.or
                case 0x85: // i64.xor
                case 0x86: // i64.shl
                case 0x87: // i64.shr_s
                case 0x88: // i64.shr_u
                case 0x89: // i64.rotl
                case 0x8A: // i64.rotr

                case 0x8B: // f32.abs
                case 0x8C: // f32.neg
                case 0x8D: // f32.ceil
                case 0x8E: // f32.floor
                case 0x8F: // f32.trunc
                case 0x90: // f32.nearest
                case 0x91: // f32.sqrt
                case 0x92: // f32.add
                case 0x93: // f32.sub
                case 0x94: // f32.mul
                case 0x95: // f32.div
                case 0x96: // f32.min
                case 0x97: // f32.max
                case 0x98: // f32.copysign

                case 0x99: // f64.abs
                case 0x9A: // f64.neg
                case 0x9B: // f64.ceil
                case 0x9C: // f64.floor
                case 0x9D: // f64.trunc
                case 0x9E: // f64.nearest
                case 0x9F: // f64.sqrt
                case 0xA0: // f64.add
                case 0xA1: // f64.sub
                case 0xA2: // f64.mul
                case 0xA3: // f64.div
                case 0xA4: // f64.min
                case 0xA5: // f64.max
                case 0xA6: // f64.copysign

                case 0xA7: // i32.wrap_i64
                case 0xA8: // i32.trunc_f32_s
                case 0xA9: // i32.trunc_f32_u
                case 0xAA: // i32.trunc_f64_s
                case 0xAB: // i32.trunc_f64_u
                case 0xAC: // i64.extend_i32_s
                case 0xAD: // i64.extend_i32_u
                case 0xAE: // i64.trunc_f32_s
                case 0xAF: // i64.trunc_f32_u
                case 0xB0: // i64.trunc_f64_s
                case 0xB1: // i64.trunc_f64_u
                case 0xB2: // f32.convert_i32_s
                case 0xB3: // f32.convert_i32_u
                case 0xB4: // f32.convert_i64_s
                case 0xB5: // f32.convert_i64_u
                case 0xB6: // f32.demote_f64
                case 0xB7: // f64.convert_i32_s
                case 0xB8: // f64.convert_i32_u
                case 0xB9: // f64.convert_i64_s
                case 0xBA: // f64.convert_i64.u
                case 0xBB: // f64.promote_f32
                case 0xBC: // i32.reinterpret_f32
                case 0xBD: // i64.reinterpret_i32
                case 0xBE: // f32.reinterpret_i32
                case 0xBF: // f64.reinterpret_i64
                    throw new Exception("Opcode not implemented: 0x" + code.ToString("X"));
            }

            return true;
        }

        protected object[] NotImplemented(object[] parameters)
        {
            throw new Exception("Function not implemented");
        }

        public void AddExportFunc(string name, byte[] parameters = null, byte[] results = null, Func<object[], object[]> action = null)
        {
            if (action == null)
                action = this.NotImplemented;
            if (parameters == null)
                parameters = new byte[] { };
            if (results == null)
                results = new byte[] { };

            this.Exports.Add(name, new Function(action, new Type(parameters, results)));
        }

        public void AddExportGlob(string name, Value v)
        {
            this.Exports.Add(name, v);
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
                var g = export.Value as Value;

                if (f != null)
                {
                    Console.WriteLine("Function " + export.Key + " " + f.Type);
                }
                if (t != null)
                {
                    Console.WriteLine("Table " + export.Key);
                }
                if (m != null)
                {
                    Console.WriteLine("Memory " + export.Key);
                }
                if (g != null)
                {
                    Console.WriteLine("Global " + export.Key);
                }
            }
        }

        public Value[] Execute(string function, params Value[] parameters)
        {
            if(!this.Exports.ContainsKey(function) || (this.Exports[function] as Function) == null)
            {
                throw new Exception("Function \"" + function + "\" does not exist in " + this.Name + ".");
            }

            Function f = this.Exports[function] as Function;
            this.IP = f.ModuleOffset;
            while(this.Step())
            {

            }

            return new Value[f.Type.Results.Length];
        }
    }
}
