using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TimeStamp.Logic
{


    public class ClipBoardLogic
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        String strBackup = "";
        Image imgBackup;

        public void makeBackup()
        {
            IDataObject data = Clipboard.GetDataObject();

            //クリップボードに文字列が格納されていた場合
            if (data.GetDataPresent(DataFormats.Text))
            {
                strBackup = (string)data.GetData(DataFormats.Text);
            }
            //クリップボードに画像が格納されていた場合
            else if (data.GetDataPresent(DataFormats.Bitmap))
            {
                imgBackup = (Image)data.GetData(DataFormats.Bitmap);
            }
        }

        public void setClipborard(String date)
        {
            try { 
                //Clipboard.SetText(date);
                Clipboard.SetDataObject(date, true, 15, 100);
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                IntPtr hWnd = GetOpenClipboardWindow();
                if (IntPtr.Zero != hWnd)
                {
                    uint pid = 0;
                    uint tid = GetWindowThreadProcessId(hWnd, out pid);
                    MessageBox.Show("クリップボードを開けませんでした。以下のプログラムが使用中です：" + Environment.NewLine + System.Diagnostics.Process.GetProcessById((int)pid).Modules[0].FileName);
                }
            }
        }

        public void RestoreBackup()
        {
            try { 
                if(String.IsNullOrEmpty(strBackup) && imgBackup == null)
                {
                    strBackup = "";
                    imgBackup = null;
                }
                if (String.IsNullOrEmpty(strBackup) && imgBackup != null)
                {
                    Clipboard.SetDataObject(imgBackup, true, 15, 100);
                }
                if(!String.IsNullOrEmpty(strBackup) && imgBackup == null)
                {
                    Clipboard.SetDataObject(strBackup, true, 15, 100);
                    //Clipboard.SetText(strBackup);
                }
            }
            catch(System.Runtime.InteropServices.ExternalException e)
            {
                
            }

            //バックアップを初期化（メモリを使用し続けそうで怖い）
            strBackup = "";
            imgBackup = null;
        }
        
    }
}
