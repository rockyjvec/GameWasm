using System;

namespace WebAssembly.Instruction
{
    internal class F64eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopF64() == store.Stack.PopF64())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return this.Next;
        }

        public F64eq(Parser parser) : base(parser, true)
        {
        }
    }
}