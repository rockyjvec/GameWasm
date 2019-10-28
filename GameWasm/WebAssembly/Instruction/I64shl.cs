using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64shl : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI64();
            var a = (UInt64)store.Stack.PopI64();

            store.Stack.Push(a << b);
            return Next;
        }

        public I64shl(Parser parser) : base(parser, true)
        {
        }
    }
}