using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32abs : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)Math.Abs(f.PopF32()));
            return Next;
        }
        public F32abs(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}