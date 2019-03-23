using System;

namespace WebAssembly.Instruction
{
    internal class F64abs : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)Math.Abs(store.Stack.PopF64()));
            return this.Next;
        }
        public F64abs(Parser parser) : base(parser, true)
        {
        }
    }
}