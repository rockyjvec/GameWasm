namespace GameWasm.Webassembly.Instruction
{
    internal class I32add : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            f.Push(a + b);

            return Next;
        }
        public I32add(Parser parser, Function f) : base(parser, f, true)
        {
            
        }
    }
}