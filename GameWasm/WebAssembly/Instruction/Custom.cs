using System;

namespace GameWasm.Webassembly.Instruction
{
    class Custom : Instruction
    {
        Action a;

        public Custom(Action a) : base(null, true)
        {
            this.a = a;
        }
    }
}
