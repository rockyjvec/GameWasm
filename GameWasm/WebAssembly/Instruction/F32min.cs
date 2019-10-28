using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32min : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();
            var a = f.PopF32();

            f.Push((float)Math.Min(a, b));
            return Next;
        }

        public F32min(Parser parser) : base(parser, true)
        {
        }
    }
}