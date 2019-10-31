using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64const : Instruction
    {
        public UInt64 value;

        public I64const(Parser parser) : base(parser, true)
        {
            value = (UInt64)parser.GetInt64();
        }

        public override string ToString()
        {
            return "i64.const " + (Int64)value;
        }
    }
}
