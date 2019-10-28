using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32rotr : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            f.Push((UInt32)((a >> (int)b) | (a << (32 - (int)b))));
            return Next;
        }

        public I32rotr(Parser parser) : base(parser, true)
        {
        }
    }
}