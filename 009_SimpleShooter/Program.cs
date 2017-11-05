namespace SimpleShooter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var f = new MainWindow())
            {
                f.Run(60);
            }
        }
    }
}
