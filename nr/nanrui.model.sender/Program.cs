using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Threading;
using System.Data;


namespace meijing.nanrui.model.sender
{
    using Apache.NMS;
    using Apache.NMS.Util;


    internal class XmlContainer
    {
        public string Corporation;
        public XmlDocument Doc;
        public XmlElement imsData;
        public Dictionary<string, XmlElement> xmlNodes = new Dictionary<string,XmlElement>();
    }

    partial class Program
    {
        static log4net.ILog logger;
        static StreamWriter linkLogger;

        //static IManagementRepository repository;
        //static ITopologyManager _topMgr;
        //static IDaoFactory factory;
        static XmlSetting defaultSetting;
        static string profix = "";
        static string Producer = "MEIJING";
        static string basePath;
        static string cachePath;
        static int sequeueId;
        static int maxSize = 4 * 1024 * 1024;
        static string corporation = "xxxx 公司";
        static bool uploadForPCAgent=false;
        static bool uploadForBadLink=false;
        static System.Net.IPAddress[] addresses;


        static Dispatcher _deviceDispatcher = new Dispatcher();
        static Dispatcher _serviceDispatcher = new Dispatcher();
        static Dictionary<string, Device> _deviceByIds = new Dictionary<string, Device>();
        static Dictionary<string, Link> _linkByIds = new Dictionary<string, Link>();
        //static Dictionary<int, ServiceStaticPropertiesData> _serviceByIds = new Dictionary<int, ServiceStaticPropertiesData>();

        static Dictionary<string, string> _deviceModels = new Dictionary<string, string>();
        static Dictionary<string, string> _deviceFields = new Dictionary<string, string>();
        static List<KeyValuePair<IPSeg, string>> _corporationIPSeg = new List<KeyValuePair<IPSeg, string>>();


        //static Dictionary<int, OSFileSystem[]> _fileSystems = new Dictionary<int, OSFileSystem[]>();
        //static Dictionary<int, OSCPU[]> _OSCPUs = new Dictionary<int, OSCPU[]>();
        //static Dictionary<int, PVInfo[]> _pvInfos = new Dictionary<int, PVInfo[]>();
        //static Dictionary<int, OSDiskUtil[]> _diskUtils = new Dictionary<int, OSDiskUtil[]>();
        //static Dictionary<int, TableSpace[]> _tableSpaces = new Dictionary<int, TableSpace[]>();
        static Dictionary<int, string> _versions = new Dictionary<int, string>();
        static bool supportMultCPU = false;


        static void ReadIni(List<KeyValuePair<IPSeg, string>> corporationIPSeg, XmlSetting[] entries)
        {
            if (null == entries)
                return;

            foreach (XmlSetting driver in entries)
            {
                string from = driver.ReadSetting("@from", null);
                string to = driver.ReadSetting("@to", null);
                string value = driver.ReadSetting("@value", null);
                IPSeg seg = null;
                try
                {
                    seg = new IPSeg(from, to);
                    corporationIPSeg.Add(new KeyValuePair<IPSeg,string>(seg, value));
                    logger.InfoFormat("{0}={1}", seg, value);
                }
                catch
                { 
                }
            }
        }

        static Dictionary<string, string> ReadIni(string fileName)
        {
            if (!File.Exists(fileName))
                return new Dictionary<string,string>();

            Dictionary<string,string> map = new Dictionary<string,string>();
            foreach (string ss in File.ReadAllLines(fileName))
            {
                if (string.IsNullOrEmpty(ss))
                    continue;

                int index = ss.IndexOf('=');
                if (-1 == index)
                    continue;
                string[] keys = ss.Substring(0, index).Split(new string[]{"||"},  StringSplitOptions.RemoveEmptyEntries);
                string value = ss.Substring(index + 1).Trim();
                foreach (string key in keys)
                {
                    map[key.Trim()] = ss.Substring(index + 1).Trim();
                }
            }
            return map;
        }

        static Dictionary<int, bool[]> ReadIniFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            Dictionary<int, bool[]> map = new Dictionary<int, bool[]>();
            foreach (string ss in File.ReadAllLines(fileName))
            {
                if (string.IsNullOrEmpty(ss))
                    continue;

                int index = ss.IndexOf('=');
                if (-1 == index)
                    continue;

                int id = 0;
                if (!int.TryParse(ss.Substring(0, index), out id))
                    continue;
                string val = ss.Substring(index + 1);
                if (string.IsNullOrEmpty(val))
                    continue;
                string[] sa = val.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (2 == sa.Length)
                    map[id] = new bool[] { "true" == sa[0], "true" == sa[1] };
            }
            return map;
        }

