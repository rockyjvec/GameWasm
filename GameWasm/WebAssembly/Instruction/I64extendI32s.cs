using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64extendI32s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            f.PushI64((UInt64)(Int64)((Int32)b));

            return Next;
        }

        public I64extendI32s(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}