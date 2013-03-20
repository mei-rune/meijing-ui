using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace meijing
{
    public class XmlSetting
    {
        XmlNode ConfigNode;

        public XmlSetting(XmlNode node) //, string Filename)
        {
            ConfigNode = node;
        }

        public XmlSetting(System.IO.TextReader reader)
        {
            //this.configFilename =  Filename;
            XmlDocument doc = new XmlDocument();

            doc.Load(reader);
            ConfigNode = doc.DocumentElement;
        }
        public XmlSetting(string xml)
        {

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);
            ConfigNode = doc.DocumentElement;
        }


        public static XmlSetting ReadFromFile(string file)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
            {
                return new XmlSetting(reader);
            }
        }

        public void WriteToFile(string file)
        {
            using (XmlTextWriter writer = new XmlTextWriter(new System.IO.StreamWriter(file)))
            {
                ConfigNode.OwnerDocument.WriteTo(writer);
            }
        }

        public void WriteSetting(string xpath, string value)
        {
            XmlNode node = ConfigNode.SelectSingleNode(xpath);
            if (null == node)
            {
                if (xpath.StartsWith("@"))
                {
                    XmlAttribute el = this.ConfigNode.OwnerDocument.CreateAttribute(xpath.TrimStart('@'));
                    el.Value = value;
                    this.ConfigNode.Attributes.Append(el);
                    return;
                }
                throw new ArgumentException("xpath");
            }


            switch (node.NodeType)
            {
                case XmlNodeType.Attribute:
                    node.Value = value;
                    break;
                case XmlNodeType.Text:
                    XmlElement el = node.ParentNode as XmlElement;
                    el.InnerText = value;
                    break;
                case XmlNodeType.Element:
                    node.InnerText = value;
                    break;
                default:
                    node.Value = value;
                    break;
            }
        }

        public string ReadSetting(string xpath)
        {
            return ReadSetting(xpath, null, true);
        }

        public string ReadSetting(string xpath, string defaultValue)
        {
            return ReadSetting(xpath, defaultValue, false);
        }

        public string ReadInt32(string xpath)
        {
            return ReadSetting(xpath, null, true);
        }

        public int ReadInt32(string xpath, int defaultValue)
        {
            return ReadInt32(xpath, defaultValue, false);
        }

        private int ReadInt32(string xpath, int defaultValue, bool isRequired)
        {
            XmlNode node = ConfigNode.SelectSingleNode(xpath);
            if (node == null)
            {
                if (isRequired)
                    throw new System.Configuration.ConfigurationErrorsException("错误配置: " + xpath);
                else
                    return defaultValue;
            }

            int value = 0;
            if (int.TryParse(node.Value, out value))
                return value;

            if (isRequired)
                throw new System.Configuration.ConfigurationErrorsException("错误配置:格式不正确，不是有效的数字");

            return defaultValue;
        }

        private string ReadSetting(string xpath, string defaultValue, bool isRequired)
        {
            XmlNode node = ConfigNode.SelectSingleNode(xpath);
            if (node == null)
            {
                if (isRequired)
                    throw new System.Configuration.ConfigurationErrorsException("错误配置: " + xpath);
                else
                    return defaultValue;
            }
            else
                return node.Value;
        }

        public XmlSetting[] Select(string xpath)
        {
            return Select(xpath, typeof(XmlSetting));
        }

        public XmlSetting[] Select(string xpath, Type ConfigEntryType)
        {
            XmlNodeList list = ConfigNode.SelectNodes(xpath);
            if (list == null)
                return null;

            ArrayList configList = new ArrayList();
            foreach (XmlNode n in list)
            {
                object o = Activator.CreateInstance(ConfigEntryType, new object[1] { n });
                configList.Add(o);
            }
            return (XmlSetting[])configList.ToArray(ConfigEntryType);
        }

        public XmlSetting SelectOne(string xpath)
        {
            XmlSetting[] entryList = Select(xpath);
            if (entryList == null || entryList.Length == 0)
                return null;
            else
                return entryList[0];

        }

        public XmlSetting AppendChild(string nm)
        {
            XmlElement el = this.ConfigNode.OwnerDocument.CreateElement(nm);
            this.ConfigNode.AppendChild(el);
            return new XmlSetting(el);
        }


        public XmlSetting AppendChild(string nm, string value)
        {
            XmlElement el = this.ConfigNode.OwnerDocument.CreateElement(nm);
            el.Value = value;
            this.ConfigNode.AppendChild(el);
            return new XmlSetting(el);
        }

        public XmlNode AsXmlNode()
        {
            return this.ConfigNode;
        }
    }
}
