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
        AppConfig AppConfig;
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now;
            var pasteFormat = ConfigurationManager.AppSettings["pasteFormat"];
            label1.Text = datetime.ToString(pasteFormat);

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

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                AppConfig.updatePasteFormat(radioButton1.Text);
                AppConfig.updateSelected(radioButton1.Name);
            }
            if (radioButton2.Checked)
            {
                AppConfig.updatePasteFormat(radioButton2.Text);
                AppConfig.updateSelected(radioButton2.Name);
            }
            if (radioButton3.Checked)
            {
                AppConfig.updatePasteFormat(radioButton3.Text);
                AppConfig.updateSelected(radioButton3.Name);
            }
            if (radioButton4.Checked)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    dt.ToString(textBox1.Text);
                    AppConfig.updatePasteFormat(textBox1.Text);
                    AppConfig.updateSelected(radioButton4.Name);
                    AppConfig.updateFreeformat(textBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("入力された形式で日付に変換することができません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            updateLabel1();
        }

        private void updateLabel1()
        {
            DateTime datetime = DateTime.Now;
            var pasteFormat = ConfigurationManager.AppSettings["pasteFormat"];
            label1.Text = datetime.ToString(pasteFormat);
            this.Update();
        }

    }
}
