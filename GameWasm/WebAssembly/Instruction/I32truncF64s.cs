using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF64s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt32)(Int32)Math.Truncate((double)f.PopF64()));

            return Next;
        }

        public I32truncF64s(Parser parser) : base(parser, true)
        {
        }
    }
}