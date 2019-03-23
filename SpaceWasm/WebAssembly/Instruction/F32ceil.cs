using System;

namespace WebAssembly.Instruction
{
    internal class F32ceil : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Ceiling(store.Stack.PopF32()));
            return this.Next;
        }

        public F32ceil(Parser parser) : base(parser, true)
        {
        }
    }
}