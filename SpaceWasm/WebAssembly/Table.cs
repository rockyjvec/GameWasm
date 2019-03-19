using System;

namespace WebAssembly
{
    public class Table
    {
        public byte Type = 0x70;
        UInt32[] table;
        public UInt32 MinSize = 0, MaxSize = 0, CurrentSize = 0;

        public Table(byte type, UInt32 minSize, UInt32 maxSize)
        {
            this.Type = type;
            this.MinSize = minSize;
            this.MaxSize = maxSize;
            this.CurrentSize = this.MinSize;

            this.table = new UInt32[this.CurrentSize];
        }

        public bool CompatibleWith(Table t)
        {
            return this.MinSize == t.MinSize && this.MaxSize == t.MaxSize && this.Type == t.Type;
        }

        public override string ToString()
        {
            return "<table type: 0x" + this.Type.ToString("X") + ", min: " + this.MinSize + ", max: " + this.MaxSize + ", cur: " + this.CurrentSize + ">";
        }

        public void Set(UInt32 offset, UInt32 funcidz)
        {
            if(offset >= this.table.Length)
            {
                throw new Exception("Invalid table offset");
            }

            this.table[offset] = funcidz;
        }

        public UInt32 Get(UInt32 offset)
        {
            if (offset >= this.table.Length)
            {
                throw new Exception("Invalid table offset");
            }

            return this.table[offset];
        }
    }
}
