using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64ne : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopI64() != f.PopI64())
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public I64ne(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
