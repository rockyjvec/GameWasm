using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64rems : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int64)store.Stack.PopI64();
            var a = (Int64)store.Stack.PopI64();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                if ((UInt64)a == 0x8000000000000000 && (UInt64)b == 0xFFFFFFFFFFFFFFFF)
                {
                    store.Stack.Push((UInt64)0);
                }
                else
                {
                    store.Stack.Push((UInt64)(a % b));
                }
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return Next;
        }

        public I64rems(Parser parser) : base(parser, true)
        {
        }
    }
}