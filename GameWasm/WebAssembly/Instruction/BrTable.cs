using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrTable : Instruction
    {
        public UInt32 defaultLabelidx;
        public UInt32[] table;

        public BrTable(Parser parser) : base(parser, true)
        {
            UInt32 vectorSize = parser.GetUInt32();
            table = new UInt32[vectorSize];
            for(int i = 0; i < vectorSize; i++)
            {
                table[i] = parser.GetIndex();
            }

            defaultLabelidx = (UInt32)parser.GetIndex();
        }

        public override string ToString()
        {
            return "br_table";
        }
    }
}
