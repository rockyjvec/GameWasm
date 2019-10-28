using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64nearest : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF64();

            f.Push((double)Math.Round(a));
            return Next;
        }

        public F64nearest(Parser parser) : base(parser, true)
        {
        }
    }
}