namespace GameWasm.Webassembly.Instruction
{
    class Block : Instruction
    {
        Instruction label = null;
        public  int labelPos;
        byte type;

        public override void End(Instruction end, int pos)
        {
            label = end;
            labelPos -= pos;
        }

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushLabel(new Stack.Label(label));
            return Next;
        }

        public Block(Parser parser, Function f, int pos) : base(parser, f, true)
        {
            type = parser.GetBlockType();
            labelPos = pos;
        }

        public override string ToString()
        {
            return "block";
        }
    }
}
