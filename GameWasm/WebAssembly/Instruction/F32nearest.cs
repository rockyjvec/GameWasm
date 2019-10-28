using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32nearest : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF32();

            store.Stack.Push((float)Math.Round(a));
            return Next;
        }

        public F32nearest(Parser parser) : base(parser, true)
        {
        }
    }
}