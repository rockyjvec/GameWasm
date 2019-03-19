using System;

namespace WebAssembly.Instruction
{
    internal class I64rems : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int64)store.Stack.PopI64();
            var a = (Int64)store.Stack.PopI64();

            store.Stack.Push(a % b);

            return this.Next;
        }

        public I64rems(Parser parser) : base(parser, true)
        {
        }
    }
}