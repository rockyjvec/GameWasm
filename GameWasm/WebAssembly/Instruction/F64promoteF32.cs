namespace GameWasm.Webassembly.Instruction
{
    internal class F64promoteF32 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64((double)f.PopF32());
            return Next;
        }

        public F64promoteF32(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}