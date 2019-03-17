using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class Call : Instruction
    {
        int funcidx;
        public override Instruction Run(Store store)
        {
            store.CurrentFrame.Module.Functions[funcidx].NativeCall();
            return this.Next;
        }

        public Call(Parser parser) : base(parser, true)
        {
            this.funcidx = (int)parser.GetIndex();
        }
    }
}
