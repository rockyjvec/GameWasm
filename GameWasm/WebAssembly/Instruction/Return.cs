namespace GameWasm.Webassembly.Instruction
{
    class Return : Instruction
    {
        public Return(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "return";
        }
    }
}
