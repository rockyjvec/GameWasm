using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32popcnt : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI32();

            UInt32 bits = 0;
            UInt32 compare = 1;
            while (true)
            {
                if ((compare & a) != 0)
                {
                    bits++;
                }
                if (compare == 0x80000000) break;
                compare <<= 1;
            }

            f.PushI32(bits);

            return Next;
        }

        public I32popcnt(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}