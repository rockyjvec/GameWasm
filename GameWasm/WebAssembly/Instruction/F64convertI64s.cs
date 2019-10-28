using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)(Int64)store.Stack.PopI64());
            return Next;
        }
        public F64convertI64s(Parser parser) : base(parser, true)
        {
        }
    }
}