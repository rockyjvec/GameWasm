namespace GameWasm.Webassembly.Instruction
{
    internal class I32divu : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            var a = f.PopI32();

            if (b == 0) throw new Trap("integer divide by zero");

            try
            {
                f.Push(a / b);
            }
            catch (System.OverflowException e)
            {
                throw new Trap("integer overflow");
            }

            return Next;
        }

        public I32divu(Parser parser) : base(parser, true)
        {
        }
    }
}