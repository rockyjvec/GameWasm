using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32floor : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF32();

            f.Push((float)Math.Floor(a));
            return Next;
        }

        public F32floor(Parser parser) : base(parser, true)
        {
        }
    }
}