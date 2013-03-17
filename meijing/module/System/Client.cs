using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace meijing.ui.module
{
    public class Client
    {
        readonly string baseUrl;
        public Client(string address)
        {
            baseUrl = address;
        }


        public Dictionary<string, object> Invoke(string action, string url, object msg,
            HttpStatusCode exceptedCode)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(url);

            httpWReq.Method = action;
            if (null != msg)
            {
                byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                httpWReq.ContentType = "application/json";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                if (exceptedCode != response.StatusCode)
                {
                    throw new ApplicationException("http error - " + response.StatusDescription);
                }

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);
            }
            catch (WebException e) {
                var body = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                throw new WebException(string.Format("{0}, {1}", e.Message, body), e);
            }
        }


        public string Create(string target, IDictionary<string, object> attributes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target).ToUrl();
            var res = Invoke("POST", url, attributes, HttpStatusCode.Created);
            return res.ToString();
        }

        public string Save(string target, IDictionary<string, string> query,
            IDictionary<string, object> attributes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target).WithQueries(query, "@")
                .WithQuery("save", "true").ToUrl();
            var res = Invoke("POST", url, attributes, HttpStatusCode.OK);
            return res.ToString();
        }

        public string UpdateById(string target, string id, IDictionary<string, object> attributes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, id).ToUrl();
            var res = Invoke("PUT", url, attributes, HttpStatusCode.OK);
            return res.ToString();
        }

        public string UpdateBy(string target, IDictionary<string, string> query,
            IDictionary<string, object> attributes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, "query").WithQueries(query, "@").ToUrl();
            var res = Invoke("PUT", url, attributes, HttpStatusCode.OK);
            return res.ToString();
        }

        public string DeleteById(string target, string id)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, id).ToUrl();
            var res = Invoke("DELETE", url, null, HttpStatusCode.OK);
            return res.ToString();
        }

        public string DeleteBy(string target, IDictionary<string, object> query)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, "query").WithQueries(query, "@").ToUrl();
	        var res = Invoke("DELETE", url, null, HttpStatusCode.OK);
	        return res.ToString();
        }
        public int Count(string type, IDictionary<string, Object> query)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> FindById(string type, string id)
        {
            return FindByIdWithIncludes(type, id, null);
        }

        public IList<IDictionary<string, Object>> FindBy(string type, IDictionary<string, string> query)
        {
            return FindByWithIncludes(type, query, null);
        }

        public IDictionary<string, object> FindByIdWithIncludes(string target, string id, string includes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, id).WithQuery("includes", includes).ToUrl();
            var res = Invoke("GET", url, null, HttpStatusCode.OK);
            return ToDictionary((JObject)res["value"]);
        }

        public IList<IDictionary<string, Object>> FindByWithIncludes(string target, 
            IDictionary<string, string> query, string includes)
        {
            var url = new UrlBuilder(baseUrl).Concat(target, "query").
                WithQueries(query, "@").
                WithQuery("includes", includes).ToUrl();

            var res = Invoke("GET", url, null, HttpStatusCode.OK);
            return ToObjects(res["value"] as IEnumerable<object>);
        }


        public IList<IDictionary<string, Object>> Children(string parent, string parent_id,
            string target, IDictionary<string, string> query)
        {
            var url = new UrlBuilder(baseUrl).Concat(parent, parent_id, "children", target).
                WithQueries(query, "@").ToUrl();

            var res = Invoke("GET", url, null, HttpStatusCode.OK);
            return ToObjects(res["value"] as IEnumerable<object>);
        }

        private static IList<IDictionary<string, object>> ToObjects(IEnumerable<object> obj)
        {
            if (null == obj)
            {
                return null;
            }

            List<IDictionary<string, object>> result = new List<IDictionary<string, object>>();
            foreach (var kp in obj)
            {
                result.Add(ToDictionary((JObject)kp));
            }
            return result;
        }
        private static IDictionary<string, object> ToDictionary(JObject obj) {
            if (null == obj) {
                return null;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var kp in obj) {
                result[kp.Key] = ToObject(kp.Value);
            }
            return result;
        }

        private static IList<object> ToArray(JArray obj)
        {
            if (null == obj)
            {
                return null;
            }

            List<object> result = new List<object>();
            foreach (var kp in obj)
            {
                result.Add(ToObject(kp));
            }
            return result;
        }

        private static object ToObject(JToken token) { 
            var jo = token as JObject;
            if(null != jo) {
               return ToDictionary(jo);
            }

            var ja = token as JArray;
            if (null != ja)
            {
               return ToArray(ja);
            }
            var jv = token as JValue;
            if (null != jv) {
                return jv.Value;
            }
            throw new ApplicationException("unsupport json type - "+ token.GetType().Name);
        }
    }
}
