namespace GameWasm.Webassembly.New
{
    public struct Label
    {
        public int ip;
        public int vStackPtr;

        public Label(int ip, int vStackPtr)
        {
            this.ip = ip;
            this.vStackPtr = vStackPtr;
        }
    }
}