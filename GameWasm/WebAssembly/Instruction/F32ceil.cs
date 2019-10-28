using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32ceil : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)Math.Ceiling(f.PopF32()));
            return Next;
        }

        public F32ceil(Parser parser) : base(parser, true)
        {
        }
    }
}