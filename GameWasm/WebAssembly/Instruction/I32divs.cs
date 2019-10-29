using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32divs : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            if ((Int32)b == 0) throw new Trap("integer divide by zero");

            try
            {
                f.PushI32((UInt32) ((Int32) a / (Int32) b));
            }
            catch (System.DivideByZeroException)
            {
                throw new Trap("integer overflow");
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            
            return Next;
        }

        public I32divs(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}