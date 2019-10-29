namespace GameWasm.Webassembly.Instruction
{
    internal class F32neg : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF32();
            f.Push(-a);
            return Next;
        }

        public F32neg(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}