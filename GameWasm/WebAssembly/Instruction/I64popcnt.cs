using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64popcnt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopI64();

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

            store.Stack.Push(bits);

            return Next;
        }

        public I64popcnt(Parser parser) : base(parser, true)
        {
        }
    }
}