namespace GameWasm.Webassembly.Module.Global
{
    class Global : Module
    {
        public Global(Store store) : base("global", store)
        {
            AddExportGlob("NaN", Type.f64, false, double.NaN);
            AddExportGlob("Infinity", Type.f64, false, double.PositiveInfinity);
        }
    }
}