        static void ReadMap(Dictionary<string, string> map, XmlSetting[] entries)
        {
            if (null == entries)
                return;

            foreach (XmlSetting driver in entries)
            {
                string key = driver.ReadSetting("@key", null);
                string value = driver.ReadSetting("@value", null);
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    continue;
                map[key] = value;

                logger.InfoFormat("{0}={1}", key, value);
            }
        }

        static XmlContainer createXml(string co, IDictionary<string, XmlContainer> dict)
        {
            if (null == co)
                co = corporation;
            co = co.Trim();
            if (0 == co.Length)
                co = corporation;

            XmlContainer xmlContainer = null;
            if (dict.TryGetValue(co, out xmlContainer))
                return xmlContainer;


            logger.InfoFormat("创建源为{0}的xml文件!", co);

            xmlContainer = new XmlContainer();
            xmlContainer.Corporation = co;
            xmlContainer.Doc = new System.Xml.XmlDocument();

            string xmlText = "<?xml version=\"1.0\" encoding=\"GB2312\"?>"
                        + "<!DOCTYPE NARI_IMS_IMPORT_DATA PUBLIC \"-//nari_ims_import_data DTD 3.0//EN\" \"nari_ims_import_data.dtd\">"
                        + "<IMSDATA>"
                        + "<datasource name=\"NARI Company IMS System's Import Data\" createtime=\"" + DateTime.Now.ToShortDateString() + "\" vendorname=\"Betasoft\" productname=\"" + Producer + "\" corporation=\"" + co + "\">"
                        + "</datasource>"
                        + "<datainfo OUTPUTIP=\"" + addresses[0].ToString() + "|\" BEGINTIME=\"" + DateTime.Now.ToShortDateString() + "\">"
                        + "</datainfo>"
                        + "</IMSDATA>";

            xmlContainer.Doc.LoadXml(xmlText);

            xmlContainer.imsData = xmlContainer.Doc.SelectSingleNode("IMSDATA") as XmlElement;

            dict[co] = xmlContainer;
            return xmlContainer;
        }

        static void clearXml(XmlContainer xmlContainer, IDictionary<string, XmlContainer> dict)
        {
            if (null == xmlContainer)
                return;


            XmlNode node = xmlContainer.Doc.SelectSingleNode("/IMSDATA/datas[@classname=\"RelationShip\"]");
            if (null != node)
            {
                XmlNode parentNode = node.ParentNode;
                parentNode.RemoveChild(node);
                parentNode.AppendChild(node);
            }

            string txt = xmlContainer.Doc.OuterXml;
            if (txt.Length < maxSize)
                return;


            logger.InfoFormat("数据长度大于{0},发送消息!", maxSize);

            Send(xmlContainer.Doc, txt);

            logger.Info("发送消息成功,清除数据!");

            dict.Remove(xmlContainer.Corporation);
        }

