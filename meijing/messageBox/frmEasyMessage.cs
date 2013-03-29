﻿using System;
using System.Windows.Forms;

namespace meijing.ui
{
    public partial class frmEasyMessage : Form
    {
        public frmEasyMessage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set Text
        /// </summary>
        /// <param name="Yes"></param>
        /// <param name="No"></param>
        internal void SetText(String OK)
        {
            cmdOK.Text = OK;
        }
        /// <summary>
        /// SetMessage
        /// </summary>
        /// <param name="Message"></param>
        internal void SetMessage(string Message)
        {
            this.lblMessage.Text = Message;
        }
        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
