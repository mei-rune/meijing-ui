using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meijing.ui.module
{
    class Client
    {
        public string Create(string type, Dictionary<string, Object> attributes)
        {
            throw new NotImplementedException();
        }

        public string Save(string type, Dictionary<string, Object> query, Dictionary<string, string> attributes)
        {
            throw new NotImplementedException();
        }

        public string UpdateById(string type, string id, Dictionary<string, Object> attributes)
        {
            throw new NotImplementedException();
        }

        public string UpdateBy(string type, Dictionary<string, Object> query, Dictionary<string, Object> attributes)
        {
            throw new NotImplementedException();
        }

        public string DeleteById(string type, string id)
        {
            throw new NotImplementedException();
        }

        public string DeleteBy(string type, Dictionary<string, Object> query)
        {
            throw new NotImplementedException();
        }
        public int Count(string type, Dictionary<string, Object> query)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, Object> FindById(string type, string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDictionary<string, Object>> FindBy(string type, Dictionary<string, Object> query)
        {
            throw new NotImplementedException();
        }
    }
}
