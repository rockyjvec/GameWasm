using System;

namespace WebAssembly.Instruction
{
    internal class I64rotl : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            store.Stack.Push((UInt64)((a << (int)b) | (a >> (32 - (int)b))));
            return this.Next;
        }

        public I64rotl(Parser parser) : base(parser, true)
        {
        }
    }
}