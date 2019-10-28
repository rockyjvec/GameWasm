using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF32u : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt32)Math.Truncate((float)f.PopF32()));

            return Next;
        }

        public I32truncF32u(Parser parser) : base(parser, true)
        {
        }
    }
}