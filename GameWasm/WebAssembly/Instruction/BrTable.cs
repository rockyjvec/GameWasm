using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrTable : Instruction
    {
        public int defaultLabelidx;
        public int[] table;

        public BrTable(Parser parser) : base(parser, true)
        {
            UInt32 vectorSize = parser.GetUInt32();
            table = new int[vectorSize];
            for(int i = 0; i < vectorSize; i++)
            {
                table[i] = (int)parser.GetIndex() + 1;
            }

            defaultLabelidx = (int)parser.GetIndex() + 1;
        }

        public override string ToString()
        {
            return "br_table";
        }
    }
}
