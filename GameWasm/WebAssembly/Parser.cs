using System;

namespace GameWasm.Webassembly
{
    public class Parser
    {
        private byte[] bytes;
        private UInt32 index;
        public Module.Module Module;

        public Parser(byte[] bytes, Module.Module module)
        {
            this.bytes = bytes;
            index = 0;
            Module = module;
        }

        public UInt32 GetPointer()
        {
            return index;
        }

        public void SetPointer(UInt32 index)
        {
            this.index = index;
        }

        public bool Done()
        {
            return index >= bytes.Length;
        }

        public byte GetByte()
        {
            return bytes[index++];
        }

        public byte[] GetBytes(int offset, int length)
        {
            byte[] output = new byte[length];
            Buffer.BlockCopy(bytes, offset, output, 0, length);
            return output;
        }

        public byte PeekByte()
        {
            return bytes[index];
        }

        public Instruction.Instruction GetExpr(Function f, bool debug = false)
        {
            return Instruction.Instruction.Consume(this, f, debug);
        }

        public UInt32 GetIndex()
        {
            return GetUInt32();
        }

        public UInt32 GetUInt32()
        {
            UInt32 result = 0;
            byte shift = 0;
            while (true)
            {
                byte b = GetByte();
                result |= (UInt32)(b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }

            return result;
        }

        public Int32 GetInt32()
        {
            return (Int32)GetSignedLEB128(32);
        }

        public Int64 GetInt64()
        {
            return (Int64)GetSignedLEB128(64);
        }

        public UInt64 GetUInt64()
        {
            UInt64 result = 0;
            byte shift = 0;
            while (true)
            {
                byte b = GetByte();
                result |= (UInt64)(b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }

            return result;
        }

        public float GetF32()
        {
            float result = BitConverter.ToSingle(bytes, (int)index);
            index += 4;
            return result;
        }

        public double GetF64()
        {
            double result = BitConverter.ToDouble(bytes, (int)index);
            index += 8;
            return result;
        }

        public Table GetTableType()
        {
            byte elemType = GetElemType();
            UInt32 min, max;
            GetLimits(out min, out max);

            return new Table(elemType, min, max);
        }

        public Memory GetMemType()
        {
            UInt32 min = 0, max = 0;
            GetLimits(out min, out max);

            return new Memory((int)min, (int)max);
        }

        public bool GetLimits(out UInt32 min, out UInt32 max)
        {
            if (GetBoolean())
            {
                min = GetUInt32();
                max = GetUInt32();
                return true;
            }
            else
            {
                min = GetUInt32();
                max = 0;
                return false;
            }
        }

        public byte GetElemType()
        {
            var type = GetByte();

            switch (type)
            {
                case 0x70: //funcref
                    break;
                default:
                    throw new Exception("Invalid element type: 0x" + type.ToString("X"));
            }

            return type;
        }

        public bool GetBoolean()
        {
            byte b = GetByte();

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

        public void GetGlobalType(out byte type, out bool mutable)
        {
            type = GetValType();
            mutable = GetBoolean();
        }

        public byte GetBlockType()
        {
            if (bytes[index] == 0x40)
            {
                index++;
                return 0x40;
            }
            else
            {
                return GetValType();
            }
        }

        public byte GetValType()
        {
            byte valType = GetByte();

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

        public string GetName()
        {
            var length = GetUInt32();
            byte[] sub = new byte[length];
            Array.Copy(bytes, index, sub, 0, length);
            string result = System.Text.Encoding.UTF8.GetString(sub);
            index += length;

            return result;
        }

        public UInt32 GetVersion()
        {
            index += 4;
            return BitConverter.ToUInt32(bytes, 4);
        }

        public void Skip(UInt32 size)
        {
            index += size;
        }

        private Int64 GetSignedLEB128(byte size)
        {
            UInt64 result = 0;
            byte shift = 0;
            byte b;
            do
            {
                b = bytes[index++];
                result |= (((UInt64)0x7F & b) << shift);
                shift += 7;
            } while ((b & 0x80) != 0);

            /* sign bit of byte is second high order bit (0x40) */
            if ((shift < size) && ((b & 0x40) != 0))
                /* sign extend */
                result |= (~(UInt64)0 << shift);
            return (Int64)result;
        }


    }
}
