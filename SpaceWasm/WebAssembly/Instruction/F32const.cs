using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32const : Instruction
    {
        float value;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(this.value);
            return this.Next;
        }

        public F32const(Parser parser) : base(parser, true)
        {
            this.value = parser.GetF32();
        }
    }
}
