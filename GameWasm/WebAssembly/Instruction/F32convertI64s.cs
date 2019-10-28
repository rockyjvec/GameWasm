using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI64s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)(Int64)store.Stack.PopI64());
            return Next;
        }

        public F32convertI64s(Parser parser) : base(parser, true)
        {
        }
    }
}