namespace GameWasm.Webassembly.Instruction
{
    class Loop : Instruction
    {
        byte type;

        public Loop(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }
    }
}
