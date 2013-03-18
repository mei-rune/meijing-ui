using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace meijing.ui.module
{
    public static class SystemManager
    {
        /// <summary>
        /// 测试模式
        /// </summary>
        public static Boolean DEBUG_MODE = false;
        /// <summary>
        /// 是否为MONO
        /// </summary>
        public static Boolean MONO_MODE = false;
        /// <summary>
        /// 版本号
        /// </summary>
        public static String Version = Application.ProductVersion;

        /// <summary>
        /// 选择对象标签
        /// </summary>
        public static Dictionary<int, string> portTypes;

        /// <summary>
        /// 获得当前对象的种类
        /// </summary>
        public static String GetPortType(int type)
        {
            if (null == portTypes) {
                portTypes = new Dictionary<int,string>();
                if (File.Exists("ports.txt")) {
                    foreach (var s in File.ReadAllLines("ports.txt")) {
                        var idx = s.IndexOf("=");
                        if (-1 != idx) {
                            int t = 0;
                            if (int.TryParse(s.Substring(0, idx), out t)) {
                                portTypes[t] = s.Substring(idx + 1);
                            }
                        }
                    }
                }
            }

            string descr;
            if (portTypes.TryGetValue(type, out descr)) {
                return descr;
            }
            return type.ToString();
        }

        private const string expressionPrefix = "@every ";
        public static string GetExpressionDescription(string s) {
            if (s.StartsWith(expressionPrefix)) { 
                return "每隔" + GetTimeDescription(s.Substring(expressionPrefix.Length).Trim()) +"调度一次";
            }
            return s;
        }
        public static int ParseExpressionAsSecond(string s)
        {
            if (s.StartsWith(expressionPrefix))
            {
                return GetTimeAsSecond(s);
            }
            return 15;
        }

        public static string CreateExpression(int interval, string unit)
        {
            return expressionPrefix + interval.ToString() + unit;
        }

        public static int GetTimeAsSecond(string s)
        {
            int idx = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (-1 == idx)
            {
                return 15;
            }
            var m = s.Substring(idx);
            if ("m" == m || "M" == m)
            {
                return int.Parse(s.Substring(0, idx)) * 60;
            }
            if ("s" == m || "s" == m)
            {
                return int.Parse(s.Substring(0, idx));
            }
            if ("ms" == m || "ms" == m)
            {
                return int.Parse(s.Substring(0, idx))/60;
            }
            return 15;
        }

        public static string GetTimeDescription(string s)
        {
            int idx = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (-1 == idx)
            {
                return s;
            }
            var m = s.Substring(idx);
            if ("m" == m || "M" == m)
            {
                return s.Substring(0, idx) + "分钟";
            }
            if ("s" == m || "s" == m)
            {
                return s.Substring(0, idx) + "秒";
            }
            if ("ms" == m || "ms" == m)
            {
                return s.Substring(0, idx) + "毫秒";
            }
            return s;
        }

        /// <summary>
        /// 文字资源
        /// </summary>
        public static StringResource mStringResource = new meijing.ui.module.StringResource();
        /// <summary>
        /// 对话框子窗体的统一管理
        /// </summary>
        /// <param name="mfrm"></param>
        /// <param name="isDispose">有些时候需要使用被打开窗体产生的数据，所以不能Dispose</param>
        /// <param name="isUseAppIcon"></param>
        public static void OpenForm(Form mfrm, Boolean isDispose, Boolean isUseAppIcon)
        {
            mfrm.StartPosition = FormStartPosition.CenterParent;
            mfrm.BackColor = Color.White;
            mfrm.FormBorderStyle = FormBorderStyle.FixedSingle;
            mfrm.MaximizeBox = false;
            if (isUseAppIcon)
            {
                mfrm.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            mfrm.ShowDialog();
            mfrm.Close();
            if (isDispose) { mfrm.Dispose(); }
        }

        /// <summary>
        /// 获取树形的根
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static TreeNode FindRootNode(TreeNode node)
        {
            if (node.Parent == null)
                return node;
            else
            {
                return FindRootNode(node.Parent);
            }
        }

        internal static void ExceptionDeal(Exception ex)
        {
            MyMessageBox.ShowMessage("Exception", "Exception", ex.ToString(), true);
        }
        internal static void ExceptionDeal(Exception ex, String Title)
        {
            if (Title == String.Empty)
            {
                MyMessageBox.ShowMessage("Exception", "Exception", ex.ToString(), true);
            }
            else
            {
                MyMessageBox.ShowMessage("Exception", Title, ex.ToString(), true);
            }
        }


        /// <summary>
        /// ActionDone
        /// </summary>
        public static EventHandler<ActionDoneEventArgs> ActionDone;

        private static Dictionary<string, string> serviceAddresses = new Dictionary<string, string>();
        /// <summary>
        ///  服务器地址
        /// </summary>
        public static void SetServerAddress(string addr) 
        {
            serviceAddresses["bridge"] = string.Format("http://{0}:7070", addr);
            serviceAddresses["mdb"] = string.Format("http://{0}:7071/mdb", addr);
            serviceAddresses["poller"] = string.Format("http://{0}:7076", addr);
        }

        public static string GetBridge()
        { 
            return serviceAddresses["bridge"];
        }

        private static Client client;
        public static Client Client 
        {
            get {
                if (null == client) {
                    client = new Client(serviceAddresses["mdb"]);
                }
                return client; 
            }
        }
    }
}
