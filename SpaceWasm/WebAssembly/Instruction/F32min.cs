using System;

namespace WebAssembly.Instruction
{
    internal class F32min : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            store.Stack.Push((float)Math.Min(a, b));
            return this.Next;
        }

        public F32min(Parser parser) : base(parser, true)
        {
        }
    }
}