namespace meijing.ui
{
    partial class frmLink
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLink));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.device1Box = new System.Windows.Forms.ComboBox();
            this.port1Box = new System.Windows.Forms.ComboBox();
            this.port2Box = new System.Windows.Forms.ComboBox();
            this.device2Box = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.samplingBox = new System.Windows.Forms.ComboBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "端口1:";
            // 
            // device1Box
            // 
            this.device1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device1Box.FormattingEnabled = true;
            this.device1Box.Location = new System.Drawing.Point(111, 48);
            this.device1Box.Name = "device1Box";
            this.device1Box.Size = new System.Drawing.Size(259, 20);
            this.device1Box.TabIndex = 2;
            this.device1Box.SelectedIndexChanged += new System.EventHandler(this.device1Box_SelectedIndexChanged);
            // 
            // port1Box
            // 
            this.port1Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.port1Box.FormattingEnabled = true;
            this.port1Box.Location = new System.Drawing.Point(111, 90);
            this.port1Box.Name = "port1Box";
            this.port1Box.Size = new System.Drawing.Size(259, 20);
            this.port1Box.TabIndex = 3;
            // 
            // port2Box
            // 
            this.port2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.port2Box.FormattingEnabled = true;
            this.port2Box.Location = new System.Drawing.Point(492, 90);
            this.port2Box.Name = "port2Box";
            this.port2Box.Size = new System.Drawing.Size(259, 20);
            this.port2Box.TabIndex = 7;
            // 
            // device2Box
            // 
            this.device2Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device2Box.FormattingEnabled = true;
            this.device2Box.Location = new System.Drawing.Point(492, 48);
            this.device2Box.Name = "device2Box";
            this.device2Box.Size = new System.Drawing.Size(259, 20);
            this.device2Box.TabIndex = 6;
            this.device2Box.SelectedIndexChanged += new System.EventHandler(this.device2Box_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(406, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "端口2:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(406, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "设备2:";
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(676, 276);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 8;
            this.ok_button.Text = "添加";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(577, 276);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 9;
            this.cancel_button.Text = "取消";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "线路名:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(406, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "采集端口：";
            // 
            // samplingBox
            // 
            this.samplingBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.samplingBox.FormattingEnabled = true;
            this.samplingBox.Items.AddRange(new object[] {
            "设备1",
            "设备2"});
            this.samplingBox.Location = new System.Drawing.Point(492, 9);
            this.samplingBox.Name = "samplingBox";
            this.samplingBox.Size = new System.Drawing.Size(259, 20);
            this.samplingBox.TabIndex = 15;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(111, 9);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(259, 21);
            this.nameBox.TabIndex = 16;
            // 
            // descriptionBox
            // 
            this.descriptionBox.Location = new System.Drawing.Point(111, 134);
            this.descriptionBox.Multiline = true;
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Size = new System.Drawing.Size(640, 117);
            this.descriptionBox.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "备注：";
            // 
            // frmLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 314);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.descriptionBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.samplingBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.port2Box);
            this.Controls.Add(this.device2Box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.port1Box);
            this.Controls.Add(this.device1Box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLink";
            this.Text = "添加线路...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox device1Box;
        private System.Windows.Forms.ComboBox port1Box;
        private System.Windows.Forms.ComboBox port2Box;
        private System.Windows.Forms.ComboBox device2Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox samplingBox;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.Label label7;
    }
}