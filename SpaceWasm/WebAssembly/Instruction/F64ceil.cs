using System;

namespace WebAssembly.Instruction
{
    internal class F64ceil : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)Math.Ceiling(store.Stack.PopF64()));
            return this.Next;
        }

        public F64ceil(Parser parser) : base(parser, true)
        {
        }
    }
}