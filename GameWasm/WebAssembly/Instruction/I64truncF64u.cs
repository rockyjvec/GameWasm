using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64truncF64u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt64)Math.Truncate((double)store.Stack.PopF64()));

            return Next;
        }

        public I64truncF64u(Parser parser) : base(parser, true)
        {
        }
    }
}