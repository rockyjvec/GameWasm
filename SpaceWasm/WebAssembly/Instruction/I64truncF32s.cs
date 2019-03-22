using System;

namespace WebAssembly.Instruction
{
    internal class I64truncF32s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt64)(Int64)Math.Truncate((float)store.Stack.PopF32()));

            return this.Next;
        }

        public I64truncF32s(Parser parser) : base(parser, true)
        {
        }
    }
}