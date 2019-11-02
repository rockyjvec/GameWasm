namespace GameWasm.Webassembly.New
{
    public class State
    {
        public int ip;
        public int vStackPtr;
        public int localBasePtr;
        public Function function;
        public int labelBasePtr;
        public int labelPtr;
    }
}