using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meijing.ui.module
{
    class UrlBuilder
    {
        readonly StringBuilder sb = new StringBuilder();
        bool is_query = false;

        public UrlBuilder(string baseUrl)
        {
            if ('/' == baseUrl[baseUrl.Length - 1])
            {
                sb.Append(baseUrl.Substring(0, baseUrl.Length - 1));
            }
            else
            {
                sb.Append(baseUrl);
            }

            if (baseUrl.Contains('?'))
            {
                is_query = true;
            }
        }

        public UrlBuilder Concat(params string[] paths)
        {
            if (this.is_query)
            {
                throw new ApplicationException("[panic] don`t append path to the query");
            }

            foreach (var pa in paths)
            {
                if (string.IsNullOrEmpty(pa))
                {
                    continue;
                }

                if ('/' != pa[0])
                {
                    sb.Append("/");
                }

                if ('/' == pa[pa.Length - 1])
                {
                    sb.Append(pa.Substring(0, pa.Length - 1));
                }
                else
                {
                    sb.Append(pa);
                }
            }
            return this;
        }

        public UrlBuilder WithQuery(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return this;
            }
            if (!this.is_query)
            {
                sb.Append("?");
                is_query = true;
            }
            else
            {
                sb.Append("&");
            }
            sb.Append(key);
            sb.Append("=");
            sb.Append(value);
            return this;
        }


        public UrlBuilder WithQueries(IDictionary<string, string> query, string prefix)
        {
            if (null == query || 0 == query.Count)
            {
                return this;
            }

            if (!this.is_query)
            {
                sb.Append("?");
                is_query = true;
            }
            else
            {
                sb.Append("&");
            }
            foreach (var kp in query)
            {
                sb.Append(prefix);
                sb.Append(kp.Key);
                sb.Append("=");
                sb.Append(kp.Value);
                sb.Append("&");
            }
            sb.Remove(sb.Length-1, 1);
            return this;
        }

        public UrlBuilder WithQueries(IDictionary<string, object> query, string prefix)
        {

            if (null == query || 0 == query.Count)
            {
                return this;
            }

            if (!this.is_query)
            {
                sb.Append("?");
                is_query = true;
            }
            else
            {
                sb.Append("&");
            }

            foreach (var kp in query)
            {
                sb.Append(prefix);
                sb.Append(kp.Key);
                sb.Append("=");
                sb.Append(kp.Value.ToString());
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return this;
        }

        public string ToUrl()
        {
            return sb.ToString();
        }
    }
}
