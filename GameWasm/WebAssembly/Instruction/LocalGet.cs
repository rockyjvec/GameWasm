namespace GameWasm.Webassembly.Instruction
{
    class LocalGet : Instruction
    {
        public LocalGet(Parser parser) : base(parser, true)
        {
            /*
            if (index >= f.Locals.Count())
                throw new Exception("Invalid local variable");*/
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "local.get $" + index;
        }
    }
}
