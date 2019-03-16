using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I64const : Instruction
    {
        UInt64 value;

        public override Instruction Run(Store store)
        {
            store.Stack.PushValue(new Stack.Value(Type.i64, false, this.value));
            return this.Next;
        }

        public I64const(Parser parser) : base(parser, true)
        {
            this.value = parser.GetUInt64();
        }
    }
}
