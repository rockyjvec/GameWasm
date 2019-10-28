using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64truncF32u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt64)Math.Truncate((float)f.PopF32()));

            return Next;
        }

        public I64truncF32u(Parser parser) : base(parser, true)
        {
        }
    }
}