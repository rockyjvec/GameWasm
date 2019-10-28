namespace GameWasm.Webassembly.Instruction
{
    internal class I64add : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.PopI64() + f.PopI64());

            return Next;
        }

        public I64add(Parser parser) : base(parser, true)
        {
        }
    }
}