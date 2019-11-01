namespace GameWasm.Webassembly.Instruction
{
    class I32gts : Instruction
    {
        public I32gts(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i32.gt_s";
        }
    }
}
