using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Custom : Instruction
    {
        Action a;
        public override Instruction Run(Store store)
        {
            a();
            return this.Next;
        }

        public Custom(Action a) : base(null, true)
        {
            this.a = a;
        }
    }
}
