namespace WebAssembly.Instruction
{
    internal class F32demoteF64 : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((float)store.Stack.PopF64());
            return this.Next;
        }

        public F32demoteF64(Parser parser) : base(parser, true)
        {
        }
    }
}