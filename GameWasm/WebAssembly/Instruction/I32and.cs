namespace GameWasm.Webassembly.Instruction
{
    internal class I32and : Instruction
    {
        public override Instruction Run(Store store)
        {
            var b = store.Stack.PopI32();
            var a = store.Stack.PopI32();

            store.Stack.Push(a & b);
            return Next;
        }

        public I32and(Parser parser) : base(parser, true)
        {
        }
    }
}