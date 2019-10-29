namespace GameWasm.Webassembly.Instruction
{
    internal class F32copysign : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopF32();
            var a = f.PopF32();

            if (a >= 0 && b < 0)
            {
                a = -a;
            }

            if (a < 0 && b >= 0)
            {
                a = -a;
            }

            f.PushF32(a);
            return Next;
        }

        public F32copysign(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}