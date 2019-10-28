using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32shru : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI32();
            var a = (UInt32)f.PopI32();

            f.Push(a >> b);
            return Next;
        }

        public I32shru(Parser parser) : base(parser, true)
        {
        }
    }
}