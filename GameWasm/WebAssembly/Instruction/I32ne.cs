namespace GameWasm.Webassembly.Instruction
{
    class I32ne : Instruction
    {
        public I32ne(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "i32.ne";
        }
    }
}
