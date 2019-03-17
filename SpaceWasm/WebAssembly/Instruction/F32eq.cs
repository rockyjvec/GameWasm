using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopF32() == store.Stack.PopF32())
            {
                store.Stack.Push(true);
            }
            else
            {
                store.Stack.Push(false);
            }

            return this.Next;
        }

        public F32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
