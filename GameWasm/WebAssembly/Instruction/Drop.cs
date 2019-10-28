namespace GameWasm.Webassembly.Instruction
{
    class Drop : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PopValue();

            return Next;
        }

        public Drop(Parser parser) : base(parser, true)
        {
            
        }
    }
}
