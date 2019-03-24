using System;

namespace WebAssembly.Instruction
{
    internal class I64divs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            if ((Int64)b == 0) throw new Trap("integer divide by zero");

            try
            {
                store.Stack.Push((UInt64)((Int64)a / (Int64)b));
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return this.Next;
        }

        public I64divs(Parser parser) : base(parser, true)
        {
        }
    }
}