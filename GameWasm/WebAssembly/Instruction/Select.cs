using System;

namespace GameWasm.Webassembly.Instruction
{
    class Select : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopI32();
            var val2 = f.Pop();
            var val1 = f.Pop();
            /*if(val1.GetType() != val2.GetType())
            {
                throw new Exception("Select types don't match.");
            }*/

            if(a != 0)
            {
                f.Push(val1);
            }
            else
            {
                f.Push(val2);
            }

            return Next;
        }

        public Select(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}
