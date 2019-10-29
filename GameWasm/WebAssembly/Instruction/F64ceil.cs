using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64ceil : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64((double)Math.Ceiling(f.PopF64()));
            return Next;
        }

        public F64ceil(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}