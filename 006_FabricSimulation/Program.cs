namespace FabricSimulation
{
    static class Program
    {
        public static void Main(string[] args)
        {
            using (var f = new FabricForm())
            {
                f.Run(160);
            }
        }
    }
}
