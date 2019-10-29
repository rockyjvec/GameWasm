using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64popcnt : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI64();

            UInt64 bits = 0;
            UInt64 compare = 1;
            while (true)
            {
                if ((compare & a) != 0)
                {
                    bits++;
                }
                if (compare == 0x8000000000000000) break;
                compare <<= 1;
            }

            f.PushI64(bits);

            return Next;
        }

        public I64popcnt(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}