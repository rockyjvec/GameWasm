namespace GameWasm.Webassembly.Instruction
{
    internal class F32add : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopF32() + f.PopF32());

            return Next;
        }

        public F32add(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}