using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32floor : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF32();

            f.PushF32((float)Math.Floor(a));
            return Next;
        }

        public F32floor(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}