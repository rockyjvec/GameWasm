using System;

namespace GameWasm.Webassembly.Instruction
{
    class Custom : Instruction
    {
        Action a;
        public override Instruction Run(Store store)
        {
            a();
            return Next;
        }

        public Custom(Action a) : base(null, true)
        {
            this.a = a;
        }
    }
}
