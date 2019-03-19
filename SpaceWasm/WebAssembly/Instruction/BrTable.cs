using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class BrTable : Instruction
    {
        int defaultLabelidx;
        int[] table;

        public override Instruction Run(Store store)
        {
            int index = (int)store.Stack.PopI32();

            if(index >= store.CurrentFrame.Labels.Count())
            {
                index = defaultLabelidx;
            }

            Instruction i = store.CurrentFrame.Labels.Pop();
            for (int j = 0; j < index - 1; j++)
            {
                i = store.CurrentFrame.Labels.Pop();
            }

            return i;
        }

        public BrTable(Parser parser) : base(parser, true)
        {
            UInt32 vectorSize = parser.GetUInt32();
            this.table = new int[vectorSize];
            for(int i = 0; i < vectorSize; i++)
            {
                this.table[i] = (int)parser.GetIndex();
            }

            this.defaultLabelidx = (int)parser.GetIndex();
        }
    }
}
