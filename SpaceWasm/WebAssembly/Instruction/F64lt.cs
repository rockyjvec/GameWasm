using System;

namespace WebAssembly.Instruction
{
    internal class F64lt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

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

        public F64lt(Parser parser) : base(parser, true)
        {
        }
    }
}