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
        public frmTrigger(IList<Model> models, string label, Metric[] metrics, Model model)
        {
            InitializeComponent();
            this.triggerCtl.SetLabel(label);
            this.triggerCtl.SetModels(models);
            this.triggerCtl.SetMetrics(metrics);
            this.triggerCtl.Model = model;
        }

        public void SetTrigger(Trigger trigger)
        {
            this.triggerCtl.SetTrigger(trigger);
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.triggerCtl.RuleName))
                {
                    MyMessageBox.ShowMessage("警告", "名称不能为空.");
                    return;
                }
                if (string.IsNullOrEmpty(this.triggerCtl.KPI))
                {
                    MyMessageBox.ShowMessage("警告", "指标不能为空.");
                    return;
                }
                if (15>= this.triggerCtl.Interval)
                {
                    MyMessageBox.ShowMessage("警告", "轮询间隔不能为小于15秒。");
                    return;
                }
                var trigger = this.triggerCtl.GetTrigger();
                var id = trigger.SaveIt();
                var actions = trigger.Children<RedisAction>(null);
                if (null == actions || 0 == actions.Count)
                {
                    var action = new RedisAction();
                    action["name"] = trigger.Name + "-" + "redis";
                    action["command"] = "SET";
                    action["arg0"] = trigger["metric"] + ":" + trigger["parent_id"];
                    action.CreateIt();
                }


                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.ShowMessage("错误", "保存任务失败!", ex.ToString());
            }
        }


        public static void ShowAdd<T>(IEnumerable<T> models, string label, Metric[] metrics, Model model) 
            where T: Model
        {
            SystemManager.OpenForm(new frmTrigger(models.Cast<Model>().ToList(), 
                label, metrics, model), true, false);
        }
        public static void ShowEdit<T>(IEnumerable<T> models, string label, Metric[] metrics,
            Model parent, Trigger trigger) 
            where T : Model
        {
            var frm = new frmTrigger(models.Cast<Model>().ToList(), label, metrics, parent);
            frm.Text = "修改任务";
            frm.ok_button.Text = "修改";
            frm.SetTrigger(trigger);
            SystemManager.OpenForm(frm, true, false);
        }

        public static void ShowAdd<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics, Model parent, Model child) 
            where T1 : Model
            where T2 : class
        {
            frmTrigger2.ShowAdd(models, parentLabel, childLabel, metrics, parent, child);
        }

        public static void ShowEdit<T1, T2>(IDictionary<T1, IList<T2>> models,
            string parentLabel, string childLabel, Metric[] metrics,
            Model parent, Model child, Trigger trigger) 
            where T1 : Model
            where T2 : class
        {
            frmTrigger2.ShowEdit(models, parentLabel, childLabel, metrics,
            parent, child, trigger);
        }
    }
}
