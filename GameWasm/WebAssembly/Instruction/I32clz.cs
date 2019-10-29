using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32clz : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI32();

            UInt32 bits = 0;
            UInt32 compare = 0x80000000;
            while (bits < 32)
            {
                if ((compare & a) == 0)
                {
                    bits++;
                    compare >>= 1;
                }
                else
                {
                    break;
                }
            }

            f.PushI32(bits);

            return Next;
        }

        public I32clz(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}