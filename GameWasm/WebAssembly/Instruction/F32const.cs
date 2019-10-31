namespace GameWasm.Webassembly.Instruction
{
    class F32const : Instruction
    {
        public float value;

        public F32const(Parser parser) : base(parser, true)
        {
            value = parser.GetF32();
        }

        public override string ToString()
        {
            return "f32.const " + value;
        }
    }
}
