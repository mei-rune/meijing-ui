using System;
using System.Collections.Generic;
using System.IO;

namespace meijing
{
    public class Helper
    {

        public static string SearchFile(string name)
        {
            return name;
        }

        public static Dictionary<int, string> portTypes;
        public static String GetPortType(int type)
        {
            if (null == portTypes) {
                var name = SearchFile("meijing.ports.txt");
                portTypes = ReadIniWithIntKeyFromFile(name);
            }

            string descr;
            if (portTypes.TryGetValue(type, out descr)) {
                return descr;
            }
            return type.ToString();
        }

        static IDictionary<string, string> deviceTypes;
        public static IDictionary<string, string> DeviceTypes
        {
            get
            {
                if (null == deviceTypes)
                {
                    deviceTypes = ReadIniFromFile("meijing.deviceTypes.txt");
                }
                return deviceTypes;
            }
        }
        public static string GetDeviceType(string t)
        {
            string res;
            if (DeviceTypes.TryGetValue(t, out res))
            {
                return res;
            }
            return t.ToString();
        }

        static IDictionary<int, string> deviceCatalogs;
        public static IDictionary<int, string> DeviceCatalogs
        {
            get
            {
                if (null == deviceCatalogs)
                {
                    deviceCatalogs = ReadIniWithIntKeyFromFile("meijing.deviceCatalogs.txt");
                }
                return deviceCatalogs;
            }
        }
        public static string GetDeviceCatalog(int t)
        {
            string res;
            if (deviceCatalogs.TryGetValue(t, out res))
            {
                return res;
            }
            return t.ToString();
        }
        static IDictionary<string, string> manufacturers;
        public static IDictionary<string, string> Manufacturers
        {
            get
            {
                if (null == manufacturers)
                {
                    manufacturers = ReadIniFromFile("meijing.manufacturers.txt");
                }
                return manufacturers;
            }
        }

        public static string GetManufacturer(string t)
        {
            string res;
            if (manufacturers.TryGetValue(t, out res))
            {
                return res;
            }
            return t.ToString();
        }

        private const string expressionPrefix = "@every ";
        public static string GetExpressionDescription(string s) {
            if (s.StartsWith(expressionPrefix)) { 
                return "每隔" + GetTimeDescription(s.Substring(expressionPrefix.Length).Trim()) +"调度一次";
            }
            return s;
        }
        public static int ParseExpressionAsSecond(string s)
        {
            if (s.StartsWith(expressionPrefix))
            {
                return GetTimeAsSecond(s);
            }
            return 15;
        }

        public static string CreateExpression(int interval, string unit)
        {
            return expressionPrefix + interval.ToString() + unit;
        }

        public static int GetTimeAsSecond(string s)
        {
            int idx = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (-1 == idx)
            {
                return 15;
            }
            var m = s.Substring(idx);
            if ("m" == m || "M" == m)
            {
                return int.Parse(s.Substring(0, idx)) * 60;
            }
            if ("s" == m || "s" == m)
            {
                return int.Parse(s.Substring(0, idx));
            }
            if ("ms" == m || "ms" == m)
            {
                return int.Parse(s.Substring(0, idx))/60;
            }
            return 15;
        }

        public static string GetTimeDescription(string s)
        {
            int idx = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (-1 == idx)
            {
                return s;
            }
            var m = s.Substring(idx);
            if ("m" == m || "M" == m)
            {
                return s.Substring(0, idx) + "分钟";
            }
            if ("s" == m || "s" == m)
            {
                return s.Substring(0, idx) + "秒";
            }
            if ("ms" == m || "ms" == m)
            {
                return s.Substring(0, idx) + "毫秒";
            }
            return s;
        }


        private static Dictionary<string, string> serviceAddresses = new Dictionary<string, string>();
        public static void SetServerAddress(string addr) 
        {
            serviceAddresses["bridge"] = string.Format("http://{0}:7070", addr);
            serviceAddresses["mdb"] = string.Format("http://{0}:7071/mdb", addr);
            serviceAddresses["poller"] = string.Format("http://{0}:7076", addr);
        }

        public static string GetBridge()
        { 
            return serviceAddresses["bridge"];
        }

        private static Client client;
        public static Client Client 
        {
            get {
                if (null == client) {
                    client = new Client(serviceAddresses["mdb"]);
                }
                return client; 
            }
        }

        static Dictionary<int, string> ReadIniWithIntKeyFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return new Dictionary<int, string>();

            Dictionary<int, string> map = new Dictionary<int, string>();
            foreach (var s in File.ReadAllLines("ports.txt"))
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                int idx = s.IndexOf('=');
                if (-1 == idx)
                    continue;

                int t = 0;
                if (int.TryParse(s.Substring(0, idx), out t))
                {
                    map[t] = s.Substring(idx + 1);
                }
            }
            return map;
        }
        static Dictionary<string, string> ReadIniFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return new Dictionary<string, string>();

            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (string ss in File.ReadAllLines(fileName))
            {
                if (string.IsNullOrEmpty(ss))
                    continue;

                int index = ss.IndexOf('=');
                if (-1 == index)
                    continue;
                string[] keys = ss.Substring(0, index).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                string value = ss.Substring(index + 1).Trim();
                foreach (string key in keys)
                {
                    map[key.Trim()] = ss.Substring(index + 1).Trim();
                }
            }
            return map;
        }


        static void WriteIniToFile(Dictionary<string, string> map, string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            if (null == map)
                return;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (KeyValuePair<string, string> kp in map)
                {
                    writer.Write(kp.Key);
                    writer.Write("=");
                    writer.WriteLine(kp.Value);
                }
            }
        }

    }
}
