using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    internal class I32eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI32() == store.Stack.PopI32())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public I32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
