using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI32() == store.Stack.PopI32())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I32eq(Parser parser) : base(parser, true)
        {
        }
    }
}
