namespace GameWasm.Webassembly.Instruction
{
    class Nop : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            return Next;
        }

        public Nop(Parser parser) : base(parser, true)
        {
        }
    }
}
