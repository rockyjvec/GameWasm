namespace GameWasm.Webassembly.Instruction
{
    class Return : Instruction
    {
        public override Instruction Run(Store store)
        {
            return null;
        }

        public Return(Parser parser) : base(parser, true)
        {
        }
    }
}
