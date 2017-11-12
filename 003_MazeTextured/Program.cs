using System;

namespace MazeTextured
{
    class Program
    {
       [STAThread]
       public static void Main()
        {
           /* Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShadowForm());*/

            using (var f = new MazeForm())
            {
                f.Run(80);
            }

        }
    }
}
