using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32rotl : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            f.PushI32((UInt32)((a << (int)b) | (a >> (32 - (int)b))));
            return Next;
        }

        public I32rotl(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}