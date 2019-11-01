namespace GameWasm.Webassembly.Instruction
{
    internal class I32add : Instruction
    {
        public I32add(Parser parser) : base(parser, true)
        {
            
        }

        public override string ToString()
        {
            return "i32.add";
        }
    }
}