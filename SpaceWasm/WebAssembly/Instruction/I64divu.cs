namespace WebAssembly.Instruction
{
    internal class I64divu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            store.Stack.Push(a / b);
            return this.Next;
        }

        public I64divu(Parser parser) : base(parser, true)
        {
        }
    }
}