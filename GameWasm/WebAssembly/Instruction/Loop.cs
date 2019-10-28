namespace GameWasm.Webassembly.Instruction
{
    class Loop : Instruction
    {
        byte type;

        public override void End(Instruction end)
        {
        }

        public override Instruction Run(Store store)
        {
            store.Stack.Push(new Stack.Label(this, new byte[] { type }));
            return Next;
        }

        public Loop(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }
    }
}
