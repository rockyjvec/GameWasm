using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64min : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            f.PushF64((double)Math.Min(a, b));
            return Next;
        }

        public F64min(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}