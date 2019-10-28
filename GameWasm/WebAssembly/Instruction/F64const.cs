namespace GameWasm.Webassembly.Instruction
{
    internal class F64const : Instruction
    {
        double value;

        public override Instruction Run(Stack.Frame f)
        {
            f.Push(value);
            return Next;
        }

        public F64const(Parser parser) : base(parser, true)
        {
            value = parser.GetF64();
        }

        public override string ToString()
        {
            return "f64.const " + value;
        }
    }
}
