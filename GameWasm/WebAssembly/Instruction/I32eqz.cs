using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32eqz : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopI32() == 0)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I32eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
