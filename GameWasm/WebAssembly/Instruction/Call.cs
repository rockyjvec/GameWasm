namespace GameWasm.Webassembly.Instruction
{
    class Call : Instruction
    {
        public int funcidx;

        public Call(Parser parser) : base(parser, true)
        {
            funcidx = (int)parser.GetIndex();
        }

        public override string ToString()
        {
            return "call $f" + funcidx;
        }
    }
}
