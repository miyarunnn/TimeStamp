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
        bool alartflg = false;
        private bool flag = false;
        Keys[] ShortcutKeys;

        public List<Keys> keyList = new List<Keys>();
        public bool InputKeyCheck(Keys keyCode)
        {
            //configからショートカットキーの文字列取得
            Keys[] ShortcutKeys = Config_ShortcutKeysConvert();
            //foreach (Keys key in keyList) {
                Console.WriteLine("flag:" + flag);
                Console.WriteLine("keyCode:" + keyCode);
                //Console.WriteLine("key:" + key);
                Console.WriteLine("keyList.Contains(keyCode):" + keyList.Contains(keyCode));
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■");
                if (keyList.Contains(keyCode))
                {
                    //入力されたキーが設定されたショートカットのキーと一致していればそのキーをリストから削除
                    keyList.RemoveAll(s => s == keyCode);
                    flag = true;
                }
                else
                {
                    //入力されたキーが設定されたショートカットと違ければリストを初期化
                    ShortcutKeyListInit();
                    flag = false;
                }
            //}

            return flag;
        }

        public Keys[] Config_ShortcutKeysConvert()
        {
            //Keys[] ShortcutKeys;
            int[] defaultIntAry = new int[2] {163, 144 };
            //configからショートカットキーの文字列取得
            String Config_ShortcutKeys = ConfigurationManager.AppSettings["hiddenShrotcut"];
            if (!String.IsNullOrEmpty(Config_ShortcutKeys)) { 
                Config_ShortcutKeys = Config_ShortcutKeys.Replace(" ", "");
                String[] Config_ShortcutKeysStrAry = Config_ShortcutKeys.Split(',');

                int[] ShortcutKeysIntAry = Config_ShortcutKeysStrAry.Select(s => KeyDown.keyCodeDictionaly[s]).ToArray();
                ShortcutKeys = ShortcutKeysIntAry.Select(s => KeyDown.keyDataDictionaly[s]).ToArray();
                return ShortcutKeys;
            }
            else
            {
                if (!alartflg) { 
                    MessageBox.Show("ショートカットキーが指定されていないため、デフォルトでCtrl+NumLockが割り当てられます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ShortcutKeys = defaultIntAry.Select(s => KeyDown.keyDataDictionaly[s]).ToArray();
                    alartflg = true;
                    return ShortcutKeys;
                }

                else
                {
                    return ShortcutKeys;
                }
            }
            
        }

        /// <summary>
        /// 入力されたキーが誤っていた場合もしくはアプリ起動時にShortcutのリストを初期化
        /// </summary>
        public void ShortcutKeyListInit()
        {
            keyList.Clear();
            keyList.AddRange(Config_ShortcutKeysConvert());
        }

    }
}
