using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meijing.ui.module
{ 
    class NameAttribute : Attribute 
    {
        private string value;

        public NameAttribute(string name) {
            this.value = name;
        }
        
        public string Value 
        {
            get {return value;} 
        }

    }
    
    class TableAttribute : Attribute 
    {
        private string value;

        public TableAttribute (string name) {
            this.value = name;
        }
        
        public string Value 
        {
            get {return value;} 
        }
    }
    
    public class Model
    {
        IDictionary<string, object> attributes;

        public Model() {
            this.attributes = new Dictionary<string,  object>();
        }
        public Model(IDictionary<string, object> attributes) {
            this.attributes = attributes;
        }

        public string Id
        {
            get { return GetString("_id");  }
            set { this.attributes["_id"] = value;  }
        }

        public string GetString(string key) {
            object v;
            this.attributes.TryGetValue(key, out v);
            if (null == v) {
                return null;
            }
            return v.ToString();
        }

        public int GetInt(string key, int defaultValue)
        {
            object v;
            this.attributes.TryGetValue(key, out v);
            if (null == v) {
                return defaultValue;
            }
            int i = 0;
            if(int.TryParse(v.ToString(), out i)) {
                return i;
            }
            return defaultValue;
        }

        public DateTime GetTimestamp(string key)
        {
            object v;
            this.attributes.TryGetValue(key, out v);
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

        public string GetClassName()
        {
            return Model.GetClassName(this.GetType());
        }

        public string GetTableName()
        {
            return Model.GetTableName(this.GetType());
        }

        public override int GetHashCode()
        {
            if (null == this.Id)
            {
                return -1;
            }
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            if (null == this.Id)
            {
                return "";
            }
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            var model = obj as Model;
            if (null == model) {
                return false;
            }

            return Id.Equals(model.Id);
        }

        public string SaveIt()
        {
            if (null == this.Id)
            {
                return CreateIt();
            }
            else
            {
                UpdateIt();
                return this.Id;
            }
        }

        public string CreateIt()
        {
            this.attributes["_id"] = SystemManager.Client.Create(GetClassName(this.GetType()),
                this.attributes);
            return this.Id;
        }

        public void UpdateIt()
        {
            SystemManager.Client.UpdateById(GetClassName(this.GetType()), this.Id, this.attributes);
        }

        public void DeleteIt()
        {
            SystemManager.Client.DeleteById(GetClassName(this.GetType()), this.Id);
        }

        public IList<T> Children<T>(IDictionary<string, string> query)
            where T : Model, new()
        {
            List<T> result = new List<T>();
            var qResult = SystemManager.Client.Children(GetClassName(this.GetType()), this.Id, 
                GetClassName(typeof(T)), query);
            foreach (var res in qResult)
            {
                var instance = new T();
                foreach (var k in res)
                {
                    instance[k.Key] = k.Value;
                }
                result.Add(instance);
            }
            return result;
        }

        private static string GetClassName(Type t) {
            var names = t.GetCustomAttributes(typeof(NameAttribute), true);
            if (null == names || 0 == names.Length) {
                throw new ApplicationException("class name is not defined.");
            }
            return ((NameAttribute)names[0]).Value;
        }

        private static string GetTableName(Type t) {
            var names = t.GetCustomAttributes(typeof(NameAttribute), true);
            if (null == names || 0 == names.Length) {
                throw new ApplicationException("table name is not defined.");
            }
            return ((NameAttribute)names[0]).Value;
        }

        public static string Create<T>(Dictionary<string, Object> attributes)
            where T : Model
        {
            return SystemManager.Client.Create(GetClassName(typeof(T)), attributes);
        }

        public static string Save<T>(IDictionary<string, string> query, IDictionary<string, object> attributes)
            where T : Model
        {
            return SystemManager.Client.Save(GetClassName(typeof(T)), query, attributes);
        }

        public static void UpdateById<T>(string id, IDictionary<string, Object> attributes)
            where T : Model
        {
            SystemManager.Client.UpdateById(GetClassName(typeof(T)), id, attributes);
        }

        public static void UpdateBy<T>(IDictionary<string, string> query, 
            IDictionary<string, object> attributes)
            where T : Model
        {
            SystemManager.Client.UpdateBy(GetClassName(typeof(T)), query, attributes);
        }

        public static void DeleteById<T>(string id)
            where T : Model
        {
            SystemManager.Client.DeleteById(GetClassName(typeof(T)), id);
        }

        public static void DeleteBy<T>(IDictionary<string, string> query)
            where T : Model
        {
            SystemManager.Client.DeleteBy(GetClassName(typeof(T)), query);
        }

        public static int Count<T>(IDictionary<string, string> query)
            where T : Model
        {
            return SystemManager.Client.Count(GetClassName(typeof(T)), query);
        }

        public static T FindById<T>(string id) 
            where T : Model, new()
        {
            var instance = new T();
            foreach(var k in SystemManager.Client.FindById(GetClassName(typeof(T)), id)){
                instance[k.Key] = k.Value;
            }
            return instance;
        }

        public static IList<T> FindBy<T>( IDictionary<string, string> query)
            where T : Model, new()
        {
            List<T> result = new List<T>();
            var qResult = SystemManager.Client.FindBy(GetClassName(typeof(T)), query);
            foreach (var res  in qResult)
            {
                var instance = new T();
                foreach (var k in res)
                {
                    instance[k.Key] = k.Value;
                }
                result.Add(instance);
            }
            return result;
        }
    }

    [Name("device")]
    public class Device : Model
    {
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

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name)?(string.IsNullOrEmpty(Description)?Id:Description):Name;
        }
    }

    [Name("link")]
    public class Link : Model
    {
        public string Name
        {
            get { return GetString("name"); }
            set { base["name"] = value; }
        }
        public string Description
        {
            get { return GetString("description"); }
            set { base["description"] = value; }
        }
        public string Device1Id
        {
            get { return GetString("device1"); }
            set { base["device1"] = value; }
        }
        public int IfIndex1
        {
            get { return GetInt("ifIndex1", -1); }
            set { base["ifIndex1"] = value; }
        }
        public string Device2Id
        {
            get { return GetString("device2"); }
            set { base["device2"] = value; }
        }
        public int IfIndex2
        {
            get { return GetInt("ifIndex2", -1); }
            set { base["ifIndex2"] = value; }
        }
        public int SamplingDirect
        {
            get { return GetInt("sampling_direct", -1); }
            set { base["sampling_direct"] = value; }
        }
        public override string ToString()
        {
            if(!string.IsNullOrEmpty(Name)) 
            {
                return this.Name;
            }

            return string.Format("[{0}]{1}-[{2}]{3}", this.Device1Id, this.IfIndex1,
                this.Device2Id, this.IfIndex2);
        }
    }

    [Name("access_params")]
    public class AccessParams : Model {
        public string Description
        {
            get{ return GetString("description"); }
            set{ base["description"] = value; }
        }
    }

    [Name("endpoint_params")]
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

    [Name("snmp_params")]
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

    [Name("ssh_params")]
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

    [Name("wbem_params")]
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

    [Name("address")]
    public class IPAddress : Model {

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

    [Name("interface")]
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

        public override string ToString()
        {
            return string.Format("[{0}{1}]", this.IfIndex, this.IfDescr);
        }
    }

    [Name("trigger")]
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
        public string Type
        {
            get { return GetString("type"); }
            set { base["type"] = value; }
        }
    }

    [Name("metric_rule")]
    public class MetricRule : Trigger {
        public string Metric
        {
            get{ return GetString("metric"); }
            set{ base["metric"] = value; }
        }
    }

    [Name("action")]
    public class Action : Model {
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

    [Name("redis_action")]
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
