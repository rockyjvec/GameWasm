namespace GameWasm.Webassembly.Instruction
{
    internal class End : Instruction
    {
        public byte Type = 0;

        public End(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "end";
        }
    }
}