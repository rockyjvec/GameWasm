using System;

namespace GameWasm.Webassembly.Instruction
{
    class MemorySize : Instruction
    {
        public MemorySize(Parser parser) : base(parser, true)
        {
            byte zero = parser.GetByte(); // May be used in future version of WebAssembly to address additional memories

            if (zero != 0x00)
            {
                Console.WriteLine("The future has come!");
            }
        }

        public override string ToString()
        {
            return "memory.size";
        }
    }
}
