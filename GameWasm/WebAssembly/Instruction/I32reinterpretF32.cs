using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32reinterpretF32 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(BitConverter.ToUInt32(BitConverter.GetBytes(store.Stack.PopF32()), 0));
            return Next;
        }

        public I32reinterpretF32(Parser parser) : base(parser, true)
        {
        }
    }
}