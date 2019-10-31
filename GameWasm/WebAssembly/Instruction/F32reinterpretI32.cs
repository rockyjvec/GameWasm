namespace GameWasm.Webassembly.Instruction
{
    internal class F32reinterpretI32 : Instruction
    {
        public F32reinterpretI32(Parser parser) : base(parser, true)
        {
        }
    }
}