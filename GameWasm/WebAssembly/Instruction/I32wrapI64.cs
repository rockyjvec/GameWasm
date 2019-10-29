using System;


namespace GameWasm.Webassembly.Instruction
{
    internal class I32wrapI64 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push((UInt32)f.PopI64());
            return Next;
        }

        public I32wrapI64(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}