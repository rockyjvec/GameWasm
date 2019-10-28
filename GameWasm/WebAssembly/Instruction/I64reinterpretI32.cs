using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I64reinterpretI32 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(BitConverter.ToUInt64(BitConverter.GetBytes(f.PopF64()), 0));
            return Next;
        }

        public I64reinterpretI32(Parser parser) : base(parser, true)
        {
        }
    }
}