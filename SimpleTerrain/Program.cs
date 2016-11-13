namespace SimpleTerrain
{
    class Program
    {
        static int Main(string[] args)
        {
            using (var f = new MainForm())
            {
                f.Run(60);
            }
            return 0;
        }
    }
}
