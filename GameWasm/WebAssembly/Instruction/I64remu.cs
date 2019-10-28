namespace GameWasm.Webassembly.Instruction
{
    internal class I64remu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI64();
            var a = store.Stack.PopI64();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                store.Stack.Push(a % b);
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return Next;
        }

        public I64remu(Parser parser) : base(parser, true)
        {
        }
    }
}