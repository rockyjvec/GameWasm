namespace WebAssembly.Instruction
{
    internal class F64sub : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            store.Stack.Push(a - b);
            return this.Next;
        }

        public F64sub(Parser parser) : base(parser, true)
        {
        }
    }
}