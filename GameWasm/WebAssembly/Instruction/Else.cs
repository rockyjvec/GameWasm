namespace GameWasm.Webassembly.Instruction
{
    internal class Else : Instruction
    {
        public Instruction end;

        public override Instruction Run(Stack.Frame f)
        {
            return end;
        }

        public override void End(Instruction end)
        {
            this.end = end;
        }

        public Else(Parser parser) : base(parser, true)
        {
        }
    }
}