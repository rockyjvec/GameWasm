using System;
using System.Collections.Generic;
using System.IO;

namespace WebAssembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();

        Stack<object> stack = new Stack<object>();

        public Store()
        {
            this.init();
        }

        public void PushValue(Value v)
        {
            this.stack.Push(v);
        }

        public Value PopValue()
        {
            Value v = this.stack.Pop() as Value;

            if(v == null)
            {
                throw new Exception("Invalid expected value on stack");
            }

            return v;
        }

        public Module.Module LoadModule(string name, string fileName)
        {         
            return this.LoadModule(name, File.ReadAllBytes(fileName));
        }

        public Module.Module LoadModule(string name, byte[] bytes)
        {
            return this.LoadModule(new Module.Module(name, this, bytes));
        }

        public Module.Module LoadModule(Module.Module module)
        {
            this.Modules.Add(module.Name, module);
            return module;
        }

        private void init()
        {
            this.LoadModule(new Module.Global.Global(this));
            this.LoadModule(new Module.Global.Math(this));
            this.LoadModule(new Module.Env(this));
            this.LoadModule(new Module.Asm2Wasm(this));
        }
    }
}
