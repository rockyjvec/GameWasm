using System;

namespace WebAssembly.Instruction
{
    internal class F32reinterpretI32 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(BitConverter.ToSingle(BitConverter.GetBytes(store.Stack.PopI32()), 0));
            return this.Next;
        }

        public F32reinterpretI32(Parser parser) : base(parser, true)
        {
        }
    }
}