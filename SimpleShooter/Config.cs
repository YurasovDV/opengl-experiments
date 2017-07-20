using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleShooter
{
    static class Config
    {
        public static bool IsSoundOn
        {
            get
            {
                return ConfigurationManager.AppSettings["enableSound"] == bool.TrueString;
            }
        }
    }
}
