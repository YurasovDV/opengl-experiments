using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcTreeExample
{
    class Program
    {
        public static void Main()
        {
            using (var f = new MainForm())
            {
                f.Run(60);
            }
        }
    }
}
