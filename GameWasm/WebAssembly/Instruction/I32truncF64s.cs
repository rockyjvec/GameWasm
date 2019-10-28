using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF64s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)(Int32)Math.Truncate((double)store.Stack.PopF64()));

            return Next;
        }

        public I32truncF64s(Parser parser) : base(parser, true)
        {
        }
    }
}