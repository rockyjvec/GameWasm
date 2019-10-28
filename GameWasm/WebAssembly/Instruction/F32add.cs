namespace GameWasm.Webassembly.Instruction
{
    internal class F32add : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.Stack.PopF32() + store.Stack.PopF32());

            return Next;
        }

        public F32add(Parser parser) : base(parser, true)
        {
        }
    }
}