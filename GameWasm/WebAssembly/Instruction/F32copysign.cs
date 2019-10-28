namespace GameWasm.Webassembly.Instruction
{
    internal class F32copysign : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopF32();
            var a = store.Stack.PopF32();

            if (a >= 0 && b < 0)
            {
                a = -a;
            }

            if (a < 0 && b >= 0)
            {
                a = -a;
            }

            store.Stack.Push(a);
            return Next;
        }

        public F32copysign(Parser parser) : base(parser, true)
        {
        }
    }
}