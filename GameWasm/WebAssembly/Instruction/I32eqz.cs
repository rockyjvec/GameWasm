namespace GameWasm.Webassembly.Instruction
{
    class I32eqz : Instruction
    {
        public I32eqz(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.eqz";
        }
    }
}
