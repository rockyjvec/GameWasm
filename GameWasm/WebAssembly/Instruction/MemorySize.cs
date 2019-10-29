using System;

namespace GameWasm.Webassembly.Instruction
{
    class MemorySize : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushI32((UInt32)f.Function.Module.Memory[0].CurrentPages);
            return Next;
        }

        public MemorySize(Parser parser, Function f) : base(parser, f, true)
        {
            byte zero = parser.GetByte(); // May be used in future version of WebAssembly to address additional memories

            if (zero != 0x00)
            {
                Console.WriteLine("The future has come!");
            }
        }
    }
}
