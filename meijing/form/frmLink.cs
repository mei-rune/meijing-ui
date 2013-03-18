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
        private string id;
        private readonly IDictionary<Device, IList<Interface>> devices;
        public frmLink(IDictionary<Device, IList<Interface>> devices, Link link)
        {
            this.devices = devices;

            InitializeComponent();
            this.device1Box.Items.AddRange(devices.Keys.Cast<object>().ToArray());
            this.device2Box.Items.AddRange(devices.Keys.Cast<object>().ToArray());

            if (null != link) 
            {
                this.LinkName = link.Name;
                this.Description = link.Description;
                this.SamplingDirect = link.SamplingDirect;

                var drv1 = devices.Keys.FirstOrDefault((Device d) => d.Id == link.Device1Id);
                var drv2 = devices.Keys.FirstOrDefault((Device d) => d.Id == link.Device2Id);

                this.Device1 = drv1;
                this.Device2 = drv2;
                if (null != drv1)
                {
                    device1Box_SelectedIndexChanged(this.port1Box, EventArgs.Empty);
                    this.Port1 = devices[drv1].FirstOrDefault((Interface ifc) => ifc.IfIndex == link.IfIndex1);
                }
                if (null != drv2)
                {
                    device2Box_SelectedIndexChanged(this.port2Box, EventArgs.Empty);
                    this.Port2 = devices[drv2].FirstOrDefault((Interface ifc) => ifc.IfIndex == link.IfIndex2);
                }
                this.id = link.Id;
                this.ok_button.Text = "修改";
                this.Text = "修改线路...";
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
            set { this.samplingBox.SelectedIndex = value-1; }
        }

        public Device Device1 {
            get { return this.device1Box.SelectedItem as Device; }
            set
            {
                if (null == value) { return;  }
                this.device1Box.SelectedItem = value;
            }
        }

        public Interface Port1
        {
            get { return this.port1Box.SelectedItem as Interface; }
            set
            {
                if (null == value) { return; }
                this.port1Box.SelectedItem = value;
            }
        }

        public Device Device2
        {
            get { return this.device2Box.SelectedItem as Device; }
            set
            {
                if (null == value) { return; }
                this.device2Box.SelectedItem = value;
            }
        }

        public Interface Port2
        {
            get { return this.port2Box.SelectedItem as Interface; }
            set
            {
                if (null == value) { return; }
                this.port2Box.SelectedItem = value;
            }
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
                if (!string.IsNullOrEmpty(id))
                {
                    link.Id = id;
                }
                link.Name = this.LinkName;
                link.Description = this.Description;
                link.Device1Id = Device1.Id;
                if (null != this.Port1)
                {
                    link.IfIndex1 = Port1.IfIndex;
                }

                link.Device2Id = Device2.Id;
                if (null != this.Port2)
                {
                    link.IfIndex2 = this.Port2.IfIndex;
                }

                link.SamplingDirect = SamplingDirect;
                link.SaveIt();

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
