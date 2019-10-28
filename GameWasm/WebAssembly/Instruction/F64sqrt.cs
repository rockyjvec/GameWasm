using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64sqrt : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();

            f.Push((double)Math.Sqrt((double)b));

            return Next;
        }

        public F64sqrt(Parser parser) : base(parser, true)
        {
        }
    }
}