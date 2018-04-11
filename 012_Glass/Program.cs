using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass
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
