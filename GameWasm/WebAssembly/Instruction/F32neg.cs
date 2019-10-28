namespace GameWasm.Webassembly.Instruction
{
    internal class F32neg : Instruction
    {
        public override Instruction Run(Store store)
        {
            var a = store.Stack.PopF32();
            store.Stack.Push(-a);
            return Next;
        }

        public F32neg(Parser parser) : base(parser, true)
        {
        }
    }
}