using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32abs : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Abs(store.Stack.PopF32()));
            return Next;
        }
        public F32abs(Parser parser) : base(parser, true)
        {
        }
    }
}