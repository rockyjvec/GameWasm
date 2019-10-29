using System;
using System.Linq;

namespace GameWasm.Webassembly
{
    public class Type
    {
        public const byte i32 = 0x7F, i64 = 0x7E, f32 = 0x7D, f64 = 0x7C, label = 0xFF;

        public byte[] Parameters;
        public byte[] Results;

        public Type(byte[] parameters, byte[] results)
        {
            Parameters = new byte[] { };
            if(parameters != null) Parameters = parameters;
            Results = new byte[] { };
            if(results != null) Results = results;
        }

        public bool SameAs(Type item)
        {
            return Parameters.SequenceEqual(item.Parameters) && Results.SequenceEqual(item.Results);
        }

        public static string Pretify(object v)
        {
            if(v is UInt32 || v is UInt64 || v is Single || v is Double)
            {
                return v.ToString();
            }
            else
            {
                return "unknown";
            }
        }

        public override string ToString()
        {
            string result = "(";

            for(int i = 0; i < Parameters.Length; i++)
            {
                switch(Parameters[i])
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

                if(i + 1 < Parameters.Length)
                {
                    result += ", ";
                }
            }

            result += ") => (";

            for (int i = 0; i < Results.Length; i++)
            {
                switch (Results[i])
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

                if (i + 1 < Results.Length)
                {
                    result += ", ";
                }
            }

            return result + ")";
        }
    }
}
