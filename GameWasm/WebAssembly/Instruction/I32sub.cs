namespace GameWasm.Webassembly.Instruction
{
    internal class I32sub : Instruction
    {
        public I32sub(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "i32.sub";
        }
    }
}