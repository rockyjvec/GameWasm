namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI64u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)f.PopI64());
            return Next;
        }

        public F32convertI64u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}