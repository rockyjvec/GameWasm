using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64ctz : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI64();

            UInt64 bits = 0;
            UInt64 compare = 1;
            while (bits < 64)
            {
                if ((compare & a) == 0)
                {
                    bits++;
                    compare <<= 1;
                }
                else
                {
                    break;
                }
            }

            f.PushI64(bits);

            return Next;
        }

        public I64ctz(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}