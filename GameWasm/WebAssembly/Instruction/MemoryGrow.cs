using System;

namespace GameWasm.Webassembly.Instruction
{
    class MemoryGrow : Instruction
    {
        public override Instruction Run(Store store)
        {
            var size = store.Stack.PopI32();

            store.Stack.Push((UInt32)store.CurrentFrame.Module.Memory[0].Grow(size));

            return Next;
        }

        public MemoryGrow(Parser parser) : base(parser, true)
        {
            UInt32 zero = parser.GetUInt32(); // May be used in future version of WebAssembly to address additional memories

            if(zero != 0x00)
            {
                Console.WriteLine("WARNING: memory.grow called with non-zero: 0x" + zero.ToString("X"));
            }
        }
    }
}
