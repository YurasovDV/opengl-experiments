using System;

namespace SimpleShadows
{
    class Program
    {
       [STAThread]
       public static void Main()
        {
           /* Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ShadowForm());*/

            using (var f = new ShadowForm())
            {
                f.Run(80);
            }

        }
    }
}
