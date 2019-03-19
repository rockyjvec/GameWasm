using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
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
            this.Type = type;
            this.mutable = mutable;
            this.value = value;
            this.Index = index;
            this.Name = "$global" + this.Index;
        }

        public object GetValue()
        {
            return this.value;
        }

        public UInt32 GetI32()
        {
            return (UInt32)this.value;
        }

        public UInt64 GetI64()
        {
            return (UInt64)this.value;
        }

        public float GetF32()
        {
            return (float)this.value;
        }

        public double GetF64()
        {
            return (double)this.value;
        }

        public void Set(object value, bool force = false)
        {
            if (!mutable && !force)
                throw new Exception("Global not mutable");

            this.value = value;
        }

        public void SetName(string name)
        {
            this.Name += "(" + name + ")";
        }
    }
}
