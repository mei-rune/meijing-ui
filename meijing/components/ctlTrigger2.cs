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

    public partial class ctlTrigger2 : UserControl
    {
        IDictionary<string, Metric> metrics = new Dictionary<string, Metric>();
        IDictionary<Model, IList<object>> models;
        public ctlTrigger2()
        {
            InitializeComponent();
        }

        public string RuleName
        {
            get { return this.nameBox.Text; }
            set { this.nameBox.Text = value; }
        }

        public int Interval
        {
            get { return int.Parse(this.pollIntervalBox.Text); }
            set { this.pollIntervalBox.Text = value.ToString(); }
        }

        public string KPI
        {
            get { 
                var m = this.kpiBox.SelectedItem as Metric;
                if(null != m) {
                    return m.Name;
                }
                return this.kpiBox.Text;
            }
            set 
            {
                Metric m;
                if (metrics.TryGetValue(value, out m))
                {
                    this.kpiBox.SelectedItem = m;
                }
                else 
                {
                    this.kpiBox.Text = value;
                }
            }
        }

        public Model Model
        {
            get { return this.topObjectBox.SelectedItem as Model; }
            set { this.topObjectBox.SelectedItem = value; }
        }

        public object Child 
        {
            get { return this.childrenBox.SelectedItem;  }
            set { this.childrenBox.SelectedItem = value; }
        }

        public void SetLabel(string parent, string child)
        {
            this.topObjectLabel.Text = parent;
            this.childObjectLabel.Text = child;
        }

        public void SetModels(IDictionary<Model, IList<object>> models) 
        {
            this.models = models;
            this.topObjectBox.Items.AddRange(models.Keys.ToArray());
        }

        public void SetMetrics(Metric[] metrics)
        {
            foreach (var m in metrics)
            {
                this.metrics[m.Name] = m;
            }
            this.kpiBox.Items.AddRange(metrics);
        }

        private void topObjectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.childrenBox.Items.Clear();
            var items = this.models[this.Model];
            if (null == items) 
            {
                return ;
            }

            this.childrenBox.Items.AddRange(items.ToArray());
        }
    }
}
