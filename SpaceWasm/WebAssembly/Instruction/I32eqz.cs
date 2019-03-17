using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32eqz : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI32() == 0)
            {
                store.Stack.Push(true);
            }
            else
            {
                store.Stack.Push(false);
            }

            return this.Next;
        }

        public I32eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
