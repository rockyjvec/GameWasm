using System;

namespace WebAssembly.Instruction
{
    internal class I64reinterpretI32 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(BitConverter.ToUInt64(BitConverter.GetBytes(store.Stack.PopF64()), 0));
            return this.Next;
        }

        public I64reinterpretI32(Parser parser) : base(parser, true)
        {
        }
    }
}