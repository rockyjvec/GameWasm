namespace WebAssembly.Instruction
{
    internal class F32mul : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            store.Stack.Push(a * b);

            return this.Next;
        }

        public F32mul(Parser parser) : base(parser, true)
        {
        }
    }
}