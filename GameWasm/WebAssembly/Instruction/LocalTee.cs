using System.Linq;

namespace GameWasm.Webassembly.Instruction
{
    class LocalTee : Instruction
    {
        public int index;

        public LocalTee(Parser parser) : base(parser, true)
        {
            index = (int)parser.GetUInt32();
        }

        public override string ToString()
        {
            return "local.tee $" + index;
        }
    }
}
