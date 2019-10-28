namespace GameWasm.Webassembly.Instruction
{
    internal class I32xor : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            f.Push(a ^ b);
            return Next;
        }

        public I32xor(Parser parser) : base(parser, true)
        {
        }
    }
}