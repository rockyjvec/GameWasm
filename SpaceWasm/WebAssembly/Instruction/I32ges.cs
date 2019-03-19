using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32ges : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int32)store.Stack.PopI32();
            var a = (Int32)store.Stack.PopI32();

            if (a >= b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public I32ges(Parser parser) : base(parser, true)
        {
        }
    }
}
