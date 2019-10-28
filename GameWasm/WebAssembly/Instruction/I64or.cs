namespace GameWasm.Webassembly.Instruction
{
    internal class I64or : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopI64() | store.Stack.PopI64());
            return Next;
        }

        public I64or(Parser parser) : base(parser, true)
        {
        }
    }
}