using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    internal class I32store : Instruction
    {
        int align, offset;

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopI32();
            byte[] bytes = BitConverter.GetBytes(v);
            store.CurrentFrame.Module.Memory[0].Buffer[offset] = bytes[0];
            store.CurrentFrame.Module.Memory[0].Buffer[offset+1] = bytes[1];
            store.CurrentFrame.Module.Memory[0].Buffer[offset+2] = bytes[2];
            store.CurrentFrame.Module.Memory[0].Buffer[offset+3] = bytes[3];
            return this.Next;
        }

        public I32store(Parser parser) : base(parser, true)
        {
            this.align = (int)parser.GetUInt32();
            this.offset = (int)parser.GetUInt32();
        }
    }
}
