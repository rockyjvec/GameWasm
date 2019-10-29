namespace GameWasm.Webassembly.Stack
{
    public struct Label
    {
        public Instruction.Instruction Instruction;
        public int Stack;
        public Label(Instruction.Instruction i)
        {
            Instruction = i;
            Stack = 0;
        }
    }
}
