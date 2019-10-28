namespace GameWasm.Webassembly.Instruction
{
    internal class I64remu : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI64();
            var a = f.PopI64();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                f.Push(a % b);
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