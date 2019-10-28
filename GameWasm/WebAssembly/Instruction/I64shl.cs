using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64shl : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI64();
            var a = (UInt64)f.PopI64();

            f.Push(a << b);
            return Next;
        }

        public I64shl(Parser parser) : base(parser, true)
        {
        }
    }
}