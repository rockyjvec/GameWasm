using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalTee : Instruction
    {
        public int localidx;

        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Locals[localidx] = store.Stack.Peek();

            return this.Next;
        }

        public LocalTee(Parser parser) : base(parser, true)
        {
            this.localidx = (int)parser.GetUInt32();
        }
    }
}
