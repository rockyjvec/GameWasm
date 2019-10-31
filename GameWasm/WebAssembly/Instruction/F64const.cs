namespace GameWasm.Webassembly.Instruction
{
    internal class F64const : Instruction
    {
        public double value;

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
