namespace GameWasm.Webassembly.Instruction
{
    internal class F32demoteF64 : Instruction
    {
        public F32demoteF64(Parser parser) : base(parser, true)
        {
        }
    }
}