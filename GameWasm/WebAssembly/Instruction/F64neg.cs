namespace GameWasm.Webassembly.Instruction
{
    internal class F64neg : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var a = f.PopF64();
            f.Push(-a);
            return Next;
        }

        public F64neg(Parser parser) : base(parser, true)
        {
        }
    }
}