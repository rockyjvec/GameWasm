using System;

namespace GameWasm.Webassembly
{
    public class Global
    {
        public byte Type;
        Value value;
        bool mutable;
        public UInt32 Index;
        public string Name;


        public Global (byte type, bool mutable, Value value, UInt32 index)
        {
            Type = type;
            this.mutable = mutable;
            this.value = value;
            Index = index;
            Name = "$global" + Index;
        }

        public Value GetValue()
        {
            return value;
        }

        public void Set(Value value, bool force = false)
        {
            if (!mutable && !force)
                throw new Exception("Global not mutable");

            if(value.type != this.value.type) throw new Trap("indirect call type mismatch");
            
            this.value = value;
        }

        public void SetName(string name)
        {
            Name += "(" + name + ")";
        }
    }
}
