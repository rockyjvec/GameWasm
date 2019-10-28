namespace GameWasm.Webassembly.Instruction
{
    internal class F64promoteF32 : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push((double)f.PopF32());
            return Next;
        }

        public F64promoteF32(Parser parser) : base(parser, true)
        {
        }
    }
}