using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64truncF64s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt64)(Int64)Math.Truncate((double)f.PopF64()));

            return Next;
        }

        public I64truncF64s(Parser parser) : base(parser, true)
        {
        }
    }
}