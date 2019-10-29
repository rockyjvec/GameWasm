namespace GameWasm.Webassembly.Instruction
{
    internal class F64div : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF64();
            var a = f.PopF64();

            f.PushF64(a / b);
            return Next;
        }

        public F64div(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}