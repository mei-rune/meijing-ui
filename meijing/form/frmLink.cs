using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace meijing.ui
{
    using meijing.ui.module;

    public partial class frmLink : Form
    {
        private readonly IDictionary<Device, IList<Interface>> devices;
        public frmLink(IDictionary<Device, IList<Interface>> devices)
        {
            this.devices = devices;

            InitializeComponent();
            foreach (var kp in devices) 
            {
                this.device1Box.Items.Add(kp.Key);
                this.device2Box.Items.Add(kp.Key);
            }
        }

        public string LinkName
        {
            get { return this.nameBox.Text; }
            set { this.nameBox.Text= value; }
        }

        public string Description
        {
            get { return this.descriptionBox.Text; }
            set { this.descriptionBox.Text = value; }
        }

        public int SamplingDirect
        {
            get
            {
                if (-1 == this.samplingBox.SelectedIndex)
                {
                    return 1;
                }
                return this.samplingBox.SelectedIndex + 1;
            }
            set { this.samplingBox.SelectedIndex = value; }
        }

        public Device Device1 {
            get { return this.device1Box.SelectedItem as Device; }
            set { this.device1Box.SelectedItem = value; }
        }

        public Interface Port1
        {
            get { return this.port1Box.SelectedItem as Interface; }
            set { this.port1Box.SelectedItem = value; }
        }

        public Device Device2
        {
            get { return this.device2Box.SelectedItem as Device; }
            set { this.device2Box.SelectedItem = value; }
        }

        public Interface Port2
        {
            get { return this.port2Box.SelectedItem as Interface; }
            set { this.port2Box.SelectedItem = value; }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            try
            {
                if ("" == this.nameBox.Text)
                {
                    MyMessageBox.ShowMessage("错误", "名称不能为空");
                    return;
                }

                if (null == this.Device1)
                {
                    MyMessageBox.ShowMessage("错误", "设备1 必须选择一个设备");
                    return;
                }

                if (null == this.Device2)
                {
                    MyMessageBox.ShowMessage("错误", "设备2 必须选择一个设备");
                    return;
                }

                var link = new Link();
                link.Name = this.LinkName;
                link.Description = this.Description;
                link.Device1Id = Device1.Id;
                link.IfIndex1 = Port1.IfIndex;
                link.Device2Id = Device2.Id;
                link.IfIndex2 = Port1.IfIndex;
                link.SamplingDirect = SamplingDirect;
                link.CreateIt();

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.ShowMessage("错误", "添加线路失败!", ex.ToString());
            }
        }

        private Interface fillPortBox(ComboBox box, Device drv, Interface port)
        {
            box.Items.Clear();
            var links = this.devices[drv];
            if (null == links)
            {
                return null;
            }

            Interface new_port = null;
            foreach (var ifc in links)
            {
                box.Items.Add(ifc);

                if (null != port && ifc.IfIndex == port.IfIndex)
                {
                    new_port = ifc;
                }
            }
            return new_port;
        }
        private void device1Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            var port = fillPortBox(this.port1Box, this.Device1, this.Port1);
            if (null != port)
            {
                this.Port1 = port;
            }
        }

        private void device2Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            var port = fillPortBox(this.port2Box, this.Device2, this.Port2);
            if (null != port)
            {
                this.Port2 = port;
            }

        }
    }
}
