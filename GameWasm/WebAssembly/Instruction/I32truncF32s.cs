using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF32s : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)(Int32)Math.Truncate((float)store.Stack.PopF32()));

            return Next;
        }

        public I32truncF32s(Parser parser) : base(parser, true)
        {
        }
    }
}