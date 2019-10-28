using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64max : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            store.Stack.Push((double)Math.Max(a, b));
            return Next;
        }

        public F64max(Parser parser) : base(parser, true)
        {
        }
    }
}