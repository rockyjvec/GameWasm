using System;

namespace WebAssembly.Instruction
{
    internal class I32rems : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int32)store.Stack.PopI32();
            var a = (Int32)store.Stack.PopI32();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                if((UInt32)a == 0x80000000 && (UInt32)b == 0xFFFFFFFF)
                {
                    store.Stack.Push((UInt32)0);
                }
                else
                {
                    store.Stack.Push((UInt32)(a % b));
                }
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }


            return this.Next;
        }

        public I32rems(Parser parser) : base(parser, true)
        {
        }
    }
}