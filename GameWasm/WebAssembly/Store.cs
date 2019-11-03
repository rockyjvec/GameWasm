using System;
using System.Collections.Generic;
using System.IO;
using GameWasm.Webassembly.New;

namespace GameWasm.Webassembly
{
    public class Store
    {
        public Dictionary<string, Module.Module> Modules = new Dictionary<string, Module.Module>();
        
        public Runtime runtime = new Runtime();

        public Store(string[] args = null, string[] env = null, string directory = ".")
        {
            if (args == null) args = new string[] { };
            if (env == null) env = new string[] { };
            
            LoadModule(new Module.Wasi(this, env, directory, args));
        }

        public Module.Module LoadModule(string name, string fileName)
        {         
            return LoadModule(name, File.ReadAllBytes(fileName));
        }

        public Module.Module LoadModule(string name, byte[] bytes)
        {
            return LoadModule(new Module.Module(name, this, bytes));
        }

        public Module.Module LoadModule(Module.Module module)
        {
            Modules.Add(module.Name, module);
            return module;
        }

        // Returning false means execution is complete
        public bool Step(int count = 1)
        {
            return runtime.Step(count);
        }
    }
}
