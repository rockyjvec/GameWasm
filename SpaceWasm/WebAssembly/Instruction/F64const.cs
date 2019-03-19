using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    internal class F64const : Instruction
    {
        double value;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(value);
            return this.Next;
        }

        public F64const(Parser parser) : base(parser, true)
        {
            this.value = parser.GetF64();
        }

        public override string ToString()
        {
            return "f64.const " + this.value;
        }
    }
}
