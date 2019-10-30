namespace GameWasm.Webassembly.Instruction
{
    class If : Instruction
    {
        Instruction end;
        public int endPos = 0;

        byte type;

        public override void End(Instruction end, int pos)
        {
            this.end = end;
            this.endPos = pos;
        }

        protected override Instruction Run(Stack.Frame f)
        {
            var v = f.PopI32();
            
            if (v > 0)
            {
                if(end as Else != null)
                    f.PushLabel(new Stack.Label((end as Else).end));
                else
                    f.PushLabel(new Stack.Label(end));
                return Next;
            }
            else
            {
                if (end as Else != null)
                {
                    f.PushLabel(new Stack.Label((end as Else).end));
                }

                return end.Next;
            }
        }

        public If(Parser parser, Function f) : base(parser, f, true)
        {
            type = parser.GetBlockType();
        }
    }
}
