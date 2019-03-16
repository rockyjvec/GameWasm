using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class GlobalGet : Instruction
    {
        int globalidx;

        public override Instruction Run(Store store)
        {
            store.Stack.PushValue(store.CurrentFrame.Module.Globals[globalidx]);

            return this.Next;
        }

        public GlobalGet(Parser parser) : base(parser, true)
        {
            globalidx = (int)parser.GetUInt32();
        }
    }
}