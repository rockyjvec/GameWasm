using System;

namespace WebAssembly.Instruction
{
    internal class I64divs : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            store.Stack.Push((UInt64)((Int64)a / (Int64)b));
            return this.Next;
        }

        public I64divs(Parser parser) : base(parser, true)
        {
        }
    }
}