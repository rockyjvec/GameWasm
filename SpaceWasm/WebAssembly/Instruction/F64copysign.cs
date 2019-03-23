namespace WebAssembly.Instruction
{
    internal class F64copysign : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF64();
            var a = store.Stack.PopF64();

            if (a >= 0 && b < 0)
            {
                a = -a;
            }

            if (a < 0 && b >= 0)
            {
                a = -a;
            }

            store.Stack.Push(a);
            return this.Next;
        }

        public F64copysign(Parser parser) : base(parser, true)
        {
        }
    }
}