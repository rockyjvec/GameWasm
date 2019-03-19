using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module
{
    class Env : Module
    {
        public Env(Store store) : base("env", store)
        {
            this.AddExportFunc("abort", new byte[] { Type.i32 }, new byte[] { }, this.abort);
            this.AddExportFunc("enlargeMemory", new byte[] { }, new byte[] { Type.i32 }, this.enlargeMemory);
            this.AddExportFunc("getTotalMemory", new byte[] { }, new byte[] { Type.i32 }, this.getTotalMemory);
            this.AddExportFunc("abortOnCannotGrowMemory", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("invoke_vii", new byte[] { Type.i32, Type.i32, Type.i32 });
            this.AddExportFunc("___syscall221", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___lock", new byte[] { Type.i32 });
            this.AddExportFunc("___unlock", new byte[] { Type.i32 });
            this.AddExportFunc("___syscall63", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_abort", new byte[] { }, new byte[] { }, this._abort);
            this.AddExportFunc("___syscall40", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_difftime", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.f64 });
            this.AddExportFunc("_system", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_longjmp", new byte[] { Type.i32, Type.i32 }, new byte[] { });
            this.AddExportFunc("___setErrNo", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("___syscall330", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall196", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_emscripten_memcpy_big", new byte[] { Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_mktime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_strftime", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_clock", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall91", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_gmtime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_getenv", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___map_file", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall54", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall38", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall10", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall6", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall5", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___clock_gettime", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_time", new byte[] { Type.i32 }, new byte[] { Type.i32 }, this._time);
            this.AddExportFunc("___syscall140", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_localtime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_exit", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("___syscall145", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall146", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });

            var special = 21216;//2672

            this.AddExportGlob("DYNAMICTOP_PTR", Type.i32, true, (UInt32)special);
            this.AddExportGlob("STACKTOP", Type.i32, true, (UInt32)21200);
            this.AddExportGlob("STACK_MAX", Type.i32, true, (UInt32)(1024 * 1024 * 5));
            this.AddExportGlob("memoryBase", Type.i32, true, (UInt32)0);
            this.AddExportGlob("tableBase", Type.i32, true, (UInt32)0);

            var memory = new Memory(256, 256);
            this.Memory.Add(memory);
            this.AddExportMemory("memory", memory);

            memory.SetBytes((UInt64)(special >> 2), BitConverter.GetBytes((UInt32)(1024*1024*5)));

            this.AddExportTable("table", new Table(0x70, 294, 294));
        }

        public object[] abort(object[] parameters)
        {
            throw new Exception("abort called with value: 0x" + ((UInt32)parameters[0]).ToString("X"));
        }

        public object[] _abort(object[] parameters)
        {
            throw new Exception("_abort called");
        }

        public object[] _time(object[] parameters)
        {
            return new object[] { (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds };
        }

        public object[] getTotalMemory(object[] parameters)
        {
            UInt32 result = 1024*1024*5;
            return new object[] { result };
        }

        public object[] enlargeMemory(object[] parameters)
        {
            return new object[] { Memory[0].Grow((UInt32)Memory[0].CurrentPages + 1) };
        }
    }
}
