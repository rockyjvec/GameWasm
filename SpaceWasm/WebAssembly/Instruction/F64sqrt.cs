using System;

namespace WebAssembly.Instruction
{
    internal class F64sqrt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();

            store.Stack.Push((double)Math.Sqrt((double)b));

            return this.Next;
        }

        public F64sqrt(Parser parser) : base(parser, true)
        {
        }
    }
}