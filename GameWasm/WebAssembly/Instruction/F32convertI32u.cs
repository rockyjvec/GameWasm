namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI32u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)f.PopI32());
            return Next;
        }

        public F32convertI32u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}