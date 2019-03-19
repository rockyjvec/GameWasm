using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class BrIf : Instruction
    {
        int labelidx;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.Pop();

            bool result = false;
            switch (v.GetType().ToString())
            {
                case "System.UInt32":
                    result = (UInt32)v > 0;
                    break;
                case "System.UInt64":
                    result = (UInt64)v > 0;
                    break;
                case "System.float":
                    result = (float)v > 0;
                    break;
                case "System.double":
                    result = (double)v > 0;
                    break;
            }

            if (result)
            {
                Instruction i = store.CurrentFrame.Labels.Pop();
                for(int j = 0; j < labelidx - 1; j++)
                {
                    i = store.CurrentFrame.Labels.Pop();
                }
                return i;
            }
            else
            {
                Instruction i = store.CurrentFrame.Labels.Pop();
                return this.Next.Next;
            }
        }

        public BrIf(Parser parser) : base(parser, true)
        {
            this.labelidx = (int)parser.GetIndex();
        }

        public override string ToString()
        {
            return base.ToString() + "(" + this.labelidx + ")";
        }
    }
}
