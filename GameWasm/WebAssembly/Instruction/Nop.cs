namespace GameWasm.Webassembly.Instruction
{
    class Nop : Instruction
    {
        public override Instruction Run(Store store)
        {
            return Next;
        }

        public Nop(Parser parser) : base(parser, true)
        {
        }
    }
}
