using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32floor : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF32();

            store.Stack.Push((float)Math.Floor(a));
            return Next;
        }

        public F32floor(Parser parser) : base(parser, true)
        {
        }
    }
}