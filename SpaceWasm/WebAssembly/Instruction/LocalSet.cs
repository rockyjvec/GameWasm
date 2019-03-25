using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalSet : Instruction
    {
        int index;

        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Locals[index] = store.Stack.Pop();
            return this.Next;
        }

        public LocalSet(Parser parser) : base(parser, true)
        {
            /*if (index >= store.CurrentFrame.Locals.Count())
                throw new Exception("Invalid local variable");*/
            this.index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "set_local $var" + this.index;
        }
    }
}
