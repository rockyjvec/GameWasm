using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32trunc : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Truncate((float)store.Stack.PopF32()));

            return Next;
        }


        public F32trunc(Parser parser) : base(parser, true)
        {
        }
    }
}