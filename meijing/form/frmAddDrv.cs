using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace meijing.ui
{
    using meijing.ui.module;

    public partial class frmAddDrv : Form
    {
        public delegate void AddListItem();

        volatile DiscoveryClient client;
        System.Action DiscoveryEnd;
        System.Action<string[]> OnMessage;
        public frmAddDrv()
        {
            InitializeComponent();
            this.DiscoveryEnd = () => { client = null; };
            this.OnMessage = (string[] messages) =>
            { this.messageBox.AppendText(string.Join("\r\n", messages)); };
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            if (null == client)
            {
                try
                {
//                    type DiscoveryParams struct {
//    IP_Range     []string `json:"ip-range"`
//    Communities  []string `json:"communities"`
//    SnmpV3Params []string `json:"snmpv3_params"`
//    Depth        int      `json:"discovery_depth"`
//    IsReadLocal  bool     `json:"is_read_local"`
//}
                    var discoveryParams = new Dictionary<string, object>();
                    discoveryParams["ip-range"] = this.rangeListBox.Lines;
                    discoveryParams["communities"] = this.communityBox.Lines;
                    discoveryParams["is_read_local"] = false;
                    discoveryParams["discovery_depth"] = 0;

                    this.messageBox.Text = "";// Newtonsoft.Json.JsonConvert.SerializeObject(discoveryParams);

                    client = new DiscoveryClient(SystemManager.GetBridge(), discoveryParams);

                    new Thread(() =>
                    {
                        try
                        {
                            using (client)
                            {
                                while (client.IsRunning())
                                {
                                    this.Invoke(this.OnMessage, client.Get());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(this.OnMessage, new string[]{ex.ToString()});
                        }
                        this.Invoke(this.DiscoveryEnd);

                    }).Start();

                    this.add_button.Text = "停止";

                }
                catch (Exception ex)
                {
                    MyMessageBox.ShowMessage("错误", ex.Message, ex.ToString());
                }
            }
            else
            {
                this.add_button.Text = "添加";
                client.Interrupt();
            }
            
        }

        private void frmAddDrv_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
