using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrTable : Instruction
    {
        UInt32 defaultLabelidx;
        UInt32[] table;

        public override Instruction Run(Store store)
        {
            UInt32 index = store.Stack.PopI32();

            if(index >= table.Length)
            {
                index = defaultLabelidx;
            }
            else
            {
                index = table[(int)index];
            }

            Stack.Label l = store.Stack.PopLabel(index + 1);

            if (l.Instruction as Loop != null) return l.Instruction;
            return l.Instruction.Next;
        }

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
    }
}
