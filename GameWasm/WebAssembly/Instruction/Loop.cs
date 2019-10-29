namespace GameWasm.Webassembly.Instruction
{
    class Loop : Instruction
    {
        byte type;

        public override void End(Instruction end)
        {
        }

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(new Stack.Label(this, new byte[] { type }));
            return Next;
        }

        public Loop(Parser parser, Function f) : base(parser, f, true)
        {
            type = parser.GetBlockType();
        }
    }
}
