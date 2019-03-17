using System;
using System.Collections.Generic;
using System.IO;

namespace WebAssembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        public Stack.Stack Stack; 
        public Stack.Frame CurrentFrame = null;

        public Store()
        {
            this.Stack = new Stack.Stack(this);
            this.init();
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

        public bool Step(bool debug = false)
        {
            if (this.CurrentFrame == null)
            {
                return false;
            }
            else
            {
                bool ret = this.CurrentFrame.Step(debug);

                if(ret)
                {
                    return true;
                }
                else
                {
                    return this.Stack.PopFrame();
                }
            }
        }
    }
}
