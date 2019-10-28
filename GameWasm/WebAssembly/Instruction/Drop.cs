namespace GameWasm.Webassembly.Instruction
{
    class Drop : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.PopValue();

            return Next;
        }

        public Drop(Parser parser) : base(parser, true)
        {
            
        }
    }
}
