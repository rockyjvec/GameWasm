using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalSet : Instruction
    {
        int localidx;

        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Locals[localidx] = store.Stack.Pop();
            return this.Next;
        }

        public LocalSet(Parser parser) : base(parser, true)
        {
            this.localidx = (int)parser.GetUInt32();
        }
    }
}
