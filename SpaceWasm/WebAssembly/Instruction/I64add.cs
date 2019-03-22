namespace WebAssembly.Instruction
{
    internal class I64add : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopI64() + store.Stack.PopI64());

            return this.Next;
        }

        public I64add(Parser parser) : base(parser, true)
        {
        }
    }
}