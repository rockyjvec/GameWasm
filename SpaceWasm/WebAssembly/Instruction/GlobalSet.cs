using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class GlobalSet : Instruction
    {
        int globalidx;

        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Module.Globals[globalidx].Set(store.Stack.PopValue());

            return this.Next;
        }

        public GlobalSet(Parser parser) : base(parser, true)
        {
            globalidx = (int)parser.GetUInt32();
        }
    }
}
