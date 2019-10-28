using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64floor : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF64();

            f.Push((double)Math.Floor(a));
            return Next;
        }

        public F64floor(Parser parser) : base(parser, true)
        {
        }
    }
}