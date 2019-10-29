using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64eq : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopI64() == f.PopI64())
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public I64eq(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
