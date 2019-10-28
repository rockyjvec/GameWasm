using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64trunc : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((double)Math.Truncate((double)f.PopF64()));

            return Next;
        }

        public F64trunc(Parser parser) : base(parser, true)
        {
        }
    }
}