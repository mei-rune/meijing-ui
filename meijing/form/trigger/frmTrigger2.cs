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

    public partial class frmTrigger2 : Form
    {
        IDictionary<Model, IList<object>> models;
        public frmTrigger2(IDictionary<Model, IList<object>> models,
            string parentLabel, string childLabel, Metric[] metrics)
        {
            this.models = models;
            InitializeComponent();
            this.triggerCtl.SetLabel(parentLabel, childLabel);
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


        public static void ShowAdd<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics)
            where T1 : Model
            where T2 : class
        {
            IDictionary<Model, IList<object>> result = new Dictionary<Model, IList<object>>();
            foreach (var kp in models)
            {
                result[kp.Key] = kp.Value.Cast<object>().ToList();
            }

            SystemManager.OpenForm(new frmTrigger2(result, parentLabel, childLabel, metrics), true, false);
        }

        public static void ShowEdit<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics)
            where T1 : Model
            where T2 : class
        {
            IDictionary<Model, IList<object>> result = new Dictionary<Model, IList<object>>();
            foreach (var kp in models)
            {
                result[kp.Key] = kp.Value.Cast<object>().ToList();
            }

            var frm = new frmTrigger2(result, parentLabel, childLabel, metrics);
            frm.Text = "修改任务";
            frm.ok_button.Text = "修改";
            SystemManager.OpenForm(frm, true, false);
        }
    }
}
