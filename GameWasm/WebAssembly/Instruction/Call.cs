using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly.Instruction
{
    class Call : Instruction
    {
        public int funcidx;
        protected override Instruction Run(Frame f)
        {
            f.Function.Module.Store.CallFunction(f.Function.Module.Functions[funcidx]);
            return Next;
        }

        public Call(Parser parser, Function f) : base(parser, f, true)
        {
            funcidx = (int)parser.GetIndex();
        }
    }
}
