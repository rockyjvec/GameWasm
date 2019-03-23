using System;

namespace WebAssembly.Instruction
{
    internal class F32convertI32s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)(Int32)store.Stack.PopI32());
            return this.Next;
        }
        public F32convertI32s(Parser parser) : base(parser, true)
        {
        }
    }
}