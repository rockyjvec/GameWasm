using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF64u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt32)Math.Truncate((double)f.PopF64()));

            return Next;
        }

        public I32truncF64u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}