using System;

namespace WebAssembly.Instruction
{
    internal class I32clz : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopI32();

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

            store.Stack.Push(bits);

            return this.Next;
        }

        public I32clz(Parser parser) : base(parser, true)
        {
        }
    }
}