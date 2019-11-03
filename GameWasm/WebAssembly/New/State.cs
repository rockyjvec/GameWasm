namespace GameWasm.Webassembly.New
{
    public class State
    {
        public int ip;
        public int vStackPtr;
        public Function function;
        public Label[] lStack;
        public int labelPtr;
        public Inst[] program;
        public Value[] locals;
        public Memory memory;
    }
}