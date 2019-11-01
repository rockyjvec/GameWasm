namespace GameWasm.Webassembly.Instruction
{
    internal class I32eq : Instruction
    {
        public I32eq(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.eq";
        }
    }
}
