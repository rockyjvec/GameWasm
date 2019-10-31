namespace GameWasm.Webassembly.Instruction
{
    internal class F64trunc : Instruction
    {
        public F64trunc(Parser parser) : base(parser, true)
        {
        }
    }
}