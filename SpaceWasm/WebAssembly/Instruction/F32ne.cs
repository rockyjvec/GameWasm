using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32ne : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopF32() != store.Stack.PopF32())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public F32ne(Parser parser) : base(parser, true)
        {
        }
    }
}
