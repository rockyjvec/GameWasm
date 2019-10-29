namespace GameWasm.Webassembly.Instruction
{
    internal class I64or : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopI64() | f.PopI64());
            return Next;
        }

        public I64or(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}