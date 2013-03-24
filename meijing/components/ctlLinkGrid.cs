using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using meijing.ui.module;

namespace meijing.ui.components
{

    public partial class ctlLinkGrid : UserControl
    {
        public ctlLinkGrid()
        {
            InitializeComponent();
        }
        public ctlLinkGrid(IList<Link> links)
        {
            InitializeComponent();
            SetLinks(links);
        }


        public IList<Link> SelectedLinks()
        {
            List<Link> links = new List<Link>();
            var items = this.listView.CheckedItems;
            for (int i = 0; i < items.Count; i++)
            {
                links.Add(items[i].Tag as Link);
            }
            return links;
        }

        public void SetLinks(IList<Link> links) {
            this.listView.BeginUpdate();
            try
            {
                listView.Items.Clear();
                foreach (var link in links)
                {
                    var item = listView.Items.Add(link.Id);
                    item.SubItems.Add(link.Name);
                    item.SubItems.Add(link.Device1Id);
                    item.SubItems.Add(link.IfIndex1.ToString());
                    item.SubItems.Add(link.Device2Id);
                    item.SubItems.Add(link.IfIndex2.ToString());
                    item.Tag = link;
                }
            }
            finally 
            {
                this.listView.EndUpdate();
            }
        }
    }
}
