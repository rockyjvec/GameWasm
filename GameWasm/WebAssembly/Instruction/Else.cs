namespace GameWasm.Webassembly.Instruction
{
    internal class Else : Instruction
    {
        public Instruction label;

        public override void End(Instruction end)
        {
            label = end;
        }

        public Else(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "else";
        }
    }
}