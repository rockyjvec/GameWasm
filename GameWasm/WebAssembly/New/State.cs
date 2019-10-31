namespace GameWasm.Webassembly.New
{
    public struct State
    {
        public int ip;
        public int vStackPtr;
        public int localBasePtr;
        public int localLength;
        public int functionPtr;
        public int labelPtr;
    }
}