using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64sub : Instruction
    {
        public I64sub(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i64.sub";
        }
    }
}