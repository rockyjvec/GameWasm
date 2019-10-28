namespace GameWasm.Webassembly.Instruction
{
    class LocalGet : Instruction
    {
        int index;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(f.Locals[index]);
            return Next;
        }

        public LocalGet(Parser parser) : base(parser, true)
        {
            /*
            if (index >= f.Locals.Count())
                throw new Exception("Invalid local variable");*/
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "get_local $var" + index;
        }
    }
}
