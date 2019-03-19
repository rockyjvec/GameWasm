using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Instruction
{
    class GlobalSet : Instruction
    {
        Global global;

        public override Instruction Run(Store store)
        {
            global.Set(store.Stack.Pop());

            return this.Next;
        }

        public GlobalSet(Parser parser) : base(parser, true)
        {
            var index = (int)parser.GetUInt32();
            if (index >= parser.Module.Globals.Count())
                throw new Exception("Invalid global variable");
            this.global = parser.Module.Globals[index];
        }

        public override string ToString()
        {
            return "set_global " + this.global.Name + " (" + Type.Pretify(this.global.GetValue()) + ")";
        }
    }
}
