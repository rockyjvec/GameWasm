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
            store.Stack.PushValue(new Stack.Value(Type.f64, false, this.value));
            return this.Next;
        }

        public F64const(Parser parser) : base(parser, true)
        {
            this.value = parser.GetF64();
        }
    }
}
