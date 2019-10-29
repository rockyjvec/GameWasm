namespace GameWasm.Webassembly.Instruction
{
    internal class F32div : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();
            var a = f.PopF32();

            f.Push(a / b);
            return Next;
        }

        public F32div(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}