using System;
using System.Collections;
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
        System.Action<IDictionary<string, object>> DiscoveryResult;
        System.Action DiscoveryEnd;
        System.Action<string[]> OnMessage;
        public frmAddDrv()
        {
            InitializeComponent();
            this.DiscoveryEnd = this.OnEnd;
            this.OnMessage = this.OnLogMessages;
            this.DiscoveryResult = this.OnResult;
        }

        private void OnEnd()
        {
            client = null;

            this.progressBar.Visible = false;
            this.add_button.Text = "添加";
        }

        private void OnResult(IDictionary<string, object> result)
        {
            foreach (var kp in result)
            {
                Save(kp.Key, kp.Value as IDictionary<string, object>);
            }
        }

        private string GetDeviceIdIfExists(string address)
        {
            var query = new Dictionary<string, string>();
            query["address"] = address;
            var t = Model.FindBy<IPAddress>(query).FirstOrDefault();
            if (null != t)
            {
                return t.DeviceId;
            }
            var d = Model.FindBy<Device>(query).FirstOrDefault();
            if (null != d)
            {
                return d.Id;
            }
            return null;
        }

        private string GetDeviceIdIfExists(string ip, IDictionary<string, object> drv)
        {
            var id = GetDeviceIdIfExists(ip);
            if (!string.IsNullOrEmpty(id)) 
            {
                return id;
            }
            object addresses;
            drv.TryGetValue("$address", out addresses);
            if (null == addresses)
            {
                return null;
            }
            var list = addresses as IEnumerable;
            if (null == list)
            {
                var map = addresses as IDictionary;
                if (null == map)
                {
                    return null;
                }
                list = map.Keys;
            }
            foreach (var obj in list)
            {
                var map = addresses as IDictionary<string, object>;
                if (null == map)
                    continue;
                object o;
                if (!map.TryGetValue("address", out o))
                {
                    continue;
                }

                id = GetDeviceIdIfExists(o.ToString());
                if (!string.IsNullOrEmpty(id))
                {
                    return id;
                }
            }
            return null;
        }

//func createTriggers(attributes map[string]interface{}) []map[string]interface{} {
//    return []map[string]interface{}{map[string]interface{}{"type": "metric_rule",
//        "expression": "@every 15s",
//        "name":       "cpu-" + fmt.Sprint(attributes["address"]),
//        "metric":     "cpu",
//        "$action":    []interface{}{map[string]interface{}{"type": "redis_action", "name": "redis-cpu-" + fmt.Sprint(attributes["address"]), "command": "SET", "arg0": "cpu:" + fmt.Sprint(attributes["address"])}}},
//        map[string]interface{}{"type": "metric_rule",
//            "expression": "@every 15s",
//            "name":       "mem-" + fmt.Sprint(attributes["address"]),
//            "metric":     "mem",
//            "$action":    []interface{}{map[string]interface{}{"type": "redis_action", "name": "redis-mem-" + fmt.Sprint(attributes["address"]), "command": "SET", "arg0": "mem:" + fmt.Sprint(attributes["address"])}}},
//        map[string]interface{}{"type": "metric_rule",
//            "expression": "@every 15s",
//            "name":       "interface-" + fmt.Sprint(attributes["address"]),
//            "metric":     "interface",
//            "$action":    []interface{}{map[string]interface{}{"type": "redis_action", "name": "redis-interface-" + fmt.Sprint(attributes["address"]), "command": "SET", "arg0": "interface:" + fmt.Sprint(attributes["address"])}}}}
//}

        private void Save(string ip, IDictionary<string, object> attributes)
        {
            try
            {
                var id = GetDeviceIdIfExists(ip, attributes);
                if (!string.IsNullOrEmpty(id))
                {
                    return;
                }
                new Device(attributes).SaveIt();
            }
            catch (Exception e)
            {
                OnLogMessages(new string[]{e.ToString()});
            }
        }

        private void OnLogMessages(string[] messages)
        {
            if (0 != messages.Length)
            {
                this.messageBox.AppendText(string.Join("\r\n", messages));
                this.messageBox.AppendText("\r\n");
            }
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
                    discoveryParams["timeout"] = 20;

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
                                    this.Invoke(this.OnMessage, new object[]{ client.Get().ToArray()});
                                }
                                this.Invoke(this.DiscoveryResult, new object[] { client.GetResult() });
                            }

                        }
                        catch (Exception ex)
                        {
                            this.Invoke(this.OnMessage, new object[] { new string[] { ex.ToString() } });
                        }
                        this.Invoke(this.DiscoveryEnd);

                    }).Start();

                    this.add_button.Text = "停止";
                    this.progressBar.Visible = true;

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
            if (null != client)
            {
                client.Interrupt();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
