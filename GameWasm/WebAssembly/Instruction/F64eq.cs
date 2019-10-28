using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64eq : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            if (f.PopF64() == f.PopF64())
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public F64eq(Parser parser) : base(parser, true)
        {
        }
    }
}