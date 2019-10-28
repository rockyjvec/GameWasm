namespace GameWasm.Webassembly.Stack
{
    public class Frame
    {
        public Store Store;
        public Module.Module Module;
        public Function Function;

        public Instruction.Instruction Instruction;
        public object[] Locals;

        public object[] Results;

        public Frame(Store store, Module.Module module, Function function, Instruction.Instruction instruction)
        {
            Store = store;
            Module = module;
            Function = function;
            Instruction = instruction;
            Locals = new object[] { };
            Results = new object[] { };
        }
    }
}
