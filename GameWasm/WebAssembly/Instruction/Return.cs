namespace GameWasm.Webassembly.Instruction
{
    class Return : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            return null;
        }

        public Return(Parser parser) : base(parser, true)
        {
        }
    }
}
