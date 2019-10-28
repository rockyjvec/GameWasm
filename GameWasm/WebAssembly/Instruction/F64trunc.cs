using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64trunc : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)Math.Truncate((double)store.Stack.PopF64()));

            return Next;
        }

        public F64trunc(Parser parser) : base(parser, true)
        {
        }
    }
}