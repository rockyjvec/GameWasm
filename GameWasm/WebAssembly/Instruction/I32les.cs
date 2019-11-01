namespace GameWasm.Webassembly.Instruction
{
    class I32les : Instruction
    {
        public I32les(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "i32.le_s";
        }
    }
}
