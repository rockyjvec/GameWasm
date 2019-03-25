using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    public class Memory
    {
        public byte[][] Buffer;
        public UInt64 MinPages = 0, MaxPages = 0, CurrentPages = 0;


        public Memory(UInt32 minPages, UInt32 maxPages)
        {
            this.MinPages = minPages;
            this.MaxPages = maxPages;
            this.CurrentPages = this.MinPages;

            this.Buffer = new byte[this.MinPages][];
            for (int i = 0; i < (int)this.CurrentPages; i++)
            {
                this.Buffer[i] = new byte[65536];
                Array.Clear(this.Buffer[i], 0, 65536);                
            }
        }

        public bool CompatibleWith(Memory m)
        {
            return this.MinPages == m.MinPages && this.MaxPages == m.MaxPages;
        }

        public override string ToString()
        {
            return "<memory min: " + this.MinPages + ", max: " + this.MaxPages + ", cur: " + this.CurrentPages + ">";
        }



        public void Set(UInt64 offset, byte b)
        {
            if (offset > (this.CurrentPages >> 16) - 1)
                throw new Trap("out of bounds memory access");

            this.Buffer[offset >> 16][offset & 0xFFFF] = b;
        }

        public byte[] GetBytes(UInt64 offset, UInt64 bytes)
        {
            if ((offset + bytes) > (this.CurrentPages << 16))
                throw new Trap("out of bounds memory access", "" + offset + " > " + ((this.CurrentPages * 65536) - (UInt64)bytes));

            byte[] buffer = new byte[bytes];
            for(UInt64 i = offset; i < offset + (UInt64)bytes; i++)
            {
                buffer[i - offset] = this.Buffer[i >> 16][i & 0xFFFF];
            }

            return buffer;
        }

        public void SetBytes(UInt64 offset, byte[] bytes)
        {
            if ((offset + (UInt64)bytes.Length) > (this.CurrentPages << 16))
                throw new Trap("out of bounds memory access", "" + offset + " > " +((this.CurrentPages * 65536) - (UInt64)bytes.Length));

            for (UInt64 i = offset; i < offset + (UInt64)bytes.Length; i++)
            {
                this.Buffer[i >> 16][i & 0xFFFF] = bytes[i - offset];
            }
        }

        public float GetF32(UInt64 offset)
        {
            return BitConverter.ToSingle(this.GetBytes(offset, 4), 0);
        }

        public double GetF64(UInt64 offset)
        {
            return BitConverter.ToDouble(this.GetBytes(offset, 8), 0);
        }

        public UInt32 GetI32(UInt64 offset)
        {
            return BitConverter.ToUInt32(this.GetBytes(offset, 4), 0);
        }

        public void SetI32(UInt64 offset, UInt32 value)
        {
            this.SetBytes(offset, BitConverter.GetBytes(value));
        }

        public UInt64 GetI64(UInt64 offset)
        {
            return BitConverter.ToUInt64(this.GetBytes(offset, 8), 0);
        }

        public UInt32 GetI3216s(UInt64 offset)
        {
            return (UInt32)((Int32)BitConverter.ToInt16(this.GetBytes(offset, 2), 0));
        }

        public UInt32 GetI3216u(UInt64 offset)
        {
            return (UInt32)BitConverter.ToUInt16(this.GetBytes(offset, 2), 0);
        }

        public UInt32 GetI328s(UInt64 offset)
        {
            return (UInt32)(Int32)(sbyte)this.GetBytes(offset, 1)[0];
        }

        public UInt32 GetI328u(UInt64 offset)
        {
            return (UInt32)this.GetBytes(offset, 1)[0];
        }

        public UInt64 GetI6416s(UInt64 offset)
        {
            return (UInt64)((Int64)BitConverter.ToInt16(this.GetBytes(offset, 2), 0));
        }

        public UInt64 GetI6416u(UInt64 offset)
        {
            return (UInt64)BitConverter.ToUInt16(this.GetBytes(offset, 2), 0);
        }

        public UInt64 GetI6432s(UInt64 offset)
        {
            return (UInt64)((Int64)BitConverter.ToInt32(this.GetBytes(offset, 4), 0));
        }

        public UInt64 GetI6432u(UInt64 offset)
        {
            return (UInt64)BitConverter.ToUInt32(this.GetBytes(offset, 4), 0);
        }

        public UInt64 GetI648s(UInt64 offset)
        {
            return (UInt64)(Int64)(sbyte)this.GetBytes(offset, 1)[0];
        }

        public UInt64 GetI648u(UInt64 offset)
        {
            return (UInt64)this.GetBytes(offset, 1)[0];
        }

        public UInt32 Grow(UInt32 size)
        {

            if (this.MaxPages != 0 && (this.CurrentPages + size) > this.MaxPages)
            {
                return 0xFFFFFFFF;

            }
            else if ((this.CurrentPages + size) < this.CurrentPages)
            {
                return 0xFFFFFFFF;
            }
            else if ((this.CurrentPages + size) == this.CurrentPages)
            {
                return (UInt32)this.CurrentPages;
            }
            else
            {
                byte[][] resized = new byte[size + this.CurrentPages][];

                for (UInt64 i = 0; i < this.CurrentPages; i++)
                {
                    Array.Copy(this.Buffer[i], 0, resized[i] = new byte[65536], 0, 65536);
                }

                for (UInt64 i = this.CurrentPages; i < size + this.CurrentPages; i++)
                {
                    resized[i] = new byte[65536];
                    Array.Clear(resized[i], 0, 65536);
                }

                this.Buffer = resized;
                this.CurrentPages += size;
                return (UInt32)(this.CurrentPages - size);
            }
        }
    }
}
