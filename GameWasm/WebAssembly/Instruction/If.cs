namespace GameWasm.Webassembly.Instruction
{
    class If : Instruction
    {
        Instruction end;
        public int endPos = 0;

        byte type;

        public override void End(Instruction end, int pos)
        {
            this.end = end;
            this.endPos = pos;
        }

        public If(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }
    }
}
