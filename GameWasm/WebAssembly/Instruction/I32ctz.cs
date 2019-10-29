using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32ctz : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI32();

            UInt32 bits = 0;
            UInt32 compare = 1;
            while(bits < 32)
            {
                if((compare & a) == 0)
                {
                    bits++;
                    compare <<= 1;
                }
                else
                {
                    break;
                }
            }

            f.PushI32(bits);

            return Next;
        }

        public I32ctz(Parser parser, Function f) : base(parser, f, true)
        {

        }
    }
}