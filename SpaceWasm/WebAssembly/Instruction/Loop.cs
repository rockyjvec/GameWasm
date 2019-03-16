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
            end.Next = this;
        }

        public Loop(Parser parser) : base(parser, true)
        {
            this.type = parser.GetBlockType();
        }
    }
}
