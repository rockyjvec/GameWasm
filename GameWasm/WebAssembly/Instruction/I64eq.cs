using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI64() == store.Stack.PopI64())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I64eq(Parser parser) : base(parser, true)
        {
        }
    }
}
