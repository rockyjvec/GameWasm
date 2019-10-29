namespace GameWasm.Webassembly.Instruction
{
    internal class I32shl : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = (byte)f.PopI32();
            var a = f.PopI32();

            f.PushI32(a << b);
            return Next;
        }

        public I32shl(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}