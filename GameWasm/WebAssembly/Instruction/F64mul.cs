namespace GameWasm.Webassembly.Instruction
{
    internal class F64mul : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            f.Push(a * b);

            return Next;
        }

        public F64mul(Parser parser) : base(parser, true)
        {
        }
    }
}