using System;

namespace WebAssembly.Instruction
{
    internal class I32divs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

            if ((Int32)b == 0) throw new Trap("integer divide by zero");

            try
            {
                store.Stack.Push((UInt32)((Int32)a / (Int32)b));
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            
            return this.Next;
        }

        public I32divs(Parser parser) : base(parser, true)
        {
        }
    }
}