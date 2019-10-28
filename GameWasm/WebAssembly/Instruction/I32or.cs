namespace GameWasm.Webassembly.Instruction
{
    internal class I32or : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopI32() | f.PopI32());
            return Next;
        }

        public I32or(Parser parser) : base(parser, true)
        {
        }
    }
}