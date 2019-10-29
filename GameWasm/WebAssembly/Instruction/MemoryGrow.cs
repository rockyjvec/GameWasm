using System;

namespace GameWasm.Webassembly.Instruction
{
    class MemoryGrow : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var size = f.PopI32();

            f.PushI32((UInt32)f.Function.Module.Memory[0].Grow(size));

            return Next;
        }

        public MemoryGrow(Parser parser, Function f) : base(parser, f, true)
        {
            UInt32 zero = parser.GetUInt32(); // May be used in future version of WebAssembly to address additional memories

            if(zero != 0x00)
            {
                Console.WriteLine("WARNING: memory.grow called with non-zero: 0x" + zero.ToString("X"));
            }
        }
    }
}
