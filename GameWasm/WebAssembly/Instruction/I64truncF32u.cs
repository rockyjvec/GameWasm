using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64truncF32u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushI64((UInt64)Math.Truncate((float)f.PopF32()));

            return Next;
        }

        public I64truncF32u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}