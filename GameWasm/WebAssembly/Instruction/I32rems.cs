using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32rems : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (Int32)f.PopI32();
            var a = (Int32)f.PopI32();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                if((UInt32)a == 0x80000000 && (UInt32)b == 0xFFFFFFFF)
                {
                    f.Push((UInt32)0);
                }
                else
                {
                    f.Push((UInt32)(a % b));
                }
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }


            return Next;
        }

        public I32rems(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}