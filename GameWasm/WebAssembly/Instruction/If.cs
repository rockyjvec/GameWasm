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

        public override Instruction Run(Store store)
        {
            var v = store.Stack.PopI32();


            if (v > 0)
            {
                if(end as Else != null)
                    store.Stack.Push(new Stack.Label((end as Else).end, new byte[] { type }));
                else
                    store.Stack.Push(new Stack.Label(end, new byte[] { type }));
                return Next;
            }
            else
            {
                if (end as Else != null)
                {
                    store.Stack.Push(new Stack.Label((end as Else).end, new byte[] { type }));
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
