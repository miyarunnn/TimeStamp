using System;
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

namespace TimeStamp
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
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

        }

        private void label1_Click(object sender, EventArgs e)
        {

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
    }
}
