namespace GameWasm.Webassembly.Instruction
{
    internal class I32clz : Instruction
    {
        public I32clz(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.clz";
        }
    }
}