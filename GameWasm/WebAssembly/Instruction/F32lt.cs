using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32lt : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            if (a < b)
            {
                store.Stack.Push((UInt32)1);
            }
            else
            {
                store.Stack.Push((UInt32)0);
            }

            return Next;
        }

        public F32lt(Parser parser) : base(parser, true)
        {
        }
    }
}
