using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class If : Instruction
    {
        Instruction elseLabel;
        Instruction endLabel;

        byte type;

        public override void End(Instruction end)
        {
            if (end as Else == null)
            {
                this.endLabel = end;
            }
            else
            {
                this.elseLabel = end;
            }
        }

        public If(Parser parser) : base(parser, true)
        {
            this.type = parser.GetBlockType();
        }
    }
}
