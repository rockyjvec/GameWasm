using System;

namespace GameWasm.Webassembly.Instruction
{
    class Select : Instruction
    {
        public Select(Parser parser) : base(parser, true)
        {
        }
    }
}
