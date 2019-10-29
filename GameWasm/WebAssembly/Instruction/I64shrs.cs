using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64shrs : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI64();
            var a = (Int64)f.PopI64();

            f.Push((UInt64)(a >> b));
            return Next;
        }

        public I64shrs(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}