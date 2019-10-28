namespace GameWasm.Webassembly.Instruction
{
    internal class End : Instruction
    {
        public byte Type = 0;

        public override Instruction Run(Store store)
        {
            store.Stack.PopLabel(1, true);
            return Next;
        }

        public End(Parser parser) : base(parser, true)
        {
        }
    }
}