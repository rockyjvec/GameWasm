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
            this.AddExportFunc("abort", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("enlargeMemory", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("getTotalMemory", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("abortOnCannotGrowMemory", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("invoke_vii", new byte[] { Type.i32, Type.i32, Type.i32 });
            this.AddExportFunc("___syscall221", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___lock", new byte[] { Type.i32 });
            this.AddExportFunc("___unlock", new byte[] { Type.i32 });
            this.AddExportFunc("___syscall63", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_abort", new byte[] { }, new byte[] { });
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
            this.AddExportFunc("_time", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall140", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_localtime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_exit", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("___syscall145", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___syscall146", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });

            this.AddExportGlob("DYNAMICTOP_PTR", new Value(Type.i32, false, 0));
            this.AddExportGlob("STACKTOP", new Value(Type.i32, false, 0));
            this.AddExportGlob("STACK_MAX", new Value(Type.i32, false, 255));
            this.AddExportGlob("memoryBase", new Value(Type.i32, false, 0));
            this.AddExportGlob("tableBase", new Value(Type.i32, false, 0));

            this.AddExportMemory("memory", new Memory(256, 256));

            this.AddExportTable("table", new Table(0x70, 294, 294));
        }
    }
}
