using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI32s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((double)(Int32)f.PopI32());
            return Next;
        }

        public F64convertI32s(Parser parser) : base(parser, true)
        {
        }
    }
}