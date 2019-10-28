namespace GameWasm.Webassembly.Instruction
{
    internal class I64mul : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            store.Stack.Push(a * b);

            return Next;
        }

        public I64mul(Parser parser) : base(parser, true)
        {
        }
    }
}