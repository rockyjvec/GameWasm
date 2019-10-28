using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32ceil : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)Math.Ceiling(store.Stack.PopF32()));
            return Next;
        }

        public F32ceil(Parser parser) : base(parser, true)
        {
        }
    }
}