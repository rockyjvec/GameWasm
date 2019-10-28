namespace GameWasm.Webassembly.Instruction
{
    internal class I64and : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopI64() & f.PopI64());
            return Next;
        }

        public I64and(Parser parser) : base(parser, true)
        {
        }
    }
}