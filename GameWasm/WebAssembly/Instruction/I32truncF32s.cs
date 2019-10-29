using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32truncF32s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushI32((UInt32)(Int32)Math.Truncate((float)f.PopF32()));

            return Next;
        }

        public I32truncF32s(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}