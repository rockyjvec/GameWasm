namespace WebAssembly.Instruction
{
    internal class I32divu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                store.Stack.Push(a / b);
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return this.Next;
        }

        public I32divu(Parser parser) : base(parser, true)
        {
        }
    }
}