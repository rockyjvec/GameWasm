using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32gtu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (UInt32)store.Stack.PopI32();
            var a = (UInt32)store.Stack.PopI32();

            if (a > b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I32gtu(Parser parser) : base(parser, true)
        {
        }
    }
}
