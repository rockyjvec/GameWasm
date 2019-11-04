namespace GameWasm.Webassembly.Instruction
{
    class Loop : Instruction
    {
        public Loop(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }

        public override string ToString()
        {
            return "loop";
        }
    }
}
