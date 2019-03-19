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
            UInt32 pointer = parser.GetPointer();
            this.value = (UInt32)parser.GetInt32();
        }

        public override string ToString()
        {
            return "i32.const " + (Int32)this.value;
        }
    }
}
