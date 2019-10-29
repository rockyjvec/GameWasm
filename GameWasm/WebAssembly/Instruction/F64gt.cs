using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64gt : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            if (a > b)
            {
                f.PushI32((UInt32)1);
            }
            else
            {
                f.PushI32((UInt32)0);
            }

            return Next;
        }

        public F64gt(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}