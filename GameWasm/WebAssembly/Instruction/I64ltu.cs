using System;

namespace GameWasm.Webassembly.Instruction
{
    class I64ltu : Instruction
    {
        public override Instruction Run(Stack.Frame f)
        {
            var b = (UInt64)f.PopI64();
            var a = (UInt64)f.PopI64();

            if (a < b)
            {
                f.Push((UInt32)1);
            }
            else
            {
                f.Push((UInt32)0);
            }

            return Next;
        }

        public I64ltu(Parser parser) : base(parser, true)
        {
        }
    }
}
