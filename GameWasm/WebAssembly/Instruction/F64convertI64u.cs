namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((double)f.PopI64());
            return Next;
        }

        public F64convertI64u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}