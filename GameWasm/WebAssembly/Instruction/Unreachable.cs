namespace GameWasm.Webassembly.Instruction
{
    class Unreachable : Instruction
    {
        public Unreachable(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "unreachable";
        }
    }
}
