using System;

namespace WebAssembly.Instruction
{
    internal class End : Instruction
    {
        public byte Type = 0;

        public override Instruction Run(Store store)
        {
            store.Stack.PopLabel();
            return this.Next;
        }

        public End(Parser parser) : base(parser, true)
        {
        }
    }
}