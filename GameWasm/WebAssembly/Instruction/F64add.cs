namespace GameWasm.Webassembly.Instruction
{
    internal class F64add : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopF64() + f.PopF64());

            return Next;
        }

        public F64add(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}