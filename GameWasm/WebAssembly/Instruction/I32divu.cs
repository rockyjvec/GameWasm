namespace GameWasm.Webassembly.Instruction
{
    internal class I32divu : Instruction
    {
        public I32divu(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.div_u";
        }
    }
}