using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly
{
    public class Type
    {
        public const byte i32 = 0x7F, i64 = 0x7E, f32 = 0x7D, f64 = 0x7C;

        public byte[] Parameters;
        public byte[] Results;

        public Type(byte[] parameters, byte[] results)
        {
            this.Parameters = parameters;
            this.Results = results;
        }

        public bool SameAs(Type item)
        {
            return this.Parameters.SequenceEqual(item.Parameters) && this.Results.SequenceEqual(item.Results);
        }

        public static string Pretify(object v)
        {
            switch (v.GetType().ToString())
            {
                case "System.UInt32":
                    return ((Int32)(UInt32)v).ToString();
                case "System.UInt64":
                    return ((Int64)(UInt64)v).ToString();
                case "System.Single":
                case "System.Double":
                    return (v).ToString();
                default:
                    return "unknown";
            }
        }

        public override string ToString()
        {
            string result = "(";

            for(int i = 0; i < this.Parameters.Length; i++)
            {
                switch(this.Parameters[i])
                {
                    case 0x7F:
                        result += "i32";
                        break;
                    case 0x7E:
                        result += "i64";
                        break;
                    case 0x7D:
                        result += "f32";
                        break;
                    case 0x7C:
                        result += "f64";
                        break;
                    default:
                        result += "??";
                        break;
                }

                if(i + 1 < this.Parameters.Length)
                {
                    result += ", ";
                }
            }

            result += ") => (";

            for (int i = 0; i < this.Results.Length; i++)
            {
                switch (this.Results[i])
                {
                    case 0x7F:
                        result += "i32";
                        break;
                    case 0x7E:
                        result += "i64";
                        break;
                    case 0x7D:
                        result += "f32";
                        break;
                    case 0x7C:
                        result += "f64";
                        break;
                    default:
                        result += "??";
                        break;
                }

                if (i + 1 < this.Results.Length)
                {
                    result += ", ";
                }
            }

            return result + ")";
        }
    }
}
