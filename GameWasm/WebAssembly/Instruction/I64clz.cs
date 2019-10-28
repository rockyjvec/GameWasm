using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64clz : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI64();

            UInt64 bits = 0;
            UInt64 compare = 0x8000000000000000;
            while (bits < 64)
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

            f.Push(bits);

            return Next;
        }

        public I64clz(Parser parser) : base(parser, true)
        {
        }
    }
}