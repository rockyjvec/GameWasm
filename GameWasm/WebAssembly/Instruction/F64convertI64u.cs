namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI64u : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((double)f.PopI64());
            return Next;
        }

        public F64convertI64u(Parser parser) : base(parser, true)
        {
        }
    }
}