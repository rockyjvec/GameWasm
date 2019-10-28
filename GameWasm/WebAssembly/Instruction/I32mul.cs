using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32mul : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt32)f.PopI32() * (UInt32)f.PopI32());

            return Next;
        }
        public I32mul(Parser parser) : base(parser, true)
        {
        }
    }
}