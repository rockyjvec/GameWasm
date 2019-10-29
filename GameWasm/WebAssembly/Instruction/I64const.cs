using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64const : Instruction
    {
        public UInt64 value;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(value);
            return Next;
        }

        public I64const(Parser parser, Function f) : base(parser, f, true)
        {
            value = (UInt64)parser.GetInt64();
        }

        public override string ToString()
        {
            return "i64.const " + (Int64)value;
        }
    }
}
