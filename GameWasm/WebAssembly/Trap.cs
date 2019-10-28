using System;

namespace GameWasm.Webassembly
{
    public class Trap : Exception
    {
        public string Details;

        public Trap(string message, string details = "") : base(message)
        {
            Details = details;
        }
    }
}
