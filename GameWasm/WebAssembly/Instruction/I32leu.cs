using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32leu : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = (UInt32)f.PopI32();
            var a = (UInt32)f.PopI32();
            if (a <= b)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I32leu(Parser parser) : base(parser, true)
        {
        }
    }
}
