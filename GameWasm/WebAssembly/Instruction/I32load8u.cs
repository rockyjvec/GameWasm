﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32load8u : Instruction
    {
        public UInt32 align;

        public I32load8u(Parser parser) : base(parser, true)
        {
            align = (UInt32)parser.GetUInt32();
            offset = (UInt32)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "i32.load8_u " + offset;
        }
    }
}
