using System;

namespace WebAssembly.Instruction
{
    internal class I32rems : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int32)store.Stack.PopI32();
            var a = (Int32)store.Stack.PopI32();

            store.Stack.Push(a % b);

            return this.Next;
        }

        public I32rems(Parser parser) : base(parser, true)
        {
        }
    }
}