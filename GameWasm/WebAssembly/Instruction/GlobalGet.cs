using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class GlobalGet : Instruction
    {
        Global global;

        public GlobalGet(Parser parser) : base(parser, true)
        {
            index = (int)parser.GetUInt32();
            if (index >= parser.Module.Globals.Count())
                throw new Exception("Invalid global variable");
            global = parser.Module.Globals[index];
        }

        public override string ToString()
        {
            return "global.get " + global.Name + " (" + Type.Pretify(global.GetValue()) + ")";
        }
    }
}