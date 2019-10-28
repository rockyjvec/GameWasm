using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64abs : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((double)Math.Abs(f.PopF64()));
            return Next;
        }
        public F64abs(Parser parser) : base(parser, true)
        {
        }
    }
}