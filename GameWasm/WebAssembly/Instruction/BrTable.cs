using System;

namespace GameWasm.Webassembly.Instruction
{
    class BrTable : Instruction
    {
        UInt32 defaultLabelidx;
        UInt32[] table;

        protected override Instruction Run(Stack.Frame f)
        {
            UInt32 index = f.PopI32();

            if(index >= table.Length)
            {
                index = defaultLabelidx;
            }
            else
            {
                index = table[(int)index];
            }

            Stack.Label l = f.PopLabel(index + 1);

            if (l.Instruction as Loop != null) return l.Instruction;
            return l.Instruction.Next;
        }

        public BrTable(Parser parser, Function f) : base(parser, f, true)
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
