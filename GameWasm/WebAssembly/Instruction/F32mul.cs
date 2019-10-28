namespace GameWasm.Webassembly.Instruction
{
    internal class F32mul : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            store.Stack.Push(a * b);

            return Next;
        }

        public F32mul(Parser parser) : base(parser, true)
        {
        }
    }
}