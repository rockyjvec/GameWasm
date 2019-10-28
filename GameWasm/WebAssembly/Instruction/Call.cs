namespace GameWasm.Webassembly.Instruction
{
    class Call : Instruction
    {
        int funcidx;
        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Module.Functions[funcidx].NativeCall();
            return Next;
        }

        public Call(Parser parser) : base(parser, true)
        {
            funcidx = (int)parser.GetIndex();
        }
    }
}
