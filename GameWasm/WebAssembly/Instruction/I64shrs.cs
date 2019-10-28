using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64shrs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI64();
            var a = (Int64)store.Stack.PopI64();

            store.Stack.Push((UInt64)(a >> b));
            return Next;
        }

        public I64shrs(Parser parser) : base(parser, true)
        {
        }
    }
}