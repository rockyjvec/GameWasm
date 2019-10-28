﻿using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32store : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopI32();
            var index = store.Stack.PopI32();

            store.CurrentFrame.Module.Memory[0].SetI32((UInt64)offset + (UInt64)index, v);
            
            return Next;
        }

        public I32store(Parser parser) : base(parser, true)
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
