namespace GameWasm.Webassembly.Instruction
{
    class If : Instruction
    {
        Instruction end;

        byte type;

        public override void End(Instruction end)
        {
            this.end = end;
        }

        public override Instruction Run(Stack.Frame f)
        {
            var v = f.PopI32();


            if (v > 0)
            {
                if(end as Else != null)
                    f.Push(new Stack.Label((end as Else).end, new byte[] { type }));
                else
                    f.Push(new Stack.Label(end, new byte[] { type }));
                return Next;
            }
            else
            {
                if (end as Else != null)
                {
                    f.Push(new Stack.Label((end as Else).end, new byte[] { type }));
                }

                return end.Next;
            }
        }

        public If(Parser parser) : base(parser, true)
        {
            type = parser.GetBlockType();
        }
    }
}
