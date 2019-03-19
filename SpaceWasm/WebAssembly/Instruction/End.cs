using System;

namespace WebAssembly.Instruction
{
    internal class End : Instruction
    {
        public override Instruction Run(Store store)
        {
            if(this.Next != null)
                store.CurrentFrame.Labels.Pop();

            return this.Next;
        }

        public End(Parser parser) : base(parser, true)
        {
        }
    }
}