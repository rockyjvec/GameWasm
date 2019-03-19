namespace WebAssembly.Instruction
{
    internal class I64and : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopI64() & store.Stack.PopI64());
            return this.Next;
        }

        public I64and(Parser parser) : base(parser, true)
        {
        }
    }
}