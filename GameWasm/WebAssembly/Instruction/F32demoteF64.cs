namespace GameWasm.Webassembly.Instruction
{
    internal class F32demoteF64 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)store.Stack.PopF64());
            return Next;
        }

        public F32demoteF64(Parser parser) : base(parser, true)
        {
        }
    }
}