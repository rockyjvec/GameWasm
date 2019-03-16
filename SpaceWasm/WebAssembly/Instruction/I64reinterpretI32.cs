namespace WebAssembly.Instruction
{
    internal class I64reinterpretI32 : Instruction
    {
        public I64reinterpretI32(Parser parser) : base(parser, true)
        {
        }
    }
}