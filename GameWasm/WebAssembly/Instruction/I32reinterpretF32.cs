using System;

namespace GameWasm.Webassembly.Instruction
{
    internal class I32reinterpretF32 : Instruction
    {
        protected override Instruction Run(Stack.Frame f)
        {
            f.Push(BitConverter.ToUInt32(BitConverter.GetBytes(f.PopF32()), 0));
            return Next;
        }

        public I32reinterpretF32(Parser parser, Function f) : base(parser, f, true)
        {
        }
    }
}