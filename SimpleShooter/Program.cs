using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
