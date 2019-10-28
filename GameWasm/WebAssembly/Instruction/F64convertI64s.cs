using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((double)(Int64)f.PopI64());
            return Next;
        }
        public F64convertI64s(Parser parser) : base(parser, true)
        {
        }
    }
}