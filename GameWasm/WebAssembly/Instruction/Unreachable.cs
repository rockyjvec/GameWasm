namespace GameWasm.Webassembly.Instruction
{
    class Unreachable : Instruction
    {
        public override Instruction Run(Store store)
        {
            throw new Trap("unreachable");
        }
        public Unreachable(Parser parser) : base(parser, true)
        {
        }
    }
}
