using System;
using System.Collections.Generic;
using System.IO;
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
        public static String SelectObjectTag = String.Empty;

        /// <summary>
        /// 获得当前对象的种类
        /// </summary>
        public static String SelectTagType
        {
            get
            {
                return GetTagType(SelectObjectTag);
            }
        }
        /// <summary>
        /// 获得当前对象的路径
        /// </summary>
        public static String SelectTagData
        {
            get
            {
                return GetTagData(SelectObjectTag);
            }
        }
        /// <summary>
        /// 获得对象的种类
        /// </summary>
        /// <returns></returns>
        public static String GetTagType(String ObjectTag)
        {
            if (ObjectTag == String.Empty)
            {
                return string.Empty;
            }
            else
            {
                return ObjectTag.Split(":".ToCharArray())[0];
            }
        }
        /// <summary>
        /// 获得对象的路径
        /// </summary>
        /// <returns></returns>
        public static String GetTagData(String ObjectTag)
        {
            if (ObjectTag == String.Empty)
            {
                return string.Empty;
            }
            else
            {
                if (ObjectTag.Split(":".ToCharArray()).Length == 2)
                {
                    return ObjectTag.Split(":".ToCharArray())[1];
                }
                else
                {
                    return string.Empty;
                }
            }
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
            mfrm.BackColor = System.Drawing.Color.White;
            mfrm.FormBorderStyle = FormBorderStyle.FixedSingle;
            mfrm.MaximizeBox = false;
            if (isUseAppIcon)
            {
                mfrm.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
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

        /// <summary>
        ///  服务器地址
        /// </summary>
        public static string ServerAddress { get; set; }
    }
}
