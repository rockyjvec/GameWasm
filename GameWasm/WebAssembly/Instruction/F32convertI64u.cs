namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI64u : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)f.PopI64());
            return Next;
        }

        public F32convertI64u(Parser parser) : base(parser, true)
        {
        }
    }
}