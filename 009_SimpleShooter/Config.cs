using System.Configuration;

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

        public static bool ShowBoundingBox
        {
            get
            {
                return ConfigurationManager.AppSettings["showBox"] == bool.TrueString;
            }
        }
    }
}
