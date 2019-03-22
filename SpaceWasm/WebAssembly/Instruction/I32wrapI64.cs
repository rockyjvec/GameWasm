using System;


namespace WebAssembly.Instruction
{
    internal class I32wrapI64 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)store.Stack.PopI64());
            return this.Next;
        }

        public I32wrapI64(Parser parser) : base(parser, true)
        {
        }
    }
}