namespace GameWasm.Webassembly.Instruction
{
    class I64eqz : Instruction
    {
        public I64eqz(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i64.eqz";
        }
    }
}
