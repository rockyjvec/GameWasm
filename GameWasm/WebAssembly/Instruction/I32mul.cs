using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32mul : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)store.Stack.PopI32() * (UInt32)store.Stack.PopI32());

            return Next;
        }
        public I32mul(Parser parser) : base(parser, true)
        {
        }
    }
}