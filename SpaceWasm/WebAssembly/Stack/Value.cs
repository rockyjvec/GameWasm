using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Stack
{
    public class Value
    {
        public byte Type;
        bool mutable = true;
        bool trap = false;
        public object Val;

        public Value(byte type, bool mutable, object value = null)
        {
            this.mutable = mutable;
            this.Type = type;
            this.Val = value;

            if(this.Val == null)
            {
                switch (this.Type)
                {
                    case WebAssembly.Type.i32:
                        this.Val = new UInt32();
                        break;
                    case WebAssembly.Type.i64:
                        this.Val = new UInt64();
                        break;
                    case WebAssembly.Type.f32:
                        this.Val = new float();
                        break;
                    case WebAssembly.Type.f64:
                        this.Val = new double();
                        break;
                }
            }
        }

        public void Set(Value value)
        {
            if (!this.CompatibleWith(value))
            {
                throw new Exception("Cannot set an incompatible value");
            }
            else if (this.mutable)
            {
                this.Val = value.Val;

                if (value.Val == null)
                    throw new Exception("Error trying to set value to null");
            }
            else
            {
                throw new Exception("Cannot set a const value");
            }
        }

        public bool CompatibleWith(Value v)
        {
            return v.Type == this.Type;
        }

        public UInt32 GetI32()
        {
            if (this.Type != WebAssembly.Type.i32)
                throw new Exception("Trying to get invalid type i32: 0x" + this.Type.ToString("X") + " from value.");

            return Convert.ToUInt32(this.Val);
        }

        public UInt64 GetI64()
        {
            if (this.Type != WebAssembly.Type.i64)
                throw new Exception("Trying to get invalid type i64: 0x" + this.Type.ToString("X") + " from value.");

            return Convert.ToUInt64(this.Val);
        }

        public float GetF32()
        {
            if (this.Type != WebAssembly.Type.f32)
                throw new Exception("Trying to get invalid type f32: 0x" + this.Type.ToString("X") + " from value.");

            return Convert.ToSingle(this.Val);
        }

        public double GetF64()
        {
            if (this.Type != WebAssembly.Type.f64)
                throw new Exception("Trying to get invalid type f64: 0x" + this.Type.ToString("X")  + " from value.");

            return Convert.ToDouble(this.Val);
        }
    }
}
