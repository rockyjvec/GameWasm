namespace WebAssembly.Instruction
{
    internal class Else : Instruction
    {
        public override Instruction Run(Store store)
        {
            return this.Next;
        }

        public Else(Parser parser) : base(parser, true)
        {
        }
    }
}