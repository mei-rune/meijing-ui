using System;
using System.IO;
using System.Windows.Forms;
using meijing.ui.module;
namespace meijing.ui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            ///这句话如果写到后面去的话，在没有Config文件的时候，服务器树形列表显示不正确
            Application.EnableVisualStyles();
            var txt = MyMessageBox.ShowInput("服务器地址:", "请输入服务器址址...", "127.0.0.1");
            if ("" == txt) {
                return;
            }
            SystemManager.SetServerAddress(txt);
            //SystemManager.DEBUG_MODE = true;
            SystemManager.MONO_MODE = Type.GetType("Mono.Runtime") != null;
            Application.Run(new frmMain());
        }
    }
}
