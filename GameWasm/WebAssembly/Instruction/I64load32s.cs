﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64load32s : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(store.CurrentFrame.Module.Memory[0].GetI6432s((UInt64)offset + (UInt64)store.Stack.PopI32()));
            return Next;
        }

        public I64load32s(Parser parser) : base(parser, true)
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