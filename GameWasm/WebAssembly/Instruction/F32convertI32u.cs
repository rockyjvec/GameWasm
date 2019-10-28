namespace GameWasm.Webassembly.Instruction
{
    internal class F32convertI32u : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)f.PopI32());
            return Next;
        }

        public F32convertI32u(Parser parser) : base(parser, true)
        {
        }
    }
}