using System;

namespace WebAssembly.Instruction
{
    internal class F32abs : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Abs(store.Stack.PopF32()));
            return this.Next;
        }
        public F32abs(Parser parser) : base(parser, true)
        {
        }
    }
}