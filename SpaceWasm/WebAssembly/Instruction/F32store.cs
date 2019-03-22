using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class F32store : Instruction
    {
        UInt32 align, offset;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopF32();
            var index = store.Stack.PopI32();
            byte[] bytes = BitConverter.GetBytes(v);
            store.CurrentFrame.Module.Memory[0].SetBytes((UInt64)offset + (UInt64)index, bytes);
            return this.Next;
        }

        public F32store(Parser parser) : base(parser, true)
        {
            this.align = (UInt32)parser.GetUInt32();
            this.offset = (UInt32)parser.GetUInt32();
        }
    }
}
