using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class I32store16 : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            var index = store.Stack.PopI32();
            var v = store.Stack.PopI32();
            byte[] bytes = BitConverter.GetBytes((UInt16)v);
            store.CurrentFrame.Module.Memory[0].SetBytes((UInt64)offset + (UInt64)index, bytes);
            return this.Next;
        }

        public I32store16(Parser parser) : base(parser, true)
        {
            this.align = (UInt32)parser.GetUInt32();
            this.offset = (UInt32)parser.GetUInt32();
        }
    }
}
