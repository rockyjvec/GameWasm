using System;

namespace WebAssembly.Instruction
{
    internal class I32shrs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI32();
            var a = (Int32)store.Stack.PopI32();

            store.Stack.Push((UInt32)(a >> b));
            return this.Next;
        }

        public I32shrs(Parser parser) : base(parser, true)
        {
        }
    }
}