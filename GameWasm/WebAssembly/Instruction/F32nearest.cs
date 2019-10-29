using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32nearest : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF32();

            f.PushF32((float)Math.Round(a));
            return Next;
        }

        public F32nearest(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}