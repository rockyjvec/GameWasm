using System;

namespace GameWasm.Webassembly.Instruction
{
    class Select : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopI32();
            var val2 = store.Stack.Pop();
            var val1 = store.Stack.Pop();
            if(val1.GetType() != val2.GetType())
            {
                throw new Exception("Select types don't match.");
            }

            if(a != 0)
            {
                store.Stack.Push(val1);
            }
            else
            {
                store.Stack.Push(val2);
            }

            return Next;
        }

        public Select(Parser parser) : base(parser, true)
        {
        }
    }
}
