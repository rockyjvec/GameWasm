using System;

namespace WebAssembly.Instruction
{
    internal class F64convertI64s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)(Int64)store.Stack.PopI64());
            return this.Next;
        }
        public F64convertI64s(Parser parser) : base(parser, true)
        {
        }
    }
}