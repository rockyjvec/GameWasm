namespace GameWasm.Webassembly.Instruction
{
    internal class I64add : Instruction
    {
        public I64add(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i64.add";
        }
    }
}