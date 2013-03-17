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

    public partial class ctlTrigger : UserControl
    {
        IList<Model> models;
        IDictionary<string, Metric> metrics = new Dictionary<string, Metric>();

        public ctlTrigger()
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
            get
            {
                var m = this.kpiBox.SelectedItem as Metric;
                if (null != m)
                {
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

        public void SetLabel(string parent)
        {
            this.topObjectLabel.Text = parent;
        }

        public void SetModels(IList<Model> models) 
        {
            this.models = models;
            this.topObjectBox.Items.AddRange(models.ToArray());
        }

        public void SetMetrics(Metric[] metrics) 
        {
            foreach (var m in metrics) 
            {
                this.metrics[m.Name] = m;
            }
            this.kpiBox.Items.AddRange(metrics);
        }
        
    }
}
