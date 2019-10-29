using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32sqrt : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();

            f.PushF32((float)Math.Sqrt((float)b));

            return Next;
        }

        public F32sqrt(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}