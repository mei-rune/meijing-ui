using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace meijing.template
{
    public interface ITemplateContext : IEnumerable<KeyValuePair<string, object>>
    {
        int Count { get; }

        object this[string key] { get;set; }

        void Add(string key, object value);

        bool Remove(string key);

        bool TryGetValue(string key, out object value);

        bool ContainsKey(string key);
    }

    public class TemplateContext : ITemplateContext
    {
        IDictionary<string, object> _dict;

        public TemplateContext()
            : this(null)
        {
        }

        public TemplateContext(IDictionary<string, object> dict)
        {
            _dict = dict ?? new Dictionary<string, object>();
        }

        #region IRepository<string,object> 成员

        public int Count
        {
            get { return _dict.Count; }
        }

        public object this[string key]
        {
            get
            {
                object obj = null;
                if (_dict.TryGetValue(key, out obj))
                    return obj;
                return null;
            }
            set
            {
                _dict[key] = value;
            }
        }

        public void Add(string key, object value)
        {
            _dict.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> 成员

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        #endregion
    }

    public class TemplateEngine //: ITemplateEngine
    {
        #region ITemplateEngine 成员

        public bool Process(ITemplateContext context, TextWriter writer, TextReader reader, string logTag)
        {
            return Process(context, reader.ReadToEnd(), writer);
        }

        public string Process(ITemplateContext context, string template)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            if (Process(context, template, writer))
                return writer.GetStringBuilder().ToString();
            return template;
        }

        public bool Process(ITemplateContext context, string txt, TextWriter writer)
        {
            int startIndex = 0;
            int index = txt.IndexOf('{', startIndex);
            while (-1 != index)
            {
                if ('{' == txt[index + 1])
                {
                    /// 同时有两个"{{",将startIndex 定位到后一个"{" 的后面
                    index += 2;
                    writer.Write(txt.Substring(startIndex, index - startIndex - 1));
                    startIndex = index;
                }
                else
                {

                    index += 1;
                    int rindex = txt.IndexOf('}', index);
                    if (-1 == rindex)
                        //writer.Write(txt.Substring(startIndex));
                        break;

                    // 将"{"前面的字符保存到writer
                    writer.Write(txt.Substring(startIndex, index - startIndex - 1));

                    ProcessVariant(context, writer, txt.Substring(index, rindex - index));

                    startIndex = ++rindex;
                }

                index = txt.IndexOf('{', startIndex);
            }
            if (0 == startIndex)
                writer.Write(txt);
            else 
                writer.Write(txt.Substring(startIndex));
            return true;
        }


        void ProcessVariant(ITemplateContext context, TextWriter writer, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                writer.Write("{}");
                return;
            }

            string nm = name;
            string fmt = null;
            int index = name.IndexOf(',');
            if (-1 != index)
            {
                fmt = name.Substring(index);
                nm = name.Substring(0, index);
            }
            else if (-1 != (index = name.IndexOf(':')))
            {
                fmt = name.Substring(index);
                nm = name.Substring(0, index);
            }

            object obj = Read(context, nm);
            if (null == obj)
            {
                writer.Write("{");
                writer.Write(name);
                writer.Write("}");
            }
            else
            {
                if (string.IsNullOrEmpty(fmt))
                    writer.Write(obj);
                else
                    writer.Write(string.Concat("{0", fmt, "}"), obj);
            }
        }


        object Read(ITemplateContext context, string nm)
        {
            if (string.IsNullOrEmpty(nm))
                return null;

            object obj = null;
            if (context.TryGetValue(nm, out obj) && null != obj)
                return obj;

            string[] names = nm.Split('.');
            if (1 == names.Length)
                return null;

            obj = context;
            foreach (string name in names)
            {
                obj = Read(obj, name, names);
                if (null == obj)
                    return null;
            }
            return obj;
        }

        object Read(object obj, string name, string[] names)
        {
            ITemplateContext context = obj as ITemplateContext;
            if (null != context)
            {
                if (context.TryGetValue(name, out obj) && null != obj)
                    return obj;
                return null;
            }

            IDictionary<string, object> dict = obj as IDictionary<string, object>;
            if (null != dict)
            {
                if (dict.TryGetValue(name, out obj))
                    return obj;
            }

            System.Collections.IDictionary dict2 = obj as System.Collections.IDictionary;
            if (null != dict2)
            {
                if (dict2.Contains(name))
                    return dict2[name];
            }

            System.Reflection.FieldInfo fi = obj.GetType().GetField(name);
            if (null != fi)
                return fi.GetValue(obj);

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(name);
            if (null != pi)
                return pi.GetValue(obj, null);

            return null;
        }

        #endregion


        public static void Test()
        {
            TemplateContext context = new TemplateContext();
            context["IP"] = "127.0.0.1";
            context["Name"] = "cpu";
            context["aa"] = "cccc";
            context["Date"] = new DateTime(2008, 6, 12, 12, 3, 23);

            DateTime now = DateTime.Now;

            for (int i = 0; i < 100; i++)
            {
                //System.IO.StringReader reader = new System.IO.StringReader();
                //System.IO.StringWriter writer = new System.IO.StringWriter();
                string text = new TemplateEngine().Process(context, "HTTP://{IP}{{IP}{}}}:ADFA/{Name},a{Date.Month}a;{aa}{aaaabb");
                if ("HTTP://127.0.0.1{IP}{}}}:ADFA/cpu,a6a;cccc{aaaabb" != text)
                    throw new ApplicationException();

                text = new TemplateEngine().Process(context, "HTTP://adfafasdf");
                if ("HTTP://adfafasdf" != text)
                    throw new ApplicationException();

                text = new TemplateEngine().Process(context, "HTTP://{IP}adfafasdf");
                if ("HTTP://127.0.0.1adfafasdf" != text)
                    throw new ApplicationException();

            }


        }
    }
}
