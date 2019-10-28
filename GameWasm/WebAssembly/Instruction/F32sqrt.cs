using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32sqrt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();

            store.Stack.Push((float)Math.Sqrt((float)b));

            return Next;
        }

        public F32sqrt(Parser parser) : base(parser, true)
        {
        }
    }
}