namespace GameWasm.Webassembly.Instruction
{
    class F32const : Instruction
    {
        float value;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(value);
            return Next;
        }

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
