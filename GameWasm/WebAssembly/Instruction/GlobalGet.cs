﻿using System;
using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class GlobalGet : Instruction
    {
        Global global;

        public override Instruction Run(Store store)
        {
            store.Stack.Push(global.GetValue());
            return Next;
        }

        public GlobalGet(Parser parser) : base(parser, true)
        {
            var index = (int)parser.GetUInt32();
            if (index >= parser.Module.Globals.Count())
                throw new Exception("Invalid global variable");
            global = parser.Module.Globals[index];
        }

        public override string ToString()
        {
            return "get_global " + global.Name + " (" + Type.Pretify(global.GetValue()) + ")";
        }
    }
}