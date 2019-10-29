namespace GameWasm.Webassembly.Instruction
{
    internal class End : Instruction
    {
        public byte Type = 0;

        protected override Instruction Run(Stack.Frame f)
        {
            f.PopLabel(1, true);
            return Next;
        }

        public End(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}