using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI32s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)(Int32)store.Stack.PopI32());
            return Next;
        }

        public F64convertI32s(Parser parser) : base(parser, true)
        {
        }
    }
}