﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    class I32load16u : Instruction
    {
        public UInt32 align;

        public I32load16u(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return base.ToString() + "(offset = " + offset + ")";
        }
    }
}
