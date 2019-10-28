using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64divs : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI64();
            var a = f.PopI64();

            if ((Int64)b == 0) throw new Trap("integer divide by zero");

            try
            {
                f.Push((UInt64)((Int64)a / (Int64)b));
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return Next;
        }

        public I64divs(Parser parser) : base(parser, true)
        {
        }
    }
}