namespace GameWasm.Webassembly.Instruction
{
    class Block : Instruction
    {
        public Instruction label;

        public override void End(Instruction end)
        {
            
            label = end;
        }

        public Block(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }

        public override string ToString()
        {
            return "block";
        }
    }
}
