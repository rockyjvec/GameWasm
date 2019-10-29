namespace GameWasm.Webassembly.Instruction
{
    internal class F64const : Instruction
    {
        public double value;

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64(value);
            return Next;
        }

        public F64const(Parser parser, Function f) : base(parser, f, true)
        {
            value = parser.GetF64();
        }

        public override string ToString()
        {
            return "f64.const " + value;
        }
    }
}
