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
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public I64eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
