namespace WebAssembly.Instruction
{
    internal class End : Instruction
    {
        public override Instruction Run(Store store)
        {
            return this.Next;
        }

        public End(Parser parser) : base(parser, true)
        {
        }
    }
}