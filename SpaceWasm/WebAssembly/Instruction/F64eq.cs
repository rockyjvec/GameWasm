namespace WebAssembly.Instruction
{
    internal class F64eq : Instruction
    {
        public override Instruction Run(Store store)
        {
            if (store.Stack.PopF64() == store.Stack.PopF64())
            {
                store.Stack.Push(true);
            }
            else
            {
                store.Stack.Push(false);
            }

            return this.Next;
        }

        public F64eq(Parser parser) : base(parser, true)
        {
        }
    }
}