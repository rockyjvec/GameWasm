namespace GameWasm.Webassembly.Instruction
{
    internal class I32remu : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

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

        public I32remu(Parser parser) : base(parser, true)
        {
        }
    }
}