using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32eq : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopF32() == f.PopF32())
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public F32eq(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
