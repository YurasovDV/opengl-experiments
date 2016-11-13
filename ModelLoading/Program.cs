using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLoading
{
    class Program
    {
        public static void Main()
        {
            using (var f = new ModelForm())
            {
                f.Run(60);
            }

        }
    }
}
