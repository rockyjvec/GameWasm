using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalGet : Instruction
    {
        int index;

        public override Instruction Run(Store store)
        {
            if (index >= store.CurrentFrame.Locals.Count())
                throw new Exception("Invalid local variable");
            store.Stack.Push(store.CurrentFrame.Locals[index]);
            return this.Next;
        }

        public LocalGet(Parser parser) : base(parser, true)
        {
            this.index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "get_local $var" + this.index;
        }
    }
}
