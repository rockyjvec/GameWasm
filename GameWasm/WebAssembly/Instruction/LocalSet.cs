namespace GameWasm.Webassembly.Instruction
{
    class LocalSet : Instruction
    {
        int index;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Locals[index] = f.Pop();
            return Next;
        }

        public LocalSet(Parser parser) : base(parser, true)
        {
            /*if (index >= f.Locals.Count())
                throw new Exception("Invalid local variable");*/
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "set_local $var" + index;
        }
    }
}
