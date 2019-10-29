using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI64s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)(Int64)f.PopI64());
            return Next;
        }

        public F32convertI64s(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}