namespace GameWasm.Webassembly.Instruction
{
    internal class I32or : Instruction
    {
        public I32or(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "i32.or";
        }
    }
}