using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Loop : Instruction
    {
        byte type;

        public override void End(Instruction end)
        {
        }

        public override Instruction Run(Store store)
        {
            store.Stack.Push(new Stack.Label(this, new byte[] { type }));
            return this.Next;
        }

        public Loop(Parser parser) : base(parser, true)
        {
            this.type = parser.GetBlockType();
        }
    }
}
