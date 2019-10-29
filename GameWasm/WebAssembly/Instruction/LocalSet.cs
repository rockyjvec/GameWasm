namespace GameWasm.Webassembly.Instruction
{
    class LocalSet : Instruction
    {
        int index;

        protected override Instruction Run(Stack.Frame f)
        {
            switch (f.Locals[index].type)
            {
                case Type.i32:
                    f.Locals[index].i32 = f.PopI32();
                    break;
                case Type.i64:
                    f.Locals[index].i64 = f.PopI64();
                    break;
                case Type.f32:
                    f.Locals[index].f32 = f.PopF32();
                    break;
                case Type.f64:
                    f.Locals[index].f64 = f.PopF64();
                    break;
            }
            return Next;
        }

        public LocalSet(Parser parser, Function f) : base(parser, f, true)
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
