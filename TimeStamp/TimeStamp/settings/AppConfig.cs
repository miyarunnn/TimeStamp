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
        public static void UpdatePasteFormat(string value)
        {
            config.AppSettings.Settings["pasteFormat"].Value = value;
            config.Save();
        }
        public static void UpdateSelected(string value)
        {
            config.AppSettings.Settings["selected"].Value = value;
            config.Save();
        }
        public static void UpdateFreeformat(string value)
        {
            config.AppSettings.Settings["freeformat"].Value = value;
            config.Save();
        }

        public static void UpdateInputkey(string value)
        {
            config.AppSettings.Settings["inputkey"].Value = value;
            config.Save();
        }

        public static void UpdateHiddenShrotcut(string value)
        {
            if(value.Equals("ControlKey, Control"))
            {
                value = "ControlKey";
            }
            config.AppSettings.Settings["hiddenShrotcut"].Value = value;
            config.Save();
        }
    }
}
