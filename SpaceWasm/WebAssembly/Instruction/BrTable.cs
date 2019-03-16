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
