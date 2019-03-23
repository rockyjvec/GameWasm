using System;

namespace WebAssembly.Instruction
{
    internal class I32rotl : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

            store.Stack.Push((UInt32)((a << (int)b) | (a >> (32 - (int)b))));
            return this.Next;
        }

        public I32rotl(Parser parser) : base(parser, true)
        {
        }
    }
}