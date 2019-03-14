using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    class Module
    {
        UInt32 index = 0;
        byte[] bytes;

        public Module(byte[] bytes)
        {
            this.bytes = bytes;

            if (this.bytes[0] != 0x00 || this.bytes[1] != 0x61 || this.bytes[2] != 0x73 || this.bytes[3] != 0x6D)
            {
                throw new Exception("Invalid magic number.");
            }

            UInt32 version = BitConverter.ToUInt32(this.bytes, 4);

            Console.WriteLine("Loading module version: " + version + "...");

            this.index = 8;

            while(this.index < this.bytes.Length)
            {
                switch(this.GetByte())
                {
                    case 0x00:
                        Console.WriteLine("Loading custom section...");
                        break;
                    case 0x01:
                        Console.WriteLine("Loading type section...");
                        this.loadTypes();
                        continue;
                    case 0x02:
                        Console.WriteLine("Loading import section...");
                        this.loadImports();
                        continue;
                    case 0x03:
                        Console.WriteLine("Loading function section...");
                        this.loadFunctions();
                        continue;
                    case 0x04:
                        Console.WriteLine("Loading table section...");
                        this.loadTables();
                        continue;
                    case 0x05:
                        Console.WriteLine("Loading memory section...");
                        this.loadMemory();
                        continue;
                    case 0x06:
                        Console.WriteLine("Loading global section...");
                        this.loadGlobals();
                        continue;
                    case 0x07:
                        Console.WriteLine("Loading export section...");
                        this.loadExports();
                        continue;
                    case 0x08:
                        Console.WriteLine("Loading start section...");
                        this.loadStart();
                        continue;
                    case 0x09:
                        Console.WriteLine("Loading element section...");
                        this.loadElements();
                        continue;
                    case 0x0A:
                        Console.WriteLine("Loading code section...");
                        this.loadCode();
                        continue;
                    case 0x0B:
                        Console.WriteLine("Loading data section...");
                        this.loadData();
                        continue;
                    default:
                        throw new Exception("Invalid module section ID: " + bytes[index]);
                }

                UInt32 sectionSize = this.GetUInt32();
                index += sectionSize;
            }

            Console.WriteLine("Module loaded.");
        }

        private void loadTypes()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Type section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Type vector size: " + vectorSize);

            for (uint fType = 0; fType < vectorSize; fType++)
            {
                if(this.GetByte() != 0x60)
                {
                    throw new Exception("Invalid function type.");
                }

                UInt32 pTypeLength = this.GetUInt32();
                for (uint pType = 0; pType < pTypeLength; pType++)
                {
                    byte valType = this.GetValType();
                    Console.WriteLine("Parameter type: {0:X}", valType);
                }

                UInt32 rTypeLength = this.GetUInt32();
                for (uint rType = 0; rType < rTypeLength; rType++)
                {
                    byte valType = this.GetValType();
                    Console.WriteLine("Return type: {0:X}", valType);
                }
            }
        }

        private void loadImports()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Import section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Import vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                var mod = this.GetName();
                var nm = this.GetName();

                var type = this.GetByte();
                switch (type)
                {
                    case 0x00: // x:typeidx => func x
                        Console.WriteLine("function " + mod + "@" + nm);
                        this.GetIndex();
                        break;
                    case 0x01: // tt:tabletype => table tt
                        Console.WriteLine("table " + mod + "@" + nm);
                        this.GetTableType();
                        break;
                    case 0x02: // mt:memtype => mem mt
                        Console.WriteLine("memory " + mod + "@" + nm);
                        this.GetMemType();
                        break;
                    case 0x03: // gt:globaltype => global gt
                        Console.WriteLine("global " + mod + "@" + nm);
                        this.GetGlobalType();
                        break;
                    default:
                        throw new Exception("Invalid import type: " + type.ToString("X"));
                }
            }
        }

        private void loadFunctions()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Function section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Function vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                var typeidx = this.GetIndex();
            }
        }

        private void loadTables()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Table section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Table vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetTableType();
            }
        }

        private void loadMemory()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Memory section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Memory vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetMemType();
            }
        }

        private void loadGlobals()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Global section size: " + sectionSize);

            this.index += sectionSize;
            /*
            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Global vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetGlobalType();
            }*/
        }

        private void loadExports()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Export section size: " + sectionSize);

            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Export vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                var nm = this.GetName();

                var type = this.GetByte();
                switch (type)
                {
                    case 0x00: // x:typeidx => func x
                        Console.WriteLine("function " + "@" + nm);
                        this.GetIndex();
                        break;
                    case 0x01: // tt:tabletype => table tt
                        Console.WriteLine("table " + "@" + nm);
                        this.GetIndex();
                        break;
                    case 0x02: // mt:memtype => mem mt
                        Console.WriteLine("memory " + "@" + nm);
                        this.GetIndex();
                        break;
                    case 0x03: // gt:globaltype => global gt
                        Console.WriteLine("global " + "@" + nm);
                        this.GetIndex();
                        break;
                    default:
                        throw new Exception("Invalid export type: " + type.ToString("X"));
                }
            }
        }

        private void loadStart()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Start section size: " + sectionSize);

            UInt32 funcidx = this.GetIndex();
            Console.WriteLine("Start function index: " + funcidx);
        }

        private void loadElements()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Element section size: " + sectionSize);

            this.index += sectionSize;
            /*
            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Global vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetGlobalType();
            }*/
        }

        private void loadCode()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Code section size: " + sectionSize);

            this.index += sectionSize;
            /*
            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Global vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetGlobalType();
            }*/
        }

        private void loadData()
        {
            UInt32 sectionSize = this.GetUInt32();
            Console.WriteLine("Data section size: " + sectionSize);

            this.index += sectionSize;
            /*
            UInt32 vectorSize = this.GetUInt32();
            Console.WriteLine("Global vector size: " + vectorSize);

            for (uint import = 0; import < vectorSize; import++)
            {
                this.GetGlobalType();
            }*/
        }

        private byte GetValType()
        {
            byte valType = this.GetByte();

            switch(valType)
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
            Array.Copy(this.bytes, this.index, sub, 0, length);
            string result = System.Text.Encoding.UTF8.GetString(sub);
            this.index += length;

            return result;
        }

        private byte GetByte()
        {
            return this.bytes[this.index++];
        }

        private UInt32 GetIndex()
        {
            return this.GetUInt32();
        }

        private UInt32 GetUInt32()
        {
            UInt32 result = 0;
            int shift = 0;
            while (true)
            {
                byte b = this.GetByte();
                result |= (UInt32)((b & 0x7F) << shift);
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }

            return result;
        }

        private void GetTableType()
        {
            byte elemType = this.GetElemType();
            UInt32 min, max;
            bool hasMax = this.GetLimits(out min, out max);
        }

        private void GetMemType()
        {
            UInt32 min, max;
            bool hasMax = this.GetLimits(out min, out max);
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
                    throw new Exception("Invalid boolean value: " + b.ToString("X"));
            }
        }

        private void GetGlobalType()
        {
            var t = this.GetValType();
            var mut = this.GetBoolean();
        }

        private byte GetBlockType()
        {
            if(this.bytes[this.index] == 0x40)
            {
                return 0x40;
            }
            else
            {
                return this.GetValType();
            }
        }
    }
}
