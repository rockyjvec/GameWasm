using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
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
                index = this.table[(int)index];
            }

            Stack.Label l = store.Stack.PopLabel(index + 1);

            return l.Instruction.Next;
        }

        public BrTable(Parser parser) : base(parser, true)
        {
            UInt32 vectorSize = parser.GetUInt32();
            this.table = new UInt32[vectorSize];
            for(int i = 0; i < vectorSize; i++)
            {
                this.table[i] = parser.GetIndex();
            }

            this.defaultLabelidx = (UInt32)parser.GetIndex();
        }
    }
}
