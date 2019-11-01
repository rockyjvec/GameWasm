namespace GameWasm.Webassembly.Instruction
{
    internal class I64mul : Instruction
    {
        public I64mul(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i64.mul";
        }
    }
}