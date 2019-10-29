namespace GameWasm.Webassembly.Instruction
{
    internal class F32demoteF64 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF32((float)f.PopF64());
            return Next;
        }

        public F32demoteF64(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}