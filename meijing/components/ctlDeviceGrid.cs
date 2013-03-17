using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace meijing.ui.components
{
    using meijing.ui.module;

    public partial class ctlDeviceGrid : UserControl
    {
        public ctlDeviceGrid()
        {
            InitializeComponent();
        }

        public IList<Device> SelectedDevices()
        {
            List<Device> devices = new List<Device>();
            var items = this.listView.CheckedItems;
            for (int i = 0; i < items.Count; i++)
            {
                devices.Add(items[i].Tag as Device);
            }
            return devices;
        }


        public void SetDevices(IList<Device> devices)
        {
            this.listView.BeginUpdate();
            try
            {
                listView.Items.Clear();
                foreach (var drv in devices)
                {
                    var item = listView.Items.Add(drv.Name);
                    item.SubItems.Add(drv.Address);
                    item.SubItems.Add(drv.Description);
                    item.SubItems.Add(drv.CreatedAt.ToString());
                    item.SubItems.Add(drv.UpdatedAt.ToString());
                    item.Tag = drv;
                }
            }
            finally
            {
                this.listView.EndUpdate();
            }
        }
    }
}
