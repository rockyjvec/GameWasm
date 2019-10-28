namespace GameWasm.Webassembly.Instruction
{
    internal class F32demoteF64 : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((float)f.PopF64());
            return Next;
        }

        public F32demoteF64(Parser parser) : base(parser, true)
        {
        }
    }
}