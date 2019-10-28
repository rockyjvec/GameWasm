using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32trunc : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((float)Math.Truncate((float)f.PopF32()));

            return Next;
        }


        public F32trunc(Parser parser) : base(parser, true)
        {
        }
    }
}