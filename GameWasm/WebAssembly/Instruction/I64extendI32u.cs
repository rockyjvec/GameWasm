using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64extendI32u : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            f.Push((UInt64)b);

            return Next;
        }

        public I64extendI32u(Parser parser) : base(parser, true)
        {
        }
    }
}