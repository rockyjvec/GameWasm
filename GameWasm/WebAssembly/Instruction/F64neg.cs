namespace GameWasm.Webassembly.Instruction
{
    internal class F64neg : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF64();
            f.PushF64(-a);
            return Next;
        }

        public F64neg(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}