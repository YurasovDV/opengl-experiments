namespace ShadowMap
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var f = new MainForm())
            {
                f.Run(60);
            }
        }
    }
}
