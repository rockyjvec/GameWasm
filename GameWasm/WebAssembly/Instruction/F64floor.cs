using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64floor : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF64();

            store.Stack.Push((double)Math.Floor(a));
            return Next;
        }

        public F64floor(Parser parser) : base(parser, true)
        {
        }
    }
}