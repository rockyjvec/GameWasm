namespace GameWasm.Webassembly.Stack
{
    public class Label
    {
        public Instruction.Instruction Instruction;
        public byte[] Type;
        public Label(Instruction.Instruction i, byte[] type)
        {
            if (type.Length == 1 && type[0] == 64) Type = new byte[] { };
            else Type = type;
            Instruction = i;
        }
    }
}
