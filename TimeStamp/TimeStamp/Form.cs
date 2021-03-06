﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using TimeStamp.settings;
using TimeStamp.Logic;

namespace TimeStamp
{
    public partial class Form : System.Windows.Forms.Form
    {
        private InputKeyCheckLogic InputKeyCheckLogic = new InputKeyCheckLogic();
        private String strDate;

        public Form()
        {
            InitializeComponent();
        }
        private static void InterceptKeyboard_KeyUpEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            Console.WriteLine("Keyup KeyCode {0}", e.KeyCode);
        }

        private void InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            //****************************
            //ここに決められたキーが入力された時の処理追加

            //キー入力値がショートカットで設定されたキーと一致するか確認
            InputKeyCheckLogic.InputKeyCheck(e.KeyCode);

            if(InputKeyCheckLogic.keyList.Count() == 0) {

                ClipBoardLogic clipBoardLogic = new ClipBoardLogic();
                //クリップボードのバックアップ取得
                clipBoardLogic.makeBackup();

                //貼り付けた後はリストをショートカットリストをリセット
                InputKeyCheckLogic.ShortcutKeyListInit();
                DateTime datetime = DateTime.Now;
                var pasteFormat = ConfigurationManager.AppSettings["pasteFormat"];

                //クリップボードに日付をセット
                clipBoardLogic.setClipborard(datetime.ToString(pasteFormat));

                //Ctrl+Vを送信して貼り付け
                SendKeys.Send("^v");

                //バックアップを戻す
                clipBoardLogic.RestoreBackup();
            }
            //Console.WriteLine("Keydown KeyCode {0}", e.KeyCode);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            setComponents();
            //TimeStamp.Logic.KeyboardHook.Start();
            var interceptKeyboard = new InterceptKeyboard();
            interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            //interceptKeyboard.KeyUpEvent += InterceptKeyboard_KeyUpEvent;
            interceptKeyboard.Hook();

            //ショートカットリストの初期化
            InputKeyCheckLogic.ShortcutKeyListInit();

            DateTime datetime = DateTime.Now;
            var pasteFormat = ConfigurationManager.AppSettings["pasteFormat"];
            textBox3.Text = datetime.ToString(pasteFormat);

            var selected = ConfigurationManager.AppSettings["selected"];
            if (selected.Equals(radioButton1.Name))
            {
                radioButton1.Select();
            }
            else if (selected.Equals(radioButton2.Name))
            {
                radioButton2.Select();
            }
            else if (selected.Equals(radioButton3.Name))
            {
                radioButton3.Select();
            }
            else if (selected.Equals(radioButton4.Name))
            {
                radioButton4.Select();
            }

            textBox1.Text = ConfigurationManager.AppSettings["freeformat"];

            textBox2.Text = ConfigurationManager.AppSettings["inputkey"];

            //隠しショートカットラベルの表示有無
            //設定の切り替えはconfigファイルを直接編集する
            //設定値がtrueだとラベルが表示される
            bool hiddenShrotcut;
            if (bool.TryParse(ConfigurationManager.AppSettings["hiddenShrotcutFlag"], out hiddenShrotcut))
            {
                hiddenShortcut.Visible = hiddenShrotcut;
            }
            else
            {
                hiddenShortcut.Visible = false;
            }
            hiddenShortcut.Text = ConfigurationManager.AppSettings["hiddenShrotcut"];

        }


        /// <summary>
        /// タスクバーのアイコン動作
        /// </summary>
        private void setComponents()
        {
            NotifyIcon icon = new NotifyIcon();
            icon.Icon = new Icon("timestamp64.ico");
            icon.Visible = true;
            icon.Text = "timestamp";
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem exitItem = new ToolStripMenuItem();
            ToolStripMenuItem settingItem = new ToolStripMenuItem();
            settingItem.Text = "&設定";
            settingItem.Click += new EventHandler(Form_Show);
            menu.Items.Add(settingItem);

            exitItem.Text = "&終了";
            exitItem.Click += new EventHandler(Close_Click);
            menu.Items.Add(exitItem);

            icon.ContextMenuStrip = menu;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            #region ラジオボタン
            if (radioButton1.Checked)
            {
                AppConfig.UpdatePasteFormat(radioButton1.Text);
                AppConfig.UpdateSelected(radioButton1.Name);
            }
            if (radioButton2.Checked)
            {
                AppConfig.UpdatePasteFormat(radioButton2.Text);
                AppConfig.UpdateSelected(radioButton2.Name);
            }
            if (radioButton3.Checked)
            {
                AppConfig.UpdatePasteFormat(radioButton3.Text);
                AppConfig.UpdateSelected(radioButton3.Name);
            }
            if (radioButton4.Checked)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    dt.ToString(textBox1.Text);
                    AppConfig.UpdatePasteFormat(textBox1.Text);
                    AppConfig.UpdateSelected(radioButton4.Name);
                    AppConfig.UpdateFreeformat(textBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("入力された形式で日付に変換することができません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            #endregion

            #region 貼り付けのショートカット
            AppConfig.UpdateInputkey(textBox2.Text);
            #endregion

            #region 貼り付けの隠しショートカット
            AppConfig.UpdateHiddenShrotcut(hiddenShortcut.Text);
            #endregion
            //List<TimeStamp.Logic.InputSimulatorlogic.Input> inputs = new List<TimeStamp.Logic.InputSimulatorlogic.Input>();
            //TimeStamp.Logic.InputSimulatorlogic.AddKeyboardInput(ref inputs, "ゆっくりしていってね！！");

            updateDisplayDate();
        }

        private void updateDisplayDate()
        {
            DateTime datetime = DateTime.Now;
            var pasteFormat = ConfigurationManager.AppSettings["pasteFormat"];
            textBox3.Text = datetime.ToString(pasteFormat);
            textBox3.Refresh();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            String input = e.KeyData.ToString();
            //1つのKeydataが複数のKeydataの組み合わせでできていたらその時点で整形
            input = shapingKeyData(input);

            String text = multiKeyDown(input);

            try { 
                textBox2.Text = text;
                //hiddenShortcut.Text = e.KeyValue.ToString();
                hiddenShortcut.Text = e.KeyData.ToString();
            }
            catch(KeyNotFoundException ex)
            {
                //何もしない
                textBox2.Text = "";
            }
        }

        ///<summary>
        ///複数キー入力(組み合わせに使用できるのはCtrl,Alt,Shift + いずれかのキー)
        ///</summary>
        private String multiKeyDown(String text)
        {
            String[] aryStr = text.Split(',');
            List <String> strList = new List<String>(aryStr);

            //文字列整形
            shapingKey(strList);
            

            String multiKey = "";
            if(aryStr.Length > 1) { 
                foreach(String str in strList)
                {
                    multiKey += str + " + ";
                }
            }
            if(aryStr.Length == 1)
            {
                multiKey = strList[0];
            }
            

            //複数キー入力されている場合は文字列の最後にある＋を取る
            if (multiKey.Contains("+"))
            {
                multiKey = multiKey.Substring(0,multiKey.Length - 3);
            }

            return multiKey;
        }

        /// <summary>
        /// 1つのKeydataが複数のKeydataの組み合わせでできていた時に整形する
        /// </summary>
        /// <param name="e"></param>
        private String shapingKeyData(String inputKey)
        {
            
            if (inputKey.Contains("ControlKey, OemBackslash"))
            {
                inputKey = inputKey.Replace("ControlKey, OemBackslash", "半/全");
            }
            if (inputKey.Contains("D4, Oemtilde"))
            {
                inputKey = inputKey.Replace("D4, Oemtilde", "半/全");
            }
            if (inputKey.Contains("ShiftKey, OemBackslash"))
            {
                inputKey = inputKey.Replace("ShiftKey, OemBackslash", "カ/ひ");
            }
            return inputKey;
        }

        /// <summary>
        /// キー入力の文字列を整形
        /// </summary>
        /// <param name="strList"></param>
        private void shapingKey(List<String> strList)
        {
            if (strList.Contains(" Control"))
            {
                strList.Remove(" Control");
                strList.Remove("ControlKey");
                strList.Add("Ctrl");
            }
            if (strList.Contains(" Shift"))
            {
                strList.Remove(" Shift");
                strList.Remove("ShiftKey");
                strList.Add("Shift");
            }
            if (strList.Contains(" Alt"))
            {
                strList.Remove(" Alt");
                strList.Remove("Menu");
                strList.Add("Alt");
            }
            if (strList.Contains("Escape"))
            {
                strList.Remove("Escape");
                strList.Add("Esc");
            }
            if (strList.Contains("Oemplus"))
            {
                strList.Remove("Oemplus");
                strList.Add(" ; ");
            }
            if (strList.Contains("Oemtilde"))
            {
                strList.Remove("Oemtilde");
                strList.Add("@");
            }
            if (strList.Contains("OemOpenBrackets"))
            {
                strList.Remove("OemOpenBrackets");
                strList.Add(" [ ");
            }
            if (strList.Contains("Oem1"))
            {
                strList.Remove("Oem1");
                strList.Add(" : ");
            }
            if (strList.Contains("Oem6"))
            {
                strList.Remove("Oem6");
                strList.Add(" ] ");
            }
            if (strList.Contains("Oemcomma"))
            {
                strList.Remove("Oemcomma");
                strList.Add(" , ");
            }
            if (strList.Contains("OemPeriod"))
            {
                strList.Remove("OemPeriod");
                strList.Add(" . ");
            }
            if (strList.Contains("OemQuestion"))
            {
                strList.Remove("OemQuestion");
                strList.Add(" ? ");
            }
            if (strList.Contains("OemBackslash"))
            {
                strList.Remove("OemBackslash");
                strList.Add(" \\ ");
            }
            if (strList.Contains("OemMinus"))
            {
                strList.Remove("OemMinus");
                strList.Add(" - ");
            }
            if (strList.Contains("Oem7"))
            {
                strList.Remove("Oem7");
                strList.Add(" ^ ");
            }
            if (strList.Contains("Oem5"))
            {
                strList.Remove("Oem5");
                strList.Add(" \\ ");
            }
        }

        private void hiddenShortcut_Click(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Form_Show(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
