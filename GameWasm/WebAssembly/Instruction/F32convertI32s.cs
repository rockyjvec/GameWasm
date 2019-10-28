using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI32s : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)(Int32)f.PopI32());
            return Next;
        }
        public F32convertI32s(Parser parser) : base(parser, true)
        {
        }
    }
}