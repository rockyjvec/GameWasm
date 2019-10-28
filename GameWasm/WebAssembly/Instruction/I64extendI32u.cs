using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64extendI32u : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            store.Stack.Push((UInt64)b);

            return Next;
        }

        public I64extendI32u(Parser parser) : base(parser, true)
        {
        }
    }
}