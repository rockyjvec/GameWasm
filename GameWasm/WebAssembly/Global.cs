using System;

namespace GameWasm.Webassembly
{
    public class Global
    {
        public byte Type;
        object value;
        bool mutable;
        public UInt32 Index;
        public string Name;


        public Global (byte type, bool mutable, object value, UInt32 index)
        {
            Type = type;
            this.mutable = mutable;
            this.value = value;
            Index = index;
            Name = "$global" + Index;
        }

        public object GetValue()
        {
            return value;
        }

        public UInt32 GetI32()
        {
            return (UInt32)value;
        }

        public UInt64 GetI64()
        {
            return (UInt64)value;
        }

        public float GetF32()
        {
            return (float)value;
        }

        public double GetF64()
        {
            return (double)value;
        }

        public void Set(object value, bool force = false)
        {
            if (!mutable && !force)
                throw new Exception("Global not mutable");

            this.value = value;
        }

        public void SetName(string name)
        {
            Name += "(" + name + ")";
        }
    }
}
