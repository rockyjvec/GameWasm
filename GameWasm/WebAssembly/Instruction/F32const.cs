namespace GameWasm.Webassembly.Instruction
{
    class F32const : Instruction
    {
        public float value;

        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32(value);
            return Next;
        }

        public F32const(Parser parser, Function f) : base(parser, f, true)
        {
            value = parser.GetF32();
        }

        public override string ToString()
        {
            return "f32.const " + value;
        }
    }
}
