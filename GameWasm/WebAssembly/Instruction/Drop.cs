namespace GameWasm.Webassembly.Instruction
{
    class Drop : Instruction
    {
        public Drop(Parser parser) : base(parser, true)
        {
            
        }
        
        public override string ToString()
        {
            return "drop";
        }
    }
}
