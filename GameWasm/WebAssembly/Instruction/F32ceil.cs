using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32ceil : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)Math.Ceiling(f.PopF32()));
            return Next;
        }

        public F32ceil(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}