using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64eqz : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI64() == 0)
            {
                store.Stack.Push(true);
            }
            else
            {
                store.Stack.Push(false);
            }

            return this.Next;
        }

        public I64eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
