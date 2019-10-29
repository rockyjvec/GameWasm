namespace GameWasm.Webassembly.Instruction
{
    internal class Else : Instruction
    {
        public Instruction end;

        protected override Instruction Run(Stack.Frame f)
        {
            return end;
        }

        public override void End(Instruction end)
        {
            this.end = end;
        }

        public Else(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}