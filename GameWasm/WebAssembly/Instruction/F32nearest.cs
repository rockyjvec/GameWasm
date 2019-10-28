using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32nearest : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF32();

            f.Push((float)Math.Round(a));
            return Next;
        }

        public F32nearest(Parser parser) : base(parser, true)
        {
        }
    }
}