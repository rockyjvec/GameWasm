using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class If : Instruction
    {
        Instruction end;

        byte type;

        public override void End(Instruction end)
        {
            this.end = end;
        }

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopI32();


            if (v > 0)
            {
                if(this.end as Else != null)
                    store.Stack.Push(new Stack.Label((this.end as Else).end, new byte[] { this.type }));
                else
                    store.Stack.Push(new Stack.Label(this.end, new byte[] { this.type }));
                return this.Next;
            }
            else
            {
                if (this.end as Else != null)
                {
                    store.Stack.Push(new Stack.Label((this.end as Else).end, new byte[] { this.type }));
                }

                return this.end.Next;
            }
        }

        public If(Parser parser) : base(parser, true)
        {
            this.type = parser.GetBlockType();
        }
    }
}