        static void Main(string[] args)
        {
            try
            {
                basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                addresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());

                string url = "/Btnm/LocalPlatform";
                string addr = "tcp://127.0.0.1:7070";


                foreach (string ss in args)
                {
                    if (ss.StartsWith("--Url=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        url = ss.Substring("--Url=".Length);
                    }
                    else if (ss.StartsWith("--Address=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        addr = ss.Substring(10);
                    }
                    else if (ss.StartsWith("--MaxSize=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        maxSize = int.Parse(ss.Substring(10));
                    }
                }


                XmlSetting xmlSetting = new XmlSetting(new StreamReader(Path.Combine(basePath, "nanrui.modul.config")));
                defaultSetting = XmlSetting.ReadFromFile(Path.Combine(basePath, "../nanrui.default.config"));

                log4net.Config.XmlConfigurator.Configure(xmlSetting.SelectOne("/configuration/log4net").AsXmlNode() as System.Xml.XmlElement);
                logger = log4net.LogManager.GetLogger("Betanetworks");

                try
                {
                    cachePath = Path.Combine(basePath, "cache");
                    profix = defaultSetting.ReadSetting("/configuration/Locale/PrefixId/@value", profix);
                    corporation = defaultSetting.ReadSetting("/configuration/Locale/Corporation/@value", corporation);
                    uploadForPCAgent = "true" == xmlSetting.ReadSetting("/configuration/Locale/UploadForPCAgent/@value", "false");
                    uploadForBadLink = "true" == xmlSetting.ReadSetting("/configuration/Locale/UploadForBadLink/@value", "false");
                    supportMultCPU = "true" == xmlSetting.ReadSetting("/configuration/Locale/SupportMultCPU/@value", "false");
                    logger.InfoFormat("配置发送程序启动,缺省前缀 '{0}',缺省公司 '{1}'", profix, corporation);

                    logger.Info("载入设备指标映射!");
                    DispatchImpl.ReadDispatch(xmlSetting.SelectOne("/configuration/Maps/Device"), _deviceDispatcher, logger);
                    
                    logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入设备类型列表!", null);
                    _deviceModels = ReadIni(Path.Combine(basePath, "../betasoft.deviceModels.txt"));
                    logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入要发送的设备备注列表!", null);
                    ReadMap(_deviceFields, xmlSetting.Select("/configuration/Locale/DeviceFields/*"));
                    
                    logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入設備的公司列表!", null);
                    ReadIni(_corporationIPSeg, xmlSetting.Select("/configuration/Locale/Corporations/*"));
                    
    //<Corporations>
    //  <IPAddress from="" to="" value="" />
    //</Corporations>

                    

                    Dictionary<string, XmlContainer> dict = new Dictionary<string, XmlContainer>();

                    logger.Info("开始读设备表!");

                        IList<Device> devices = Device.FindBy<Device>(null);

                        logger.Info("读设备表完成,开始处理!");


                        foreach (Device dev in devices)
                        {
                            XmlContainer xmlContainer = null;
                            switch (dev.Catalog)
                            {
                                case 1: //交换机
                                    logger.InfoFormat("开始处理交换机{0}:{1}:{2}!", dev.Id, dev.Address, dev);
                                    xmlContainer = processSW(dev, dict);
                                    break;
                                case 2://路由器
                                    logger.InfoFormat("开始处理路由器{0}:{1}:{2}!", dev.Id, dev.Address, dev);
                                    xmlContainer = processRT(dev, dict);
                                    break;
                                case 3://交换路由器
                                    logger.InfoFormat("开始处理交换路由器{0}:{1}:{2}!", dev.Id, dev.Address, dev);
                                    xmlContainer = processSW(dev, dict);
                                    break;
                                default:
                                    logger.InfoFormat("跳过未知设备{0}:{1}:{2}!", dev.Id, dev.Address, dev);
                                    break;
                            }

                            if (null != xmlContainer)
                            {
                                _deviceByIds[dev.Id] = dev;
                                clearXml(xmlContainer, dict);
                            }

                        }

                        logger.Info("处理设备表完成,开始读线路表!");

                        using (linkLogger = new StreamWriter(Path.Combine(basePath, string.Concat("nanrui.modul.link.txt"))))
                        {
                            IList<Link> links = Link.FindBy<Link>(null);
                            logger.Info("读线路表完成,开始处理!");

                            foreach (Link link in links)
                            {
                                logger.InfoFormat("开始处理线路{0}:{1}!", link.Id, link);

                                XmlContainer xmlContainer = processLink(link, dict);

                                clearXml(xmlContainer, dict);


                                if (null != xmlContainer)
                                {
                                    _linkByIds[link.Id] = link;
                                    clearXml(xmlContainer, dict);
                                }
                            }
                        }
                        linkLogger = null;

                        logger.Info("处理线路表完成, 开始发送数据!");

                        foreach (XmlContainer xmlContainer in dict.Values)
                        {

                            XmlNode node = xmlContainer.Doc.SelectSingleNode("/IMSDATA/datas[@classname=\"RelationShip\"]");
                            if (null != node)
                            {
                                XmlNode parentNode = node.ParentNode;
                                parentNode.RemoveChild(node);
                                parentNode.AppendChild(node);
                            }

                            Send(xmlContainer.Doc, xmlContainer.Doc.OuterXml);
                        }
                    
                        logger.Info("数据处理完成,配置发送程序退出!");
                }
                catch (Exception e)
                {
                    logger.Fatal("发生异常!", e);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void WriteFile(IEnumerable<int> idList, string nm)
        {
            if (File.Exists(nm + ".tmp"))
                File.Delete(nm + ".tmp");

            using (StreamWriter writer = new StreamWriter(nm + ".tmp"))
            {
                foreach (int id in idList)
                {
                    writer.WriteLine(id);
                }
            }

            if (File.Exists(nm))
                File.Delete(nm);

            File.Move(nm + ".tmp", nm);
        }

        static void Send(XmlDocument doc, string txt)
        {
            try
            {
                //XmlSetting xmlSetting = new XmlSetting(new StreamReader(Path.Combine(basePath, "nanrui.modul.config")));

                using (Session session = NMSSupport.Create(defaultSetting))
                {
                    //IDestination destination = Apache.NMS.Util.SessionUtil.GetDestination(session, xmlSetting.ReadSetting("/configuration/MQ/DestinationName/@value"));
                    //using (IMessageProducer producer = session.CreateProducer(destination))
                    {
                        //TextMessage msg = producer.CreateTextMessage();
                        //msg.Text = txt;
                        session.Send(Destination.MODEL, txt);

                        Session.Error[] errors = session.GetLastErrors();
                        if (null != errors && 0 != errors.Length)
                        {
                            logger.WarnFormat("主消息队列发送成功,但还有辅消息队列发送失败!", errors.Length );

                            foreach (Session.Error err in errors)
                            {
                                logger.Warn(string.Concat("发送配置到 ", err.Name, "时发生异常!"), err.exception);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("发送配置发生异常!", e);
            }
            doc.Save(Path.Combine(basePath, string.Concat("nanrui.modul", ++sequeueId, ".xml")));
        }

        static XmlElement AppendChild(XmlElement element, string name, object value)
        {
            XmlElement node = element.OwnerDocument.CreateElement(name);
            element.AppendChild(node);
            node.InnerText = value.ToString();
            return node;
        }

        static XmlElement AppendChild(XmlElement element, string name)
        {
            XmlElement node = element.OwnerDocument.CreateElement(name);
            element.AppendChild(node);
            return node;
        }

        static XmlElement FindAndAppendChild(XmlElement element, string name)
        {
            foreach (XmlNode child in element.ChildNodes)
            {
                if (child.Name == name)
                    return child as XmlElement;
            }
            XmlElement node = element.OwnerDocument.CreateElement(name);
            element.AppendChild(node);
            return node;
        }

        static string generateOriginalKey(Device dev)
        {
            return string.Concat(profix, "Device-", dev.Id);
        }

        //static string generateOriginalKey(BusinessSystem businessSystem)
        //{
        //    return string.Concat(profix, "BusinessSystem-", businessSystem.Id);
        //}
        
        //static string generateOriginalKey(ServiceStaticPropertiesData service)
        //{
        //    return string.Concat(profix, "SERVER-", service.Id);
        //}

        //static XmlContainer processService( BTSystem btSystem
        //    , ServiceStaticPropertiesData service
        //    , IDictionary<string, XmlContainer> xmlNodes)
        //{

        //    if (null == service.Device)
        //    {
        //        logger.DebugFormat("跳过{0}服务[{1}:{2}] - 找不到所依赖的设备{3}"
        //            , service.TypeName, service.Id, service.CustomName, service.DeviceId);
        //        return null;
        //    }

        //    switch (service.Type)
        //    {
        //        case 1://IIS-WEB
        //            logger.InfoFormat("开始处理IIS-WEB-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForIISWEB(btSystem, service, xmlNodes);
        //        case 2://IIS-FTP
        //            logger.InfoFormat("开始处理IIS-FTP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForIISFTP(btSystem, service, xmlNodes);
        //        case 3://Oracle
        //            logger.InfoFormat("开始处理Oracle-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForOracle(btSystem, service, xmlNodes);
        //        case 4://Apache-WEB
        //            logger.InfoFormat("开始处理Apache-WEB-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForApache(btSystem, service, xmlNodes);
        //        case 5://TELNET
        //            logger.InfoFormat("开始处理TELNET-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForTELNET(btSystem, service, xmlNodes);
        //        case 6://SQLServer
        //            logger.InfoFormat("开始处理SQLServer-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForSQLServer(btSystem, service, xmlNodes);
        //        case 7://SMTP
        //            logger.InfoFormat("开始处理SMTP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForSMTP(btSystem, service, xmlNodes);
        //        case 8://POP3
        //            logger.InfoFormat("开始处理POP3-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForPOP3(btSystem, service, xmlNodes);
        //        case 9://WEBLogic
        //            logger.InfoFormat("开始处理WEBLogic-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForWEBLogic(btSystem, service, xmlNodes);
        //        case 10://WebSphere
        //            logger.InfoFormat("开始处理WebSphere-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForWebSphere(btSystem, service, xmlNodes);
        //        case 11://Domino
        //            logger.InfoFormat("开始处理Domino-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForDomino(btSystem, service, xmlNodes);
        //        case 12://Sybase
        //            logger.InfoFormat("开始处理Sybase-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForSybase(btSystem, service, xmlNodes);
        //        case 13://Sap
        //            logger.InfoFormat("开始处理Sap-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForSap(btSystem, service, xmlNodes);
        //        case 14://HTTP
        //            logger.InfoFormat("开始处理HTTP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForHTTP(btSystem, service, xmlNodes);
        //        case 15://DNS
        //            logger.InfoFormat("开始处理DNS-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForDNS(btSystem, service, xmlNodes);
        //        case 16://Windows-FTP
        //            logger.InfoFormat("开始处理Windows-FTP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForWindowsFTP(btSystem, service, xmlNodes);
        //        case 17://DHCP
        //            logger.InfoFormat("开始处理DHCP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForDHCP(btSystem, service, xmlNodes);
        //        case 18://LDAP
        //            logger.InfoFormat("开始处理LDAP-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForLDAP(btSystem, service, xmlNodes);
        //        case 19://MySql
        //            logger.InfoFormat("开始处理MySql-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForMySql(btSystem, service, xmlNodes);
        //        case 20://Tuxedo
        //            logger.InfoFormat("开始处理Tuxedo-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForTuxedo(btSystem, service, xmlNodes);
        //        case 21://Informix
        //            logger.InfoFormat("开始处理Informix-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForInformix(btSystem, service, xmlNodes);
        //        case 22://DB2
        //            logger.InfoFormat("开始处理DB2-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForDB2(btSystem, service, xmlNodes);
        //        case 23://EAServer
        //            logger.InfoFormat("开始处理EAServer-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForEAServer(btSystem, service, xmlNodes);
        //        case 24://MQ
        //            logger.InfoFormat("开始处理MQ-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForMQ(btSystem, service, xmlNodes);
        //        case 25://SybaseIQ
        //            logger.InfoFormat("开始处理SybaseIQ-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForSybaseIQ(btSystem, service, xmlNodes);
        //        case 26://Cognos
        //            logger.InfoFormat("开始处理Cognos-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForCognos(btSystem, service, xmlNodes);
        //        case 27://Informatica
        //            logger.InfoFormat("开始处理Informatica-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForInformatica(btSystem, service, xmlNodes);
        //        case 28://Oracle_RAC
        //            logger.InfoFormat("开始处理Oracle_RAC-{0}-{1}:{2}!", service.TypeName, service.Id, service.CustomName);
        //            return processServiceForOracle_RAC(btSystem, service, xmlNodes);       
        //        default:
        //            return null;
        //    }
        //}

        static XmlElement createXmlElement(string nodeName, XmlContainer xmlContainer)
        {
            XmlElement instances = null;
            if (xmlContainer.xmlNodes.TryGetValue(nodeName, out instances))
                return instances;

            instances = AppendChild(xmlContainer.imsData, "datas");
            instances.SetAttribute("classname", nodeName);
            xmlContainer.xmlNodes[nodeName] = instances;
            return instances;
        }

        static XmlContainer processRT(Device dev, IDictionary<string, XmlContainer> dict)
        {
            XmlContainer xmlContainer = createXml(GetAddress(dev.Address, dev.Address), dict);
            XmlElement rt = AppendChild(createXmlElement("Router", xmlContainer), "Router");
            string originalKey = generateOriginalKey(dev);
            AppendChild(rt, "OriginalKey", originalKey);
            //AppendChild(rt, "OriginalKey", string.Concat(profix, "Router-", dev.DeviceId));
            AppendChild(rt, "Name", dev.GetName());
            AppendChild(rt, "Description", dev.Description);
            AppendChild(rt, "DevLevel", dev.Level);
            AppendChild(rt, "Manufacturer", dev.Manufacturer);
            AppendChild(rt, "Architecture", dev.SysTypeName);
            AppendChild(rt, "ROMVersion", dev.SystemOID);
            AppendChild(rt, "OSInfo", dev.SysTypeName);

            string model = null;
            if (_deviceModels.TryGetValue(dev.SystemOID.Trim(new char[]{'.'}), out model)
                && !string.IsNullOrEmpty(model))
                AppendChild(rt, "Model", model);

            AppendChild(rt, "PrimaryMACAddress", btSystem.GetPrimaryMAC(dev));
            AppendChild(rt, "UsingIPAddress", dev.Address);


            foreach (KeyValuePair<string, string> kp in _deviceFields)
            {
                if (string.IsNullOrEmpty(kp.Key) || string.IsNullOrEmpty(kp.Value))
                    continue;

                string val = null;
                if (dev.RemarksMap.TryGetValue(kp.Value, out val)
                    && !string.IsNullOrEmpty(val))
                    AppendChild(rt, kp.Value, val);
            }

            XmlElement containsInstance = FindAndAppendChild(rt, "ContainsInstance");
            processPorts(dev, true, containsInstance);
            processCPU(dev, rt.Name, containsInstance, originalKey);
            return xmlContainer;
        }

        static XmlContainer processSW(Device dev, Dictionary<string, XmlContainer> dict)
        {
            XmlContainer xmlContainer = createXml(GetAddress(dev.IP, dev.Address), dict);
            XmlElement sw = AppendChild(createXmlElement("Switch", xmlContainer), "Switch");
            //logger.InfoFormat("添加节点 {0}, 父节点{1}已有{2}个子节点", string.Concat(profix, "Device-", dev.DeviceId), sw.ParentNode.Name, sw.ParentNode.ChildNodes.Count);
            //AppendChild(sw, "OriginalKey", string.Concat(profix, "Switch-", dev.DeviceId));
            string originalKey = generateOriginalKey(dev);
            AppendChild(sw, "OriginalKey", originalKey);
            AppendChild(sw, "Name", dev.GetName());
            AppendChild(sw, "Description", dev.Description);
            AppendChild(sw, "DevLevel", dev.Level);
            AppendChild(sw, "Manufacturer", dev.Manufacturer);

            AppendChild(sw, "Architecture", dev.SysTypeName);
            AppendChild(sw, "ROMVersion", dev.SystemOID);
            AppendChild(sw, "OSInfo", dev.SysTypeName);

            string model = null;
            if (_deviceModels.TryGetValue(dev.SystemOID.Trim(new char[]{'.'}), out model)
                && !string.IsNullOrEmpty(model))
                AppendChild(sw, "Model", model);

            AppendChild(sw, "PrimaryMACAddress", btSystem.GetPrimaryMAC(dev));
            AppendChild(sw, "UsingIPAddress", dev.Address);


            foreach (KeyValuePair<string, string> kp in _deviceFields)
            {
                if (string.IsNullOrEmpty(kp.Key) || string.IsNullOrEmpty(kp.Value))
                    continue;

                string val = null;
                if (dev.RemarksMap.TryGetValue(kp.Value, out val)
                    && !string.IsNullOrEmpty(val))
                    AppendChild(sw, kp.Value, val);
            }

            XmlElement containsInstance = FindAndAppendChild(sw, "ContainsInstance");
            processPorts(btSystem, dev, false, containsInstance);
            processCPU(btSystem, dev, sw.Name, containsInstance, originalKey);
            return xmlContainer;
        }

        static string GetAddress(IPAddress ip, string address)
        {
            if (!string.IsNullOrEmpty(address))
                return address;
            foreach (KeyValuePair<IPSeg, string> kp in _corporationIPSeg)
            {
                if (kp.Key.In(ip))
                    return kp.Value;
            }
            return address;
        }

        static XmlContainer processLink(Link link, Dictionary<string, XmlContainer> dict)
        {
            Device dev1 = null;
            _deviceByIds.TryGetValue(link.Device1Id, out dev1);

            Device dev2 = null;
            _deviceByIds.TryGetValue(link.Device2Id, out dev2);
            if (null == dev1 || null == dev2)
            {
                logger.InfoFormat("线路[{0}:{1}]其中一端的设备没有,跳过!", link.Id, link.DisplayName);
                return null;
            }


            Interface port1 = btSystem.GetDevicePort(link.DevId1, link.IfIndex1);
            Interface port2 = btSystem.GetDevicePort(link.DevId2, link.IfIndex2);

            if (null == port1)
            {
                if (null != linkLogger)
                {
                    linkLogger.WriteLine(string.Concat("线路 - ", link.Id
                        , "-", (null == dev1) ? link.DevId1.ToString() : dev1.DisplayName
                        , "-", (null == dev2) ? link.DevId2.ToString() : dev2.DisplayName
                        , " ---- ", link.DisplayName));
                }

                if (!uploadForBadLink)
                {
                    logger.InfoFormat("线路[{0}:{1}]其中一端的端口没有,跳过!", link.Id, link.DisplayName);
                    return null;
                }
            }

            if (null == port2)
            {
                if (null != linkLogger)
                {
                    linkLogger.WriteLine(string.Concat("线路 - ", link.Id
                        , "-", (null == dev1) ? link.DevId1.ToString() : dev1.DisplayName
                        , "-", (null == dev2) ? link.DevId2.ToString() : dev2.DisplayName
                        , " ---- ", link.DisplayName));
                }

                if (!uploadForBadLink)
                {
                    logger.InfoFormat("线路[{0}:{1}]其中一端的端口没有,跳过!", link.Id, link.DisplayName);
                    return null;
                }
            }


            string dev1FieldName = "Port1ID";
            string dev2FieldName = "Port2ID";
            string port1FieldName = "Dev1ID";
            string port2FieldName = "Dev2ID";

            string className = "L2Link";
            //if (link.IsVirtual)
            //{
            //    className = "LogicalLink";

            //    dev1FieldName = "Dev1ID";
            //    dev2FieldName = "Dev2ID";
            //    port1FieldName = "Port1ID";
            //    port2FieldName = "Port2ID";
            //}

            XmlContainer xmlContainer = createXml(GetAddress(dev1.IP, dev1.Address), dict);
            XmlElement linkNode = AppendChild(createXmlElement(className, xmlContainer), className);


            //- <!--   原系统ID 必须填写 --> 
            //  <OriginalKey>640000701</OriginalKey> 
            AppendChild(linkNode, "OriginalKey", string.Concat(profix, "L2Link-", link.Id));
            //- <!--  链路名称 选填 --> 
            //  <Name>主干链路</Name> 
            AppendChild(linkNode, "Name", link.DisplayName);
            AppendChild(linkNode, "Description", string.IsNullOrEmpty(link.Descr) ? "" : link.Descr);

            //- <!--  链路类型 选填 --> 
            //  <Type>3</Type> 
            AppendChild(linkNode, "Type", link.TypeName);

            //- <!--  网络接口1ID 必须填写 --> 
            //  <Dev1ID>6000203</Dev1ID> 
            //AppendChild(linkNode, "Dev1ID", string.Concat(profix, "Device-", link.DevId1));

            //- <!--  网络接口2ID 必须填写 --> 
            //  <Dev2ID>670202</Dev2ID> 
            //AppendChild(linkNode, "Dev2ID", string.Concat(profix, "Device-", link.DevId2));


            // Port1ID与Dev1ID这两个字段意思是反的,这是南瑞制定的格式,我没有办法.
            // linkNode.AppendChild(linkNode.OwnerDocument.CreateComment("Port1ID与Dev1ID这两个字段意思是反的,但这是南瑞制定的格式,我没有办法."));


            if ("virtual" == link.TypeName)
            {
                AppendChild(linkNode, dev1FieldName, generateOriginalKey(dev1));
                AppendChild(linkNode, dev2FieldName, generateOriginalKey(dev2));
            }

            if (null != port1)
            {
                AppendChild(linkNode, port1FieldName, string.Concat(profix, "IpInterface-", port1.Id));
            }

            if (null != port2)
            {
                AppendChild(linkNode, port2FieldName, string.Concat(profix, "IpInterface-", port2.Id));
            }

            //- <!--  链路带宽 必须填写 --> 
            //  <BandWidth>10</BandWidth> 
            AppendChild(linkNode, "BandWidth", Math.Max(link.UserSpeedUp, link.UserSpeedDown) / 1000000);

            AppendChild(linkNode, "UpSpeed", link.UserSpeedUp / 1000000);

            AppendChild(linkNode, "DownSpeed", link.UserSpeedDown / 1000000);
            //AppendChild(linkNode, "LinkLevel", link.Level);
            return xmlContainer;
        }

        static void processPorts(Device dev, bool isRT, XmlElement element)
        {
            Dictionary<int, IPAddress> ipList = new Dictionary<int, IPAddress>();

            foreach (IPAddress ipData in dev.Children<IPAddress>(null))
            {
                ipList[ipData.IfIndex] = ipData;
            }

            foreach (Interface port in dev.Children<Interface>(null))
            {
                IPAddress ip = null;
                ipList.TryGetValue(port.IfIndex, out ip);

                //if ("172.16.138.254" == dev.IP)
                //    Console.WriteLine( "{0} - {1}", port.IfIndex, null == ip );


                processPort(dev, port, ip, isRT, element);
            }
        }

        static void processPort(Device dev
            , Interface port
            , IPAddress ipData
            , bool isRT
            , XmlElement element)
        {
            XmlElement ip = AppendChild(element, "IpInterface");

            //- <!--  原系统ID 必须填写  --> 
            //  <OriginalKey>7833</OriginalKey> 
            AppendChild(ip, "OriginalKey", string.Concat(profix, "IpInterface-", port.Id));

            //- <!--  设备名称 选填  --> 
            //  <Name>xxxxx</Name> 
            AppendChild(ip, "Name", port.ToString());

            //- <!--  端口在设备内部的编号/例如ETH1=1 必须填写  --> 
            //  <InternalNumber>1</InternalNumber> 
            AppendChild(ip, "InternalNumber", port.IfIndex);

            if (null == ipData)
            {
                if (!isRT)
                {
                    //- <!--  IP地址   --> 
                    //  <Address>10.144.99.1</Address> 
                    AppendChild(ip, "Address", dev.Address);

                    //- <!--  子网掩码   --> 
                    //  <NetworkMask>255.255.255.0</NetworkMask> 
                    AppendChild(ip, "NetworkMask", "255.255.255.0");
                }
            }
            else
            {
                AppendChild(ip, "Address", ipData.Address);
                AppendChild(ip, "NetworkMask", ipData.Mask);
            }

            //  <MACAddress>1A.73.8A.4A.32.21</MACAddress> 
            //- <!--  最大传输速率   --> 
            AppendChild(ip, "Speed", Math.Max(port.IfSpeedIn, port.IfSpeedOut) / 1000000);


            AppendChild(ip, "IpInterfaceType", port.IfTypeName);
            //AppendChild(ip, "Duplex", port.);
            AppendChild(ip, "MTU", port.IfMtu);


            //- <!--  MAC地址   --> 
            //  <MACAddress>1A.73.8A.4A.32.21</MACAddress> 
            AppendChild(ip, "MACAddress", port.IfPhysAddress);
        }

        static string Execute(string cmd, string arguments)
        {
            string result = "";
            using (System.Diagnostics.Process shell = new System.Diagnostics.Process())
            {
                shell.StartInfo.UseShellExecute = false;

                shell.StartInfo.FileName = cmd;
                shell.StartInfo.Arguments = arguments;
                shell.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                shell.StartInfo.RedirectStandardOutput = true;
                shell.Start();
                if (shell.WaitForExit(3 * 60 * 1000))
                {
                    result = shell.StandardOutput.ReadToEnd();
                    shell.Close();
                }
                else
                {
                    shell.Kill();
                    Kill(System.IO.Path.GetFileName(cmd), System.IO.Path.GetDirectoryName(cmd));
                    Kill("DW20.exe", System.IO.Path.GetDirectoryName(cmd));
                }
                return result;
            }
        }


        static private void Kill(string nm, string workingDirectory)
        {
            using (System.Diagnostics.Process shell = new System.Diagnostics.Process())
            {
                shell.StartInfo.UseShellExecute = true;
                string binDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(workingDirectory))
                    workingDirectory = binDirectory;
                else if (!System.IO.Path.IsPathRooted(workingDirectory))
                    workingDirectory = System.IO.Path.Combine(binDirectory, workingDirectory);
                shell.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath(workingDirectory);

                shell.StartInfo.FileName = System.IO.Path.Combine(binDirectory ,"kill.bat" );
                if (!File.Exists(shell.StartInfo.FileName))
                {
                    shell.StartInfo.FileName = System.IO.Path.Combine(binDirectory, "kill.exe");
                    shell.StartInfo.Arguments = "-f " + nm;
                }
                else
                {
                    shell.StartInfo.Arguments = nm;
                }
                shell.StartInfo.CreateNoWindow = true;
                shell.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                shell.Start();
                shell.WaitForExit();
                shell.Close();
            }
        }
    }

}
