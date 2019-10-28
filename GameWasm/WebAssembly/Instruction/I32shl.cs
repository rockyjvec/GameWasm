namespace GameWasm.Webassembly.Instruction
{
    internal class I32shl : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI32();
            var a = f.PopI32();

            f.Push(a << b);
            return Next;
        }

        public I32shl(Parser parser) : base(parser, true)
        {
        }
    }
}