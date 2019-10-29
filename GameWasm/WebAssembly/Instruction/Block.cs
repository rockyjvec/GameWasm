namespace GameWasm.Webassembly.Instruction
{
    class Block : Instruction
    {
        Instruction label = null;
        byte type;

        public override void End(Instruction end)
        {
            label = end;
        }

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushLabel(new Stack.Label(label, new byte[] { type }));
            return Next;
        }

        public Block(Parser parser, Function f) : base(parser, f, true)
        {
            type = parser.GetBlockType();
        }

        public override string ToString()
        {
            return "block";
        }
    }
}
