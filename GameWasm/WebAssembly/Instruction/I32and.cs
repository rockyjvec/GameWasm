namespace GameWasm.Webassembly.Instruction
{
    internal class I32and : Instruction
    {
        public I32and(Parser parser) : base(parser, true)
        {
        }
    
        public override string ToString()
        {
            return "i32.and";
        }
    }
}