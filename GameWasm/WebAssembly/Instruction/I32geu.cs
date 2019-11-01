namespace GameWasm.Webassembly.Instruction
{
    class I32geu : Instruction
    {
        public I32geu(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.ge_u";
        }
    }
}
