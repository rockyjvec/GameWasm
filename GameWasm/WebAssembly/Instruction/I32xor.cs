namespace GameWasm.Webassembly.Instruction
{
    internal class I32xor : Instruction
    {
        public I32xor(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.xor";
        }
    }
}