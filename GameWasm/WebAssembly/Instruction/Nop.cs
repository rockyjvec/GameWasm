namespace GameWasm.Webassembly.Instruction
{
    class Nop : Instruction
    {
        public Nop(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "nop";
        }
    }
}
