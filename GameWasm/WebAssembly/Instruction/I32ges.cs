using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32ges : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (Int32)f.PopI32();
            var a = (Int32)f.PopI32();

            if (a >= b)
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public I32ges(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
