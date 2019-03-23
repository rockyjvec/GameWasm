using System;

namespace WebAssembly.Instruction
{
    internal class F32floor : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF32();

            store.Stack.Push((float)Math.Floor(a));
            return this.Next;
        }

        public F32floor(Parser parser) : base(parser, true)
        {
        }
    }
}