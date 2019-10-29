using System;

namespace GameWasm.Webassembly.Instruction
{
    class F32le : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();
            var a = f.PopF32();

            if (a <= b)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public F32le(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
