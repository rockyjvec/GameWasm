namespace GameWasm.Webassembly.Instruction
{
    class Unreachable : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            throw new Trap("unreachable");
        }
        public Unreachable(Parser parser) : base(parser, true)
        {
        }
    }
}
