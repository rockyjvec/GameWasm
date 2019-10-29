using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32ne : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopI32() != f.PopI32())
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public I32ne(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
