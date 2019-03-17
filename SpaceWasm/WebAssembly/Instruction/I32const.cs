using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32const : Instruction
    {
        UInt32 value;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(this.value);
            return this.Next;
        }

        public I32const(Parser parser) : base(parser, true)
        {
            this.value = parser.GetUInt32();
        }
    }
}
