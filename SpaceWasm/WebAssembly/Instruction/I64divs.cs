using System;

namespace WebAssembly.Instruction
{
    internal class I64divs : Instruction
    {
        public I64divs(Parser parser) : base(parser, true)
        {
        }
    }
}