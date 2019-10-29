using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class F64reinterpretI64 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64(BitConverter.ToDouble(BitConverter.GetBytes(f.PopI64()), 0));
            return Next;
        }

        public F64reinterpretI64(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}