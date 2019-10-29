using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI32s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((float)(Int32)f.PopI32());
            return Next;
        }
        public F32convertI32s(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}