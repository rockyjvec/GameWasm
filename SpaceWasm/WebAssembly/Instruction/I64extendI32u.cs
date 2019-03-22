using System;

namespace WebAssembly.Instruction
{
    internal class I64extendI32u : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            store.Stack.Push((UInt64)b);

            return this.Next;
        }

        public I64extendI32u(Parser parser) : base(parser, true)
        {
        }
    }
}