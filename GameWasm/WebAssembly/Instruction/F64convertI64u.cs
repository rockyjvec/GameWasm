namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64u : Instruction
    {
        public override Instruction Run(Store store)
        {
            store.Stack.Push((double)store.Stack.PopI64());
            return Next;
        }

        public F64convertI64u(Parser parser) : base(parser, true)
        {
        }
    }
}