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

    public partial class frmTrigger : Form
    {
        public frmTrigger(IList<Model> models, string label, Metric[] metrics)
        {
            InitializeComponent();
            this.triggerCtl.SetLabel(label);
            this.triggerCtl.SetModels(models);
            this.triggerCtl.SetMetrics(metrics);
        }


        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }


        public static void ShowAdd<T>(IEnumerable<T> models, string label, Metric[] metrics) 
            where T: Model
        {
            SystemManager.OpenForm(new frmTrigger(models.Cast<Model>().ToList(), label, metrics), true, false);
        }
        public static void ShowEdit<T>(IEnumerable<T> models, string label, Metric[] metrics) 
            where T : Model
        {
            var frm = new frmTrigger(models.Cast<Model>().ToList(), label, metrics);
            frm.Text = "修改任务";
            frm.ok_button.Text = "修改";
            SystemManager.OpenForm(frm, true, false);
        }

        public static void ShowAdd<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics) 
            where T1 : Model
            where T2 : class
        {
            frmTrigger2.ShowAdd(models, parentLabel, childLabel, metrics);
        }

        public static void ShowEdit<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics) 
            where T1 : Model
            where T2 : class
        {
            frmTrigger2.ShowEdit(models, parentLabel, childLabel, metrics);
        }
    }
}
