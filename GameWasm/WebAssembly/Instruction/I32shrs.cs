using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32shrs : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI32();
            var a = (Int32)f.PopI32();

            f.Push((UInt32)(a >> b));
            return Next;
        }

        public I32shrs(Parser parser) : base(parser, true)
        {
        }
    }
}