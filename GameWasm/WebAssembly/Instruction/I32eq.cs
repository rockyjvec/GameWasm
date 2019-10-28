using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32eq : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopI32() == f.PopI32())
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
