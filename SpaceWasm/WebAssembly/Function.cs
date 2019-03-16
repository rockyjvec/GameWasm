using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    class Function
    {
        bool native = false;
        public UInt32 Index = 0;
        Func<object[], object[]> action;
        public Type Type;

        Instruction.Instruction instruction;

        List<byte> localTypes = new List<byte>();

        public Function(UInt32 index, Type type)
        {
            this.Index = index;
            this.Type = type;
        }

        public Function(Func<object[], object[]> action, Type type)
        {
            this.native = true;
            this.action = action;
            this.Type = type;
        }

        public void Execute()
        {
            if (native)
            {

            }
            else
            {

            }

            throw new Exception("Function.Execute is not implemented");            
        }

        public void AddLocal(byte type)
        {
            this.localTypes.Add(type);
        }

        public void SetInstruction(Instruction.Instruction instruction)
        {
            this.instruction = instruction;
        }

        public Instruction.Instruction GetInstruction()
        {
            return this.instruction;
        }
    }
}
