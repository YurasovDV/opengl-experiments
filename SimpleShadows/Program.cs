using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleShadows
{
    class Program
    {
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
