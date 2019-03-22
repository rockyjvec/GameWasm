namespace WebAssembly.Instruction
{
    internal class F64mul : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            store.Stack.Push(a * b);

            return this.Next;
        }

        public F64mul(Parser parser) : base(parser, true)
        {
        }
    }
}