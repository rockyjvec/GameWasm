using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI64s : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)(Int64)f.PopI64());
            return Next;
        }

        public F32convertI64s(Parser parser) : base(parser, true)
        {
        }
    }
}