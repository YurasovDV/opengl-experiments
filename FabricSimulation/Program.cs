using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
