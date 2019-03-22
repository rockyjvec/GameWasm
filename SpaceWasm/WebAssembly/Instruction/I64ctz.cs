using System;

namespace WebAssembly.Instruction
{
    internal class I64ctz : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopI64();

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

            store.Stack.Push(bits);

            return this.Next;
        }

        public I64ctz(Parser parser) : base(parser, true)
        {
        }
    }
}