namespace GameWasm.Webassembly.Instruction
{
    internal class F32neg : Instruction
    {
        public F32neg(Parser parser) : base(parser, true)
        {
        }
        
        public override string ToString()
        {
            return "f32.neg";
        }
    }
}