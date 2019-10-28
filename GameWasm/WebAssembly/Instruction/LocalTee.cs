using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
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
            return Next;
        }

        public LocalTee(Parser parser) : base(parser, true)
        {
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "tee_local $var" + index;
        }
    }
}
