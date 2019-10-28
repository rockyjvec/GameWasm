using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32const : Instruction
    {
        public UInt32 value;

        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(value);
            return Next;
        }

        public I32const(Parser parser) : base(parser, true)
        {
            value = (UInt32)parser.GetInt32();
        }

        public override string ToString()
        {
            return "i32.const " + (Int32)value;
        }
    }
}
