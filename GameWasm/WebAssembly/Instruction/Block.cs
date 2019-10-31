namespace GameWasm.Webassembly.Instruction
{
    class Block : Instruction
    {
        Instruction label = null;
        public  int labelPos;
        byte type;

        public override void End(Instruction end, int pos)
        {
            label = end;
            labelPos = pos;
        }

        public Block(Parser parser, int pos) : base(parser, true)
        {
            type = parser.GetBlockType();
            labelPos = pos;
        }

        public override string ToString()
        {
            return "block";
        }
    }
}
