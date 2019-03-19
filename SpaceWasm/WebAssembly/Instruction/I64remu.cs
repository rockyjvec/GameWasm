namespace WebAssembly.Instruction
{
    internal class I64remu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            store.Stack.Push(a % b);

            return this.Next;
        }

        public I64remu(Parser parser) : base(parser, true)
        {
        }
    }
}