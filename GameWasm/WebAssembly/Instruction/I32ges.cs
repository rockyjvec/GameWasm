using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32ges : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = (Int32)f.PopI32();
            var a = (Int32)f.PopI32();

            if (a >= b)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I32ges(Parser parser) : base(parser, true)
        {
        }
    }
}
