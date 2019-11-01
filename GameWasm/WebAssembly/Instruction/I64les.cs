﻿namespace GameWasm.Webassembly.Instruction
{
    class I64les : Instruction
    {
        public I64les(Parser parser) : base(parser, true)
        {
        }

        public override string ToString()
        {
            return "i64.le_s";
        }
    }
}
