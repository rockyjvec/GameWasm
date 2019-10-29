namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI32u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.PushF64((double)f.PopI32());
            return Next;
        }
        public F64convertI32u(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}