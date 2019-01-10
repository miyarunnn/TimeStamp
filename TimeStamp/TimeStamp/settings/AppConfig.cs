using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TimeStamp.settings
{
    class AppConfig
    {
        static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static void updatePasteFormat(string value)
        {
            config.AppSettings.Settings["pasteFormat"].Value = value;
            config.Save();
        }
        public static void updateSelected(string value)
        {
            config.AppSettings.Settings["selected"].Value = value;
            config.Save();
        }
        public static void updateFreeformat(string value)
        {
            config.AppSettings.Settings["freeformat"].Value = value;
            config.Save();
        }
    }
}
