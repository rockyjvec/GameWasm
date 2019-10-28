using System;

namespace GameWasm.Webassembly
{
    public class Table
    {
        public byte Type = 0x70;
        UInt32[] table;
        public UInt32 MinSize = 0, MaxSize = 0, CurrentSize = 0;

        public Table(byte type, UInt32 minSize, UInt32 maxSize)
        {
            Type = type;
            MinSize = minSize;
            MaxSize = maxSize;
            CurrentSize = MinSize;

            table = new UInt32[CurrentSize];
        }

        public bool CompatibleWith(Table t)
        {
            return MinSize == t.MinSize && MaxSize == t.MaxSize && Type == t.Type;
        }

        public override string ToString()
        {
            return "<table type: 0x" + Type.ToString("X") + ", min: " + MinSize + ", max: " + MaxSize + ", cur: " + CurrentSize + ">";
        }

        public void Set(UInt32 offset, UInt32 funcidz)
        {
            if(offset >= table.Length)
            {
                throw new Exception("Invalid table offset");
            }

            table[offset] = funcidz;
        }

        public UInt32 Get(UInt32 offset)
        {
            if (offset >= table.Length)
            {
                throw new Trap("undefined element");
            }

            return table[offset];
        }
    }
}
