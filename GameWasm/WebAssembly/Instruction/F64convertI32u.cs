namespace GameWasm.Webassembly.Instruction
{
    internal class F64convertI32u : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((double)f.PopI32());
            return Next;
        }
        public F64convertI32u(Parser parser) : base(parser, true)
        {
        }
    }
}