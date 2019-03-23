namespace WebAssembly.Instruction
{
    internal class F32convertI64u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)store.Stack.PopI64());
            return this.Next;
        }

        public F32convertI64u(Parser parser) : base(parser, true)
        {
        }
    }
}