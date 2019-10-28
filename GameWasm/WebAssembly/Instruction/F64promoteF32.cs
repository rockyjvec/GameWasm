namespace GameWasm.Webassembly.Instruction
{
    internal class F64promoteF32 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)store.Stack.PopF32());
            return Next;
        }

        public F64promoteF32(Parser parser) : base(parser, true)
        {
        }
    }
}