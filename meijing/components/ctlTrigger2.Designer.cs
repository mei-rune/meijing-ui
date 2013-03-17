namespace meijing.ui.components
{
    partial class ctlTrigger2
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pollIntervalBox = new System.Windows.Forms.ComboBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.topObjectLabel = new System.Windows.Forms.Label();
            this.topObjectBox = new System.Windows.Forms.ComboBox();
            this.childrenBox = new System.Windows.Forms.ComboBox();
            this.childObjectLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.kpiBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称:";
            // 
            // pollIntervalBox
            // 
            this.pollIntervalBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pollIntervalBox.FormatString = "N0";
            this.pollIntervalBox.FormattingEnabled = true;
            this.pollIntervalBox.Items.AddRange(new object[] {
            "15",
            "30",
            "45",
            "60",
            "120",
            "180",
            "240",
            "300",
            "600"});
            this.pollIntervalBox.Location = new System.Drawing.Point(94, 37);
            this.pollIntervalBox.Name = "pollIntervalBox";
            this.pollIntervalBox.Size = new System.Drawing.Size(204, 20);
            this.pollIntervalBox.TabIndex = 1;
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(94, 4);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(204, 21);
            this.nameBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "轮询间隔(秒):";
            // 
            // topObjectLabel
            // 
            this.topObjectLabel.AutoSize = true;
            this.topObjectLabel.Location = new System.Drawing.Point(-3, 72);
            this.topObjectLabel.Name = "topObjectLabel";
            this.topObjectLabel.Size = new System.Drawing.Size(65, 12);
            this.topObjectLabel.TabIndex = 4;
            this.topObjectLabel.Text = "一级对象：";
            // 
            // topObjectBox
            // 
            this.topObjectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topObjectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topObjectBox.FormattingEnabled = true;
            this.topObjectBox.Location = new System.Drawing.Point(94, 69);
            this.topObjectBox.Name = "topObjectBox";
            this.topObjectBox.Size = new System.Drawing.Size(204, 20);
            this.topObjectBox.TabIndex = 5;
            this.topObjectBox.SelectedIndexChanged += new System.EventHandler(this.topObjectBox_SelectedIndexChanged);
            // 
            // childrenBox
            // 
            this.childrenBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.childrenBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.childrenBox.FormattingEnabled = true;
            this.childrenBox.Location = new System.Drawing.Point(94, 102);
            this.childrenBox.Name = "childrenBox";
            this.childrenBox.Size = new System.Drawing.Size(204, 20);
            this.childrenBox.TabIndex = 6;
            // 
            // childObjectLabel
            // 
            this.childObjectLabel.AutoSize = true;
            this.childObjectLabel.Location = new System.Drawing.Point(-1, 105);
            this.childObjectLabel.Name = "childObjectLabel";
            this.childObjectLabel.Size = new System.Drawing.Size(65, 12);
            this.childObjectLabel.TabIndex = 7;
            this.childObjectLabel.Text = "二级对象：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "指标名：";
            // 
            // kpiBox
            // 
            this.kpiBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kpiBox.FormattingEnabled = true;
            this.kpiBox.Location = new System.Drawing.Point(94, 135);
            this.kpiBox.Name = "kpiBox";
            this.kpiBox.Size = new System.Drawing.Size(204, 20);
            this.kpiBox.TabIndex = 8;
            // 
            // ctlTrigger2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.kpiBox);
            this.Controls.Add(this.childObjectLabel);
            this.Controls.Add(this.childrenBox);
            this.Controls.Add(this.topObjectBox);
            this.Controls.Add(this.topObjectLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.pollIntervalBox);
            this.Controls.Add(this.label1);
            this.Name = "ctlTrigger2";
            this.Size = new System.Drawing.Size(304, 160);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox pollIntervalBox;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label topObjectLabel;
        private System.Windows.Forms.ComboBox topObjectBox;
        private System.Windows.Forms.ComboBox childrenBox;
        private System.Windows.Forms.Label childObjectLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox kpiBox;
    }
}
