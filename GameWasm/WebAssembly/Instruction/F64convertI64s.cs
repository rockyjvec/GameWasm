using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64s : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64((double)(Int64)f.PopI64());
            return Next;
        }
        public F64convertI64s(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}