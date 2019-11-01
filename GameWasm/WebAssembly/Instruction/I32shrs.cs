namespace GameWasm.Webassembly.Instruction
{
    internal class I32shrs : Instruction
    {
        public I32shrs(Parser parser) : base(parser, true)
        {
        }
 
        public override string ToString()
        {
            return "i32.shr_s";
        }
    }
}