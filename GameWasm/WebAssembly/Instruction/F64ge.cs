using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64ge : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

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

        public F64ge(Parser parser) : base(parser, true)
        {
        }
    }
}