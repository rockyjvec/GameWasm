namespace GameWasm.Webassembly.Instruction
{
    class Block : Instruction
    {
        public Instruction label;
        byte type;

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
