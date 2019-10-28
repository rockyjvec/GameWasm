using System;

namespace GameWasm.Webassembly.Instruction
{
    class MemorySize : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((UInt32)store.CurrentFrame.Module.Memory[0].CurrentPages);
            return Next;
        }

        public MemorySize(Parser parser) : base(parser, true)
        {
            byte zero = parser.GetByte(); // May be used in future version of WebAssembly to address additional memories

            if (zero != 0x00)
            {
                Console.WriteLine("The future has come!");
            }
        }
    }
}
