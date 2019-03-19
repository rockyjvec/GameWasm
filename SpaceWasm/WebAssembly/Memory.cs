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
                this.Buffer[i] = new byte[65536];
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
            UInt32 page = (UInt32)(offset / 65536);
            UInt32 remainder = (UInt32)(offset % 65536);
            if (offset > this.CurrentPages * 65536 - 1)
                throw new Trap("out of bounds memory access");
            this.Buffer[page][remainder] = b;
        }

        public byte[] GetBytes(UInt64 offset, int bytes)
        {
            if (offset > ((this.CurrentPages * 65536) - (UInt64)bytes))
                throw new Trap("out of bounds memory access", "" + offset + " > " + ((this.CurrentPages * 65536) - (UInt64)bytes));

            byte[] buffer = new byte[bytes];
            for(UInt64 index = offset; index < offset + (UInt64)bytes; index++)
            {
                UInt32 page = (UInt32)(index / 65536);
                UInt32 remainder = (UInt32)(index % 65536);
                buffer[(int)(index - offset)] = this.Buffer[page][remainder];
            }

            return buffer;
        }

        public void SetBytes(UInt64 offset, byte[] bytes)
        {
            if (offset > ((this.CurrentPages * 65536) - (UInt64)bytes.Length))
                throw new Trap("out of bounds memory access", "" + offset + " > " +((this.CurrentPages * 65536) - (UInt64)bytes.Length));

            for (UInt64 i = 0; i < (UInt64)bytes.Length; i++)
            {
                UInt64 index = offset + i;
                UInt32 page = (UInt32)(index / 65536);
                UInt32 remainder = (UInt32)(index % 65536);

                this.Buffer[page][remainder] = bytes[i];
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
            return (UInt32)((Int32)this.GetBytes(offset, 1)[0]);
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
            return (UInt64)((Int64)this.GetBytes(offset, 1)[0]);
        }

        public UInt64 GetI648u(UInt64 offset)
        {
            return (UInt64)this.GetBytes(offset, 1)[0];
        }

        public UInt32 Grow(UInt32 size)
        {

            if (this.MaxPages != 0 && size > this.MaxPages)
            {
                return size - 1;

            }
            else if (size < this.CurrentPages)
            {
                return size - 1;
            }
            else if (size == this.CurrentPages)
            {
                return size;
            }
            else
            {
                byte[][] resized = new byte[size][];
                for(int i = 0; i < (int)this.CurrentPages; i++)
                    Array.Copy(this.Buffer[i], 0, resized[i], 0, 65536);
                this.Buffer = resized;
                this.CurrentPages = size;
                return size;
            }
        }
    }
}
