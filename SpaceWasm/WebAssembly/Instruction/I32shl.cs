namespace WebAssembly.Instruction
{
    internal class I32shl : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = (byte)store.Stack.PopI32();
            var a = store.Stack.PopI32();

            store.Stack.Push(a << b);
            return this.Next;
        }

        public I32shl(Parser parser) : base(parser, true)
        {
        }
    }
}