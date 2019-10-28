namespace GameWasm.Webassembly.Instruction
{
    class LocalGet : Instruction
    {
        int index;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.CurrentFrame.Locals[index]);
            return Next;
        }

        public LocalGet(Parser parser) : base(parser, true)
        {
            /*
            if (index >= store.CurrentFrame.Locals.Count())
                throw new Exception("Invalid local variable");*/
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "get_local $var" + index;
        }
    }
}
