using System;

namespace WebAssembly.Instruction
{
    internal class I32shru : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI32();
            var a = (UInt32)store.Stack.PopI32();

            store.Stack.Push(a >> b);
            return this.Next;
        }

        public I32shru(Parser parser) : base(parser, true)
        {
        }
    }
}