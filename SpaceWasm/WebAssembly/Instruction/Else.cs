namespace WebAssembly.Instruction
{
    internal class Else : Instruction
    {
        Instruction endLabel;

        public override Instruction Run(Store store)
        {
            return this.endLabel;
        }

        public override void End(Instruction end)
        {
            this.endLabel = end;
        }

        public Else(Parser parser) : base(parser, true)
        {
        }
    }
}