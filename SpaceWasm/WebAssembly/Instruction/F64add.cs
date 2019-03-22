namespace WebAssembly.Instruction
{
    internal class F64add : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopF64() + store.Stack.PopF64());

            return this.Next;
        }

        public F64add(Parser parser) : base(parser, true)
        {
        }
    }
}