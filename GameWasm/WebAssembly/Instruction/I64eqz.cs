using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64eqz : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            if (f.PopI64() == 0)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I64eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
