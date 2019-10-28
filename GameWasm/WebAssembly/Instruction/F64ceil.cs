using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64ceil : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((double)Math.Ceiling(f.PopF64()));
            return Next;
        }

        public F64ceil(Parser parser) : base(parser, true)
        {
        }
    }
}