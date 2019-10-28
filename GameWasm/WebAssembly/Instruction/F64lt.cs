using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64lt : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            if (a < b)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public F64lt(Parser parser) : base(parser, true)
        {
        }
    }
}