using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64extendI32s : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = f.PopI32();
            f.Push((UInt64)(Int64)((Int32)b));

            return Next;
        }

        public I64extendI32s(Parser parser) : base(parser, true)
        {
        }
    }
}