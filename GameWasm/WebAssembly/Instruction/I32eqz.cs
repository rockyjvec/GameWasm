using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32eqz : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI32() == 0)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I32eqz(Parser parser) : base(parser, true)
        {
        }
    }
}
