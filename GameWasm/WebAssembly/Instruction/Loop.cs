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
            f.PushLabel(new Stack.Label(this ));
            return Next;
        }

        public Loop(Parser parser, Function f) : base(parser, f, true)
        {
            type = parser.GetBlockType();
        }
    }
}
