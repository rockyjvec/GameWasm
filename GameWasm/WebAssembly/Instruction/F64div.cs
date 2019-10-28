namespace GameWasm.Webassembly.Instruction
{
    internal class F64div : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            store.Stack.Push(a / b);
            return Next;
        }

        public F64div(Parser parser) : base(parser, true)
        {
        }
    }
}