using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeStamp.settings;
using static TimeStamp.Logic.InterceptKeyboard;

namespace TimeStamp.Logic
{
    class InputKeyCheckLogic
    {
        static bool flag = false;
        List<Keys> keyList = new List<Keys>(); 
        static public bool InputKeyCheck(Keys keyCode)
        {
            //configからショートカットキーの文字列取得
            Keys[] ShortcutKeys = Config_ShortcutKeysConvert();
            foreach (Keys key in ShortcutKeys) {
                Console.WriteLine("flag:" + flag);
                Console.WriteLine("keyCode:" + keyCode);
                Console.WriteLine("key:" + key);
                Console.WriteLine("keyCode == key:" + (keyCode == key));
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■");
                if (keyCode == key)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

                if (!flag)
                {
                    break;
                }
            }

            return flag;
        }

        static Keys[] Config_ShortcutKeysConvert()
        {
            //configからショートカットキーの文字列取得
            String Config_ShortcutKeys = ConfigurationManager.AppSettings["hiddenShrotcut"];
            Config_ShortcutKeys = Config_ShortcutKeys.Replace(" ", "");
            String[] Config_ShortcutKeysStrAry = Config_ShortcutKeys.Split(',');

            int[] ShortcutKeysIntAry = Config_ShortcutKeysStrAry.Select(s => KeyDown.keyCodeDictionaly[s]).ToArray();
            Keys[] ShortcutKeys = ShortcutKeysIntAry.Select(s => KeyDown.keyDataDictionaly[s]).ToArray();

            return ShortcutKeys;
        }

    }
}
