using GameWasm.Webassembly.Stack;

namespace GameWasm.Webassembly.Instruction
{
    class Call : Instruction
    {
        int funcidx;
        public override Instruction Run(Frame f)
        {
            f.Function.Module.Store.CallFunction(f.Function.Module.Functions[funcidx]);
            return Next;
        }

        public Call(Parser parser) : base(parser, true)
        {
            funcidx = (int)parser.GetIndex();
        }
    }
}
