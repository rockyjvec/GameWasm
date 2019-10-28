using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64min : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            f.Push((double)Math.Min(a, b));
            return Next;
        }

        public F64min(Parser parser) : base(parser, true)
        {
        }
    }
}