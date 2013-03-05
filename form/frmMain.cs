using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using meijing.ui.module;
using System.Threading;

namespace meijing.ui
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            GetSystemIcon.InitMainTreeImage();
            GetSystemIcon.InitTabViewImage();
            trvsrvlst.ImageList = GetSystemIcon.MainTreeImage;

            if (!SystemManager.DEBUG_MODE)
            {
                //非Debug模式的时候,UT菜单不可使用
                toolStripMenuItem12.Visible = false;
            }
            this.Text += "  " + SystemManager.Version;
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (SystemManager.MONO_MODE)
            {
                this.Text += " MONO";
            }
            //长时间操作时候，实时提示进度在状态栏中
            SystemManager.ActionDone += new EventHandler<ActionDoneEventArgs>(
                (x, y) =>
                {
                    //1.lblAction 没有InvokeRequired
                    //2.DoEvents必须
                    lblAction.Text = y.Message;
                    Application.DoEvents();
                }
            );
        }

        /// <summary>
        /// 多文档视图管理
        /// </summary>
        Dictionary<String, TabPage> ViewTabList = new Dictionary<String, TabPage>();
     
        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {

        }


        #region"数据库连接"
        /// <summary>
        /// Expand All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvsrvlst.CollapseAll();
        }
        /// <summary>
        /// Collapse All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvsrvlst.BeginUpdate();
            this.trvsrvlst.ExpandAll();
            this.trvsrvlst.EndUpdate();
        }
        /// <summary>
        /// Exit Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion



        #region "Help"
        /// <summary>
        /// About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmAbout(), true, true);
        }
        /// <summary>
        /// Thanks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMessageBox.ShowMessage("Thanks", "DreamOMS",
                                     meijing.ui.module.GetResource.GetImage(meijing.ui.module.ImageType.Smile),
                                     new System.IO.StreamReader("ReleaseNote.txt").ReadToEnd());
        }

        #endregion
       
    }
}
