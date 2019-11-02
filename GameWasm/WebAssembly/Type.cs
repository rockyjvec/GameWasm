using System;
using System.Linq;

namespace GameWasm.Webassembly
{
    public class Type
    {
        public const byte i32 = 0x7F, i64 = 0x7E, f32 = 0x7D, f64 = 0x7C, localGet = 0xFF, localSet = 0xFE, globalGet = 0xFD, globalSet = 0xFC, load32 = 0xFB, store32 = 0xFA, and32 = 0xF9, add32 = 0xF8;

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

        public static string Pretify(Value v)
        {
            switch (v.type)
            {
                case Type.i32:
                    return ((Int32)v.i32).ToString();
                case Type.i64:
                    return ((Int64)v.i64).ToString();
                case Type.f32:
                    return v.f32.ToString();
                case Type.f64:
                    return v.f64.ToString();
                default:
                    return "unknown (" + v.type + ")";
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
