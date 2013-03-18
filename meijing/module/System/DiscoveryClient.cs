using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace meijing.ui.module
{

//public class DiscoveryParams {
//    string[] IP_Range     []string `json:"ip-range"`
//    string[] Communities  []string `json:"communities"`
//    string[] SnmpV3Params []string `json:"snmpv3_params"`
//    int Depth        int      `json:"discovery_depth"`
//    bool IsReadLocal  bool     `json:"is_read_local"`
//}


    class DiscoveryClient : Client, IDisposable
    {
        string id;
        bool is_eof = false;

        public DiscoveryClient(string address, IDictionary<string, object> discoveryParams)
            : base(address)
        {
            var res = Invoke("POST", new UrlBuilder(this.baseUrl).Concat("discovery").ToUrl(),
                discoveryParams, HttpStatusCode.Created);
            id = res["value"].ToString();
        }

        public bool IsRunning()
        {
            return !is_eof;
        }

        public void Interrupt() 
        {
            is_eof = true;
        }

        public IEnumerable<string> Get()
        {
            var res = Invoke("GET", new UrlBuilder(this.baseUrl).Concat("discovery", id).
                WithQuery("dst", "message").ToUrl(),
                null, HttpStatusCode.OK);

            object array;
            res.TryGetValue("value", out array);

            if (null == array)
            {
                return null;
            }

            List<string> result = new List<string>();
            foreach (var kp in array as IEnumerable)
            {
                if ("end" == kp.ToString()) {
                    is_eof = true;
                    break;
                }
                result.Add(kp.ToString());
            }
            return result;
        }

        public void Dispose()
        {
            var res = Invoke("DELETE", new UrlBuilder(this.baseUrl).Concat("discovery", id).ToUrl(),
                null, HttpStatusCode.OK);
        }
    }
}
