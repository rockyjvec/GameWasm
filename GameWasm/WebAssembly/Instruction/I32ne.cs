using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32ne : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopI32() != store.Stack.PopI32())
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I32ne(Parser parser) : base(parser, true)
        {
        }
    }
}
