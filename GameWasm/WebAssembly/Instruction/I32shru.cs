namespace GameWasm.Webassembly.Instruction
{
    internal class I32shru : Instruction
    {
        public I32shru(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.shr_u";
        }
    }
}