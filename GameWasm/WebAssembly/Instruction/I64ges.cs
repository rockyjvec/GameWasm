using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64ges : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (Int64)store.Stack.PopI64();
            var a = (Int64)store.Stack.PopI64();
            if (a >= b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public I64ges(Parser parser) : base(parser, true)
        {
        }
    }
}
