using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32lt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            if (a < b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public F32lt(Parser parser) : base(parser, true)
        {
        }
    }
}
