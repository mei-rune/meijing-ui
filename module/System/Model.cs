using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meijing.ui.module.System
{ 
    
    class Model
    {
        IDictionary<string, object> attributes;

        public Model() {
            this.attributes = new Dictionary<string,  object>();
        }
        public Model(IDictionary<string, object> attributes) {
            this.attributes = attributes;
        }

        public string GetString(string key) {
            var v = this.attributes[key];
            if (null == v) {
                return null;
            }
            return v.ToString();
        }
        
        public int GetInt(string key, int defaultValue) {
            var v = this.attributes[key];
            if (null == v) {
                return defaultValue;
            }
            int i = 0;
            if(int.TryParse(v.ToString(), out i)) {
                return i;
            }
            return defaultValue;
        }
        
        public DateTime GetTimestamp(string key) {
            var v = this.attributes[key];
            if (null == v) {
                return DateTime.MinValue;
            }
            DateTime t;
            if(DateTime.TryParse(v.ToString(), out t)) {
                return t;
            }
            return DateTime.MinValue;
        }

        
        public object this[string key] {
            get { return this.attributes[key]; }
            set { this.attributes[key] = value; }
        }

    }

    class Device : Model
    {
        IDictionary<string, object> attributes;

        public string Name
        {
            get{ return GetString("name"); }
            set{ base["name"] = value; }
        }
        public string Address
        {
            get{ return GetString("address"); }
            set{ base["address"] = value; }
        }
        public string Description
        {
            get{ return GetString("description"); }
            set{ base["description"] = value; }
        }
        public int Catalog
        {
            get{ return GetInt("catalog", -1); }
            set{ base["catalog"] = value; }
        }
        public string Oid
        {
            get{ return GetString("oid"); }
            set{ base["oid"] = value; }
        }
        public int Services
        {
            get{ return GetInt("services", -1); }
            set{ base["services"] = value; }
        }
        public string Location
        {
            get{ return GetString("location"); }
            set{ base["location"] = value; }
        }
        public DateTime CreatedAt
        {
            get{ return GetTimestamp("created_at"); }
            set{ base["created_at"] = value; }
        }
        public DateTime UpdatedAt
        {
            get{ return GetTimestamp("updated_at"); }
            set{ base["updated_at"] = value; }
        }
    }
    public class AccessParams : Model {
        public string Description
        {
            get{ return GetString("description"); }
            set{ base["description"] = value; }
        }
    }
    
    public class EndpointParams: AccessParams {

        public string Address
        {
            get{ return GetString("address"); }
            set{ base["address"] = value; }
        }
        public int Port
        {
            get{ return GetInt("port", -1); }
            set{ base["port"] = value; }
        }
    }
    
    public class SnmpParams: EndpointParams {
        public string Version
        {
            get{ return GetString("version"); }
            set{ base["version"] = value; }
        }
        public string Community
        {
            get{ return GetString("community"); }
            set{ base["community"] = value; }
        }
    }
    public class SshParams: EndpointParams {
        public string User
        {
            get{ return GetString("user"); }
            set{ base["user"] = value; }
        }
        public string Password
        {
            get{ return GetString("password"); }
            set{ base["password"] = value; }
        }
    }
    public class WbemParams: AccessParams {
        public string Url
        {
            get{ return GetString("url"); }
            set{ base["url"] = value; }
        }
        public string User
        {
            get{ return GetString("user"); }
            set{ base["user"] = value; }
        }
        public string Password
        {
            get{ return GetString("password"); }
            set{ base["password"] = value; }
        }
    }
    public class Address : Model {
        public string Address
        {
            get{ return GetString("address"); }
            set{ base["address"] = value; }
        }
        public int IfIndex
        {
            get{ return GetInt("ifIndex", -1); }
            set{ base["ifIndex"] = value; }
        }
        public string Netmask
        {
            get{ return GetString("netmask"); }
            set{ base["netmask"] = value; }
        }
        public string Password
        {
            get{ return GetString("password"); }
            set{ base["password"] = value; }
        }
        public int BcastAddress
        {
            get{ return GetInt("bcastAddress", -1); }
            set{ base["bcastAddress"] = value; }
        }
        public int ReasmMaxSize
        {
            get{ return GetInt("reasmMaxSize", -1); }
            set{ base["reasmMaxSize"] = value; }
        }
    }
    
    public class Interface : Model {
        public string IfDescr
        {
            get{ return GetString("ifDescr"); }
            set{ base["ifDescr"] = value; }
        }
        public int IfIndex
        {
            get{ return GetInt("ifIndex", -1); }
            set{ base["ifIndex"] = value; }
        }
        public int IfType
        {
            get{ return GetInt("ifType", -1); }
            set{ base["ifType"] = value; }
        }
        public int IfMtu
        {
            get{ return GetInt("ifMtu", -1); }
            set{ base["ifMtu"] = value; }
        }
        public int IfSpeed
        {
            get{ return GetInt("ifSpeed", -1); }
            set{ base["ifSpeed"] = value; }
        }
        public string IfPhysAddress
        {
            get{ return GetString("ifPhysAddress"); }
            set{ base["ifPhysAddress"] = value; }
        }
    }
    
    
    public class Trigger : Model {
        public string Name
        {
            get{ return GetString("name"); }
            set{ base["name"] = value; }
        }
        public string Expression
        {
            get{ return GetString("expression"); }
            set{ base["expression"] = value; }
        }
        public string Attachment
        {
            get{ return GetString("attachment"); }
            set{ base["attachment"] = value; }
        }
        public string Description
        {
            get{ return GetString("description"); }
            set{ base["description"] = value; }
        }
    }
    
    public class MetricRule : Trigger {
        public string Metric
        {
            get{ return GetString("metric"); }
            set{ base["metric"] = value; }
        }
    }

    class Action : Model {
        public string Name
        {
            get{ return GetString("name"); }
            set{ base["name"] = value; }
        }
        public string Description
        {
            get{ return GetString("description"); }
            set{ base["description"] = value; }
        }
    }
    
    public class RedisAction : Action {
        public string Command
        {
            get{ return GetString("command"); }
            set{ base["command"] = value; }
        }
        public string Arg0
        {
            get{ return GetString("arg0"); }
            set{ base["arg0"] = value; }
        }
        public string Arg1
        {
            get { return GetString("arg1"); }
            set { base["arg1"] = value; }
        }
        public string Arg2
        {
            get { return GetString("arg2"); }
            set { base["arg2"] = value; }
        }
        public string Arg3
        {
            get { return GetString("arg3"); }
            set { base["arg3"] = value; }
        }
        public string Arg4
        {
            get { return GetString("arg4"); }
            set { base["arg4"] = value; }
        }
    }

}
