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
            store.Stack.Push(this.value);
            return this.Next;
        }

        public I64const(Parser parser) : base(parser, true)
        {
            this.value = (UInt64)parser.GetInt64();
        }

        public override string ToString()
        {
            return "i64.const " + (Int64)this.value;
        }
    }
}
