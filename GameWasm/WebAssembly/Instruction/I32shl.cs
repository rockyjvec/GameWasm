namespace GameWasm.Webassembly.Instruction
{
    internal class I32shl : Instruction
    {
        public I32shl(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.shl";
        }
    }
}