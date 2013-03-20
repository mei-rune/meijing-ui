using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace meijing
{
    public class StringMap : IEnumerable<KeyValuePair<string, string>>
    {
        KeyValuePair<string, string>[] _list;

        public StringMap(string key, string value)
        {
            _list = new KeyValuePair<string, string>[1];
            _list[0] = new KeyValuePair<string, string>(key, value);
        }

        public StringMap(string[] keys, string[] values)
        {
            if (keys.Length != values.Length)
                throw new ArgumentException("keys");

            _list = new KeyValuePair<string, string>[keys.Length];
            for (int i = 0; i < keys.Length; ++i)
            {
                _list[i] = new KeyValuePair<string, string>(keys[i], values[i]);
            }
        }

        public int Count
        {
            get { return _list.Length; }
        }

        public KeyValuePair<string, string>[] ToArray()
        {
            return _list;
        }

        public KeyValuePair<string, string> this[int i]
        {
            get { return _list[i]; }
        }

        public string this[string key]
        {
            get
            {
                foreach (KeyValuePair<string, string> kp in _list)
                {
                    if (kp.Key == key)
                        return kp.Value;
                }
                return null;
            }
        }

        #region IEnumerable<KeyValuePair<string,string>> 成员

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (KeyValuePair<string, string> kp in _list)
                yield return kp;
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }

    public class MatchResult
    {
        public string Schema;
        public string PKI;

        public StringMap Misc;
    }

    public interface IMatcher
    {
        MatchResult Match(string txt);
    }


    public class SplitMatcher : IMatcher
    {
        public string Name;

        public MatchResult Match(string txt)
        {
            //HostState:Row=0?State
            //OSSwapInfo:Row=0?SwapIn

            int p = txt.IndexOf(':');
            if (-1 == p)
                return null;

            string schema = txt.Substring(0, p);
            txt = txt.Substring(p + 1);

            p = txt.IndexOf('?');
            if (-1 == p)
                return null;

            MatchResult result = new MatchResult();
            result.Schema = schema;
            result.PKI = txt.Substring(p + 1);

            txt = txt.Substring(0, p);
            p = txt.IndexOf('=');
            if (-1 == p)
                result.Misc = new StringMap(txt, null);
            else
                result.Misc = new StringMap(txt.Substring(0, p), txt.Substring(p + 1).Trim('"'));
            return result;
        }
    }

    public class RegexMatcher
    {
        public string Name;
        public Regex Rx;
        public string Separator;

        public MatchResult Match(string txt)
        {
            return null;
            //    Match m = Rx.Match(txt);

            //    if (!m.Success)
            //        return null;

            //    if (3 > m.Groups.Count)
            //        return null;

            //    return new string[]{ m.Groups[1].Captures[0].Value
            //        , m.Groups[2].Captures[0].Value};
        }
    }

    public class Context : IEnumerable<KeyValuePair<string, string>>
    {
        Dictionary<string, string> values = new Dictionary<string, string>();

        public int Count
        {
            get { return values.Count; }
        }

        public string this[string key]
        {
            get { return Get(key); }
            set { values[key] = value; }
        }

        public string Get(string key)
        {
            string value = null;
            if (values.TryGetValue(key, out value))
                return value;
            return null;
        }

        public string Put(string key, string value)
        {
            string old = null;
            values.TryGetValue(key, out old);
            values[key] = value;
            return old;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }


    public class DispatchResult
    {
        public string Key;
        public string Value;
        public string Alias;
        public string Separator;
        public string Dynamic;
    }

    public class Dispatcher
    {
        string _key;
        public Dictionary<string, Dispatcher> Childs = new Dictionary<string, Dispatcher>();
        public virtual string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public virtual DispatchResult Find(Context context)
        {
            if (string.IsNullOrEmpty(Key))
                return null;

            string va = context[Key];

            Dispatcher dispatcher = null;
            if (Childs.TryGetValue(va, out dispatcher))
                return dispatcher.Find(context);
            return null;
        }
    }


    public class DispatchImpl : Dispatcher
    {
        public DispatchResult Result = new DispatchResult();

        public override string Key
        {
            get { return Result.Key; }
            set { base.Key = value; }
        }

        public override DispatchResult Find(Context context)
        {
            DispatchResult result = base.Find(context);
            return result ?? Result;
        }


        public static void ReadDispatch(XmlSetting xmlSetting, Dispatcher dispatcher, log4net.ILog logger)
        {
            dispatcher.Key = xmlSetting.ReadSetting("@key", null);
            if (string.IsNullOrEmpty(dispatcher.Key))
                return;
            foreach (XmlSetting child in xmlSetting.Select("*"))
            {
                DispatchImpl ch = ReadDispatch(child, 1, logger);
                if (null == ch)
                    continue;
                dispatcher.Childs[ch.Result.Value] = ch;
            }
        }

        static DispatchImpl ReadDispatch(XmlSetting xmlSetting, int level, log4net.ILog logger)
        {
            if (null == xmlSetting)
                return null;

            string value = xmlSetting.ReadSetting("@value", null);
            string alias = xmlSetting.ReadSetting("@alias", null);
            string key = xmlSetting.ReadSetting("@key", null);
            if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(key))
                return null;

            DispatchImpl impl = new DispatchImpl();
            impl.Result.Key = key;
            impl.Result.Value = value;
            impl.Result.Alias = alias;
            impl.Result.Separator = xmlSetting.ReadSetting("@separator", null);
            impl.Result.Dynamic = xmlSetting.ReadSetting("@dynamic", null);

            logger.InfoFormat("{0} key={1},value={2},alias={3},separator={4},dynamic={5}"
                , new string(' ', level)
                , impl.Result.Key
                , impl.Result.Value
                , impl.Result.Alias
                , impl.Result.Separator
                , impl.Result.Dynamic);
            if (string.IsNullOrEmpty(key))
                return impl;

            foreach (XmlSetting child in xmlSetting.Select("*"))
            {
                DispatchImpl ch = ReadDispatch(child, ++level, logger);
                if (null == ch)
                    continue;

                impl.Childs[ch.Result.Value] = ch;
            }
            return impl;
        }
    }
}
