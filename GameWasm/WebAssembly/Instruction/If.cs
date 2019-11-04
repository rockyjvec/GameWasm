using System;

namespace GameWasm.Webassembly.Instruction
{
    class If : Instruction
    {
        public Instruction label;

        public override void End(Instruction end)
        {
            label = end;
        }

        public If(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }
        
        public override string ToString()
        {
            return "if";
        }
    }
}
