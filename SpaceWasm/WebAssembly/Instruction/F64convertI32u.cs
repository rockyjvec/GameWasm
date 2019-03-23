namespace WebAssembly.Instruction
{
    internal class F64convertI32u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)store.Stack.PopI32());
            return this.Next;
        }
        public F64convertI32u(Parser parser) : base(parser, true)
        {
        }
    }
}