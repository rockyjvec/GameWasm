using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAssembly.Module
{
    class Env : Module
    {
        UInt32 tempRet0;
        WebAssembly.Global DYNAMICTOP_PTR;



        public Env(Store store) : base("env", store)
        {
            this.AddExportFunc("abort", new byte[] { Type.i32 }, new byte[] { }, this.abort);
            this.AddExportFunc("setTempRet0", new byte[] { Type.i32 }, new byte[] { }, this.setTempRet0);
            this.AddExportFunc("getTempRet0", new byte[] { }, new byte[] { Type.i32 }, this.getTempRet0);
            this.AddExportFunc("invoke_vii",this.invoke_vii());
            this.AddExportFunc("___buildEnvironment", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("___clock_gettime", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___lock", new byte[] { Type.i32 });
            this.AddExportFunc("___map_file", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("___setErrNo", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("___syscall10", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall140", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall145", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall146", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall196", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall221", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall330", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall38", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall40", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall5", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall54", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall6", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall63", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___syscall91", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.i32 }, this.syscall);
            this.AddExportFunc("___unlock", new byte[] { Type.i32 });
            this.AddExportFunc("_abort", new byte[] { }, new byte[] { }, this._abort);
            this.AddExportFunc("_clock", new byte[] { }, new byte[] { Type.i32 });
            this.AddExportFunc("_difftime", new byte[] { Type.i32, Type.i32 }, new byte[] { Type.f64 });
            this.AddExportFunc("_emscripten_get_heap_size", new byte[] { }, new byte[] { Type.i32 }, this._emscripten_get_heap_size);
            this.AddExportFunc("_emscripten_memcpy_big", new byte[] { Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_emscripten_resize_heap", new byte[] { Type.i32 }, new byte[] { Type.i32 }, this._emscripten_resize_heap);
            this.AddExportFunc("_exit", new byte[] { Type.i32 }, new byte[] { });
            this.AddExportFunc("_getenv", new byte[] { Type.i32 }, new byte[] { Type.i32 }, this._getenv);
            this.AddExportFunc("_gmtime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_llvm_log10_f64", new byte[] { Type.f64 }, new byte[] { Type.f64 });
            this.AddExportFunc("_llvm_log2_f64", new byte[] { Type.f64 }, new byte[] { Type.f64 });
            this.AddExportFunc("_localtime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_longjmp", this._longjmp());
            this.AddExportFunc("_mktime", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_strftime", new byte[] { Type.i32, Type.i32, Type.i32, Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_system", new byte[] { Type.i32 }, new byte[] { Type.i32 });
            this.AddExportFunc("_time", new byte[] { Type.i32 }, new byte[] { Type.i32 }, this._time);
            this.AddExportFunc("abortOnCannotGrowMemory", new byte[] { Type.i32 }, new byte[] { Type.i32 }, this.abortOnCannotGrowMemory);

            this.AddExportGlob("__table_base", Type.i32, true, (UInt32)0);

            this.DYNAMICTOP_PTR = this.AddExportGlob("DYNAMICTOP_PTR", Type.i32, true, (UInt32)19984);

            var memory = new Memory(256, 256);
            memory.SetBytes((UInt64)19984, BitConverter.GetBytes((UInt32)(5264096)));
  
            this.Memory.Add(memory);
            this.AddExportMemory("memory", memory);

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

        public object[] abortOnCannotGrowMemory(object[] parameters)
        {
            return this.abort(parameters);
        }


        public object[] _emscripten_get_heap_size(object[] parameters)
        {
            return new object[] { (UInt32) (this.Memory[0].CurrentPages * 65536) };
        }

        public object[] _emscripten_resize_heap(object[] parameters)
        {
            return this.abortOnCannotGrowMemory(parameters);
        }

        public object[] setTempRet0(object[] parameters)
        {
            this.tempRet0 = (UInt32)parameters[0];
            return new object[] { };
        }

        public object[] getTempRet0(object[] parameters)
        {
            return new object[] { (UInt32) this.tempRet0 };
        }

        public Function invoke_vii()
        {
            UInt32 sp = 0;
            var start = new Instruction.Custom(delegate
            {
                this.Store.Modules["lua"].Execute("stackSave");
            });

            start.Next = new Instruction.Custom(delegate
            {
                sp = this.Store.Stack.PopI32();
                this.Store.Modules["lua"].Execute("dynCall_vii", this.Store.CurrentFrame.Locals.ToArray());
            });

            start.Next.Next = new Instruction.Custom(delegate
            {
                if(this.Store.Stack.Thrown != null)
                {
                    this.Store.Modules["lua"].Execute("stackRestore", new object[] { sp });                   
                }
            });

            start.Next.Next.Next = new Instruction.Custom(delegate
            {
                if (this.Store.Stack.Thrown != null)
                {
                    this.Store.Stack.Thrown = null;
                    this.Store.Modules["lua"].Execute("_setThrew", new object[] { (UInt32)1, (UInt32)0 });
                }
            });

            var f = new Function(
               this,
               new byte[] { Type.i32, Type.i32, Type.i32 },
               new byte[] { },
               start
            );
            f.Catcher = true;
            return f;
        }

        public Function _longjmp()
        {
            var start = new Instruction.Custom(delegate
            {
                this.Store.Modules["lua"].Execute("_setThrew", new object[] { (UInt32)this.Store.CurrentFrame.Locals[0], (UInt32)this.Store.CurrentFrame.Locals[1] });
            });

            start.Next = new Instruction.Custom(delegate
            {
                this.Store.Stack.Throw("longjmp");
            });

            return new Function(
               this,
               new byte[] { Type.i32, Type.i32 },
               new byte[] { },
               start
           );
        }

        public UInt32 dynamicAlloc(UInt32 size)
        {
            UInt32 ret = this.Memory[0].GetI32(this.DYNAMICTOP_PTR.GetI32());
            UInt32 end = (ret + size + 15) & 0xFFFFFFF0;
            if (end <= (UInt32)(this._emscripten_get_heap_size(null)[0]))
            {
                this.Memory[0].SetI32(this.DYNAMICTOP_PTR.GetI32(), end);
            }
            else
            {
                return 0;
            }
            return ret;
        }

        public object[] _getenv(object[] parameters)
        {

            return new object[] { (UInt32)0 };
        }

        public object[] syscall(object[] parameters)
        {
            UInt32 num = (UInt32)parameters[0];
            UInt32 varargs = (UInt32)parameters[1];
            UInt32 ret = 0;
            switch (num)
            {
                case 54: // ioctl
                    ret = 0;
                    break;
                case 146: // writev
                    ret = this.writev(varargs);
                    break;
                default:
                    throw new Exception("Syscall " + num + " not implemented");
            }

            return new object[] { ret };

        }

        public UInt32 writev(UInt32 varargs)
        {
            UInt32 stream = this.Memory[0].GetI32(varargs); varargs += 4;
            UInt32 iov = this.Memory[0].GetI32(varargs); varargs += 4;
            UInt32 iovcnt = this.Memory[0].GetI32(varargs); varargs += 4;
            UInt32 ret = 0;
            for (UInt32 i = 0; i < iovcnt; i++)
            {
                var ptr = this.Memory[0].GetI32(iov + (i * 8));
                var len = this.Memory[0].GetI32(iov + (i * 8) + 4);
                Console.Write(System.Text.Encoding.Default.GetString(this.Memory[0].GetBytes(ptr, len)));
                ret += len;
            }

            return ret;
        }        
    }
}
