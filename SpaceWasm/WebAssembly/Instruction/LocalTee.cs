using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class LocalTee : Instruction
    {
        public int index;

        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopValue();
            store.Stack.Push(a);
            if (index >= store.CurrentFrame.Locals.Count())
                throw new Exception("Invalid local variable");
            store.CurrentFrame.Locals[index] = a;
            return this.Next;
        }

        public LocalTee(Parser parser) : base(parser, true)
        {
            this.index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "tee_local $var" + this.index;
        }
    }
}
