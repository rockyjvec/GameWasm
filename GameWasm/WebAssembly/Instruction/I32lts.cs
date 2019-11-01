namespace GameWasm.Webassembly.Instruction
{
    class I32lts : Instruction
    {
        public I32lts(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "i32.lt_s";
        }
    }
}
