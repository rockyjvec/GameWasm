using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopF32() == store.Stack.PopF32())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public F32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
