using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class GlobalSet : Instruction
    {
        Global global;

        public override Instruction Run(Store store)
        {
            global.Set(store.Stack.Pop());

            return Next;
        }

        public GlobalSet(Parser parser) : base(parser, true)
        {
            var index = (int)parser.GetUInt32();
            if (index >= parser.Module.Globals.Count())
                throw new Exception("Invalid global variable");
            global = parser.Module.Globals[index];
        }

        public override string ToString()
        {
            return "set_global " + global.Name + " (" + Type.Pretify(global.GetValue()) + ")";
        }
    }
}
