namespace GameWasm.Webassembly.Instruction
{
    internal class Else : Instruction
    {
        public Instruction end;
        public int endPos = 0;

        public override void End(Instruction end, int pos = 0)
        {
            this.end = end;
            this.endPos = pos;
        }

        public Else(Parser parser) : base(parser, true)
        {
        }
    }
}