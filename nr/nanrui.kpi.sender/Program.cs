using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Data.Common;
using System.Data;
using System.Transactions;
using System.Text.RegularExpressions;
using log4net;
using Apache.NMS;


namespace meijing.nanrui.kpi.sender
{
    public class PortKey
    {
        public int DeviceId;
        public int IfIndex;

        public PortKey()
        { }

        public PortKey(int deviceId, int ifIndex)
        {
            DeviceId = deviceId;
            IfIndex = ifIndex;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            PortKey data = obj as PortKey;
            if (null == data) return false;
            return DeviceId == data.DeviceId && this.IfIndex == data.IfIndex;
        }

        public override int GetHashCode()
        {
            return DeviceId * 100000 + IfIndex;
        }

        public override string ToString()
        {
            return string.Concat(DeviceId, "-", IfIndex);
        }
    }
    class PerformanceKey
    {
        public string moType;
        public int moId;
        public string performanceName;
        public int storeId;

        public PerformanceKey()
        { }

        public PerformanceKey(string moType, int moId, string performanceName, int storeId)
        {
            this.moType = moType;
            this.moId = moId;
            this.performanceName = performanceName;
            this.storeId = storeId;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            PerformanceKey data = obj as PerformanceKey;
            if (null == data) return false;
            return moId == data.moId
                && this.storeId == data.storeId
                && this.moType == data.moType
                && this.performanceName == data.performanceName;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat(moType, "-", moId, "-", storeId, performanceName);
        }
    }


    partial class Program
    {
        static int MAX_QUEUE = 1000000;
        static string _manufacturer = "Betasoft";
        static string _prefixId = "";
        static Encoding _encoding;
        static string _corporation;
        static string _basePath;

        static ILog _logger;
        static bool _isDebugging = false;


        static Dictionary<int, string> _deviceTypes = new Dictionary<int, string>();
        static Dictionary<int, string> _osTypes = new Dictionary<int, string>();
        static Dictionary<int, string> _serverTypes = new Dictionary<int, string>();

        static Dispatcher _deviceDispatcher = new Dispatcher();
        static Dispatcher _serviceDispatcher = new Dispatcher();
        static Dictionary<string, string> _performanceMapForPorts = new Dictionary<string, string>();
        static Dictionary<string, string> _performanceMapForLinks = new Dictionary<string, string>();
        static List<IMatcher> _serviceMatchers = new List<IMatcher>();


        static Dictionary<int, bool[]> _deviceList = null;
        static Dictionary<int, bool[]> _serverList = null;
        static Dictionary<int, bool[]> _linkList = null;



        //static Dictionary<int, Device> _devices = new Dictionary<int, Device>();

        static string GetDBConfigPath(string basePath, string dbPath)
        {
            if (Path.IsPathRooted(dbPath))
                return dbPath;

            return Path.Combine(basePath, dbPath);
        }

        //static void Test()
        //{
        //    ExecuteNonQuery( "delete from BHistoryReadySend" );
        //    ExecuteNonQuery( "delete from BHistorySendMap" );

        //    long now = ToInt64( DateTime.Now );
        //    ExecuteNonQuery("insert into BHistoryReadySend(BID,BCreatedTime,BMOId,BMOtype,BValue,BStatus,BStoreId) values(" + now.ToString() + ",@22,@23,@24,@25,@26,0)");

        //    data.Id = Convert.ToInt32(reader["BID"]);
        //    data.CreatedTime = ToInt64(Convert.ToDateTime(reader["BCreatedTime"]));
        //    data.MOId = Convert.ToInt32(reader["BMOId"]);
        //    data.MOType = reader["BMOtype"].ToString();
        //    data.StatusX = reader["BStatus"].ToString();
        //    data.StoreId = Convert.ToInt32(reader["BStoreId"]);
        //    data.Value = Convert.ToDouble(reader["BValue"]);
        //}

        static string GetBasePath(string basePath, string dbPath)
        {
            if (Path.IsPathRooted(dbPath))
                return dbPath;

            return Path.Combine(basePath, dbPath);
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

                _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Verbose, string.Format("{0}={1}", key, value), null);
            }
        }
        static void ReadMap(Dictionary<int, string> map, XmlSetting[] entries)
        {
            if (null == entries)
                return;

            foreach (XmlSetting driver in entries)
            {
                string key = driver.ReadSetting("@key", null);
                string value = driver.ReadSetting("@value", null);
                int intKey = 0;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || !int.TryParse(key, out intKey))
                    continue;
                map[intKey] = value;
                _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Verbose, string.Format("{0}={1}", intKey, value), null);
            }
        }

        static string encodeFile(string nm)
        {
            return nm.Replace('/', '-')
                .Replace('\\', '-')
                .Replace(':', '-')
                .Replace('"', '#')
                .Replace('|', '-')
                .Replace('*', '-')
                .Replace('?', '-')
                .Replace('<', '-')
                .Replace('>', '-');
        }

        static Dictionary<int, string> ReadIni(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            Dictionary<int, string> map = new Dictionary<int, string>();
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

                map[id] = ss.Substring(index + 1);
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

        static void Main(string[] args)
        {
            try
            {
                _basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


                foreach (string l in args)
                {
                    if (l.StartsWith("--PATH=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _basePath = GetBasePath(_basePath, l.Substring(7));
                    }
                    else if (l.StartsWith("--PrefixId=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _prefixId = l.Substring(11);
                    }
                }

                _isDebugging = File.Exists(Path.Combine(Path.GetDirectoryName(_basePath), "Betasoft.DataProvider.Debuger.Running"));

                XmlSetting xmlSetting = new XmlSetting(new StreamReader(Path.Combine(_basePath, "nanrui.performance.config")));
                XmlSetting defaultSetting = XmlSetting.ReadFromFile(Path.Combine(_basePath, "../nanrui.default.config"));

                _prefixId = defaultSetting.ReadSetting("/configuration/Locale/PrefixId/@value", _prefixId);
                _corporation = defaultSetting.ReadSetting("/configuration/Locale/Corporation/@value", "");


                string maxQueue = xmlSetting.ReadSetting("/configuration/Locale/MAX_QUEUE/@value", MAX_QUEUE.ToString());
                if (!int.TryParse(maxQueue, out MAX_QUEUE))
                    MAX_QUEUE = 1000000;

                log4net.Config.XmlConfigurator.Configure(xmlSetting.SelectOne("/configuration/log4net").AsXmlNode() as System.Xml.XmlElement);
                _logger = LogManager.GetLogger("Betanetworks.Performance");

                try
                {
                    _logger.Info("程序启动,开始读配置");

                    foreach (XmlSetting driver in xmlSetting.Select("/configuration/DBConfig/driver/*"))
                    {
                        string key = driver.ReadSetting("@key", null);
                        string typeString = driver.ReadSetting("@value", null);
                        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(typeString))
                            continue;

                        Type type = Type.GetType(typeString);
                        if (null == type)
                            continue;

                        DBSupport.AddDriver(key, type);
                    }

                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入操作系统类型!", null);
                    ReadMap(_osTypes, xmlSetting.Select("/configuration/OSTypes/*"));

                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入设备类型!", null);
                    ReadMap(_deviceTypes, xmlSetting.Select("/configuration/DeviceTypes/*"));
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入服务类型!", null);
                    ReadMap(_serverTypes, xmlSetting.Select("/configuration/ServiceTypes/*"));
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入设备指标映射!", null);
                    DispatchImpl.ReadDispatch(xmlSetting.SelectOne("/configuration/Maps/Device"), _deviceDispatcher, _logger);
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入服务指标映射!", null);
                    DispatchImpl.ReadDispatch(xmlSetting.SelectOne("/configuration/Maps/Service"), _serviceDispatcher, _logger);

                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入端口指标映射!", null);
                    ReadMap(_performanceMapForPorts, xmlSetting.Select("/configuration/PerformanceKeyMaps/Port/*"));
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入线路指标映射!", null);
                    ReadMap(_performanceMapForLinks, xmlSetting.Select("/configuration/PerformanceKeyMaps/Link/*"));

                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入允许发送的设备列表!", null);
                    _deviceList = ReadIniFromFile(Path.Combine(_basePath, "../deviceList.txt"));
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入允许发送的服务列表!", null);
                    _serverList = ReadIniFromFile(Path.Combine(_basePath, "../serverList.txt"));
                    _logger.Logger.Log(typeof(Dispatcher), log4net.Core.Level.Trace, "载入允许发送的线路列表!", null);
                    _linkList = ReadIniFromFile(Path.Combine(_basePath, "../linkList.txt"));



                    _serviceMatchers.Add(new SplitMatcher());

                    //foreach (XmlSetting setting in xmlSetting.Select("/configuration/RegexMatcher/*"))
                    //{
                    //    string nm = setting.ReadSetting("@name", null);
                    //    if (string.IsNullOrEmpty(nm))
                    //        continue;

                    //    string rx = setting.ReadSetting("@pattern", null);
                    //    if (string.IsNullOrEmpty(rx))
                    //        continue;

                    //    string sp = setting.ReadSetting("@separator", null);
                    //    if (string.IsNullOrEmpty(rx))
                    //        continue;
                    //    RegexMatcher rm = new RegexMatcher();
                    //    rm.Name = nm;
                    //    rm.Rx = new Regex(rx, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    //    rm.Separator = sp;
                    //    _serviceMatchers.Add(rm);

                    //    _logger.InfoFormat("载入正则处理器{0}, pattern={1}", nm, rx);
                    //}

                    using (DBSupport dbSupport = new DBSupport(xmlSetting.SelectOne("/configuration/DBConfig"), _basePath))
                    {
                        dbSupport.GetConnection();

                        _dbSupport = dbSupport;
                        _btSystem = new BTSystem(defaultSetting, dbSupport, _basePath);

                        _logger.Info("数据库连接建立成功!");


                        List<PerformanceData> dataList = GetAllReadyRecords();
                        _logger.InfoFormat("读队列表中的数据成功，共{0}条!", dataList.Count);
                        List<PerformanceData2> dataList2 = GetAllReadyRecords2();
                        _logger.InfoFormat("读队列表2中的数据成功，共{0}条!", dataList2.Count);

                        Dictionary<PerformanceKey, PerformanceData> dataMap = GetAllRecordFromMap();
                        _logger.InfoFormat("读MAP表中的数据成功，共{0}条!", dataMap.Count);
                        Dictionary<PerformanceKey, PerformanceData2> dataMap2 = GetAllRecordFromMap2();
                        _logger.InfoFormat("读MAP表中的数据成功，共{0}条!", dataMap2.Count);

                        MoveToMAP(dataList, dataMap);
                        _logger.Info("更校报MAP表完成!");
                        MoveToMAP2(dataList2, dataMap2);
                        _logger.Info("更校报MAP表2完成!");

                        long now = ToInt64(DateTime.Now);


                        List<PerformanceBase> recordForSend = new List<PerformanceBase>();
                        foreach (PerformanceBase perf in GetSendRecordFromMap(dataList.Count > 0, now))
                        {
                            recordForSend.Add(perf);
                        }

                        foreach (PerformanceBase perf in GetSendRecordFromMap2(dataList.Count > 0, now))
                        {
                            recordForSend.Add(perf);
                        }

                        if (0 == recordForSend.Count)
                        {
                            _logger.InfoFormat("取得需要发送的消息{0}条,没有需要发送的数据,退出程序!", recordForSend.Count);
                            return;
                        }

                        _logger.InfoFormat("取得需要发送的消息{0}条,开始登录MQ!", recordForSend.Count);
                        int count = 0;
                        try
                        {
                            _encoding = System.Text.Encoding.GetEncoding(defaultSetting.ReadSetting("/configuration/Locale/Encoding/@value", "utf-8"));

                            using (Session session = NMSSupport.Create(defaultSetting))
                            {
                                //IDestination destination = Apache.NMS.Util.SessionUtil.GetDestination(session, xmlSetting.ReadSetting("/configuration/MQ/DestinationName/@value"));
                                //using (IMessageProducer producer = session.CreateProducer(destination))
                                {
                                    _logger.InfoFormat("登录MQ成功开始发送消息!");

                                    string nowTimestamp = DateTime.Now.ToString();

                                    foreach (PerformanceBase data in recordForSend.ToArray())
                                    {
                                        string logPath = Path.Combine(Path.Combine(_basePath, "log"), data.MOType);
                                        if (!Directory.Exists(logPath))
                                            Directory.CreateDirectory(logPath);

                                        string logFile = Path.Combine(logPath, string.Concat(data.MOType, "-", data.MOId, "-", encodeFile(data.StatusX), "-", data.StoreId, ".txt"));
                                        string historyFile = Path.Combine(logPath, string.Concat(data.MOType, "-", data.MOId, "-", encodeFile(data.StatusX), "-", data.StoreId, ".history"));

                                        using (StreamWriter writer = new StreamWriter(logFile))
                                        {
                                            MapMessage message = createMapMessage(session, data, writer);
                                            if (null != message)
                                            {
                                                session.Send(Destination.PERFORMANCE, message);


                                                Session.Error[] errors = session.GetLastErrors();
                                                foreach (Session.Error err in errors)
                                                {
                                                    writer.Write("//MQError:channel="); writer.Write(err.Name); writer.Write("="); writer.WriteLine(err.exception.Message);
                                                }


                                                count++;
                                                recordForSend.Remove(data);

                                                if (_isDebugging)
                                                {
                                                    File.AppendAllText(historyFile, nowTimestamp + " success\r\n");
                                                }
                                            }
                                            else if (_isDebugging && File.Exists(historyFile))
                                            {
                                                File.AppendAllText(historyFile, nowTimestamp + " error\r\n");
                                            }
                                        }
                                    }
                                }
                            }

                            _logger.InfoFormat("本次消息发送完成,共{0}条,开始更新发送时间!", count);
                            _dbSupport.ExecuteNonQuery(string.Format(SQLMapper.GetSqlString(SQLDialect.UPDATE_MAP_SEND_TIME), now));
                            _dbSupport.ExecuteNonQuery(string.Format(SQLMapper.GetSqlString(SQLDialect.UPDATE_MAP_SEND_TIME2), now));
                            _logger.InfoFormat("更新发送时间完成,退出程序!");
                            return;
                        }
                        catch (Exception e)
                        {
                            _logger.Fatal(e);
                        }

                        try
                        {
                            if (_isDebugging)
                            {
                                string nowTimestamp = DateTime.Now.ToString();

                                foreach (PerformanceBase data in recordForSend.ToArray())
                                {
                                    string logPath = Path.Combine(Path.Combine(_basePath, "log"), data.MOType);
                                    string historyFile = Path.Combine(logPath, string.Concat(data.MOType, "-", data.MOId, "-", encodeFile(data.StatusX), "-", data.StoreId, ".history"));

                                    if (File.Exists(historyFile))
                                    {
                                        File.AppendAllText(historyFile, nowTimestamp + " mq error\r\n");
                                    }
                                }
                            }
                        }
                        catch (Exception exx)
                        {
                            _logger.Fatal("记录调试数据时发生错误", exx);
                        }
                    }
                }
                //catch (DbException db)
                //{
                //    _logger.Fatal("发生数据库错误!", db);

                //    _logger.Fatal("尝试恢复数据库!");
                //    try
                //    {
                //        initialize_datebase(xmlSetting);
                //        _logger.Fatal("恢复数据库成功!");
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.Fatal("恢复数据库成功失败", ex);
                //    }
                //}
                catch (Exception ex)
                {
                    _logger.Fatal(ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        #region 处理表1
        static IDbDataParameter CreateParameter(IDbCommand command, string name, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        static DateTime START_TIME = new DateTime(1970, 1, 1);
        static long ToInt64(DateTime time)
        {
            //TimeSpan time = ;
            return (long)((DateTime.Parse(time.ToString()) - START_TIME).TotalSeconds);
        }

        /// <summary>
        /// 返回所有的待发送表的数据
        /// </summary>
        static List<PerformanceData> GetAllReadyRecords()
        {
            try
            {
                int count = 0;
                /// 如果数据大多时，只有丢数据了
                using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.COUNT_QUEUE)))
                {
                    if (reader.Read())
                        count = Convert.ToInt32(reader[0]);
                }

                if (count > MAX_QUEUE)
                {
                    int result = _dbSupport.ExecuteNonQuery(SQLMapper.GetSqlString(SQLDialect.DELETE_QUEUE));
                    _logger.ErrorFormat("因为消息太多，删除了一部分多余的数据", result);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            List<PerformanceData> datas = new List<PerformanceData>();
            using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.SELECT_QUEUE)))
            {
                while (reader.Read())
                {
                    int bid = 0;
                    try
                    {
                        //if (reader["BMOId"].GetType() != typeof(DBNull)
                        //    && reader["BStoreId"].GetType() != typeof(DBNull)
                        //    && reader["BValue"].GetType() != typeof(DBNull))
                        //{
                        PerformanceData data = new PerformanceData();
                        bid = Convert.ToInt32(reader["BID"]);
                        data.Id = bid;
                        data.CreatedTime = ToInt64(Convert.ToDateTime(reader["BCreatedTime"]));
                        data.MOId = Convert.ToInt32(reader["BMOId"]);
                        data.MOType = reader["BMOtype"].ToString();
                        data.StatusX = reader["BStatus"].ToString();
                        data.StoreId = Convert.ToInt32(reader["BStoreId"]);
                        data.Value = DBSupport.ToDouble(reader["BValue"], double.NaN);
                        if(!double.NaN.Equals(data.Value))
                            datas.Add(data);
                        //}
                    }
                    catch (Exception e)
                    {
                        _logger.Error("在理" + bid.ToString() + "时出错", e);
                        //throw;
                    }
                }
            }
            return datas;
        }

        static PerformanceData ReadRecordFromMap(IDataReader reader)
        {
            PerformanceData data = new PerformanceData();
            data.CreatedTime = Convert.ToInt64(reader["BCreatedTime"]);
            data.MOId = Convert.ToInt32(reader["BMOId"]);
            data.MOType = reader["BMOtype"].ToString();
            data.StatusX = reader["BStatus"].ToString();
            data.StoreId = Convert.ToInt32(reader["BStoreId"]);
            data.Value = Convert.ToDouble(reader["BValue"]);
            data.SendTime = Convert.ToInt64(reader["BSendTime"]);
            return data;
        }
        static Dictionary<PerformanceKey, PerformanceData> GetAllRecordFromMap()
        {
            Dictionary<PerformanceKey, PerformanceData> dataList = new Dictionary<PerformanceKey, PerformanceData>();
            using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.SELECT_MAP)))
            {
                while (reader.Read())
                {
                    PerformanceData data = ReadRecordFromMap(reader);
                    dataList[new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId)] = data;
                }
            }
            return dataList;
        }

        /// <summary>
        /// 返回MAP表中的所有记录
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static List<PerformanceData> GetRecordFromMap(string sql)
        {
            List<PerformanceData> dataList = new List<PerformanceData>();
            using (IDataReader reader = _dbSupport.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    PerformanceData data = ReadRecordFromMap(reader);
                    dataList.Add(data);
                }
            }
            return dataList;
        }

        static string GetParameter(string driver, string tag, int i)
        {
            return (("oracle" == _dbSupport.DriverType) ? ":" : "@") + tag + i.ToString();
        }

        /// <summary>
        /// 将待发送表中的数据转移到MAP表中并清空
        /// </summary>
        /// <param name="datas"></param>
        static void MoveToMAP(List<PerformanceData> dataList, Dictionary<PerformanceKey, PerformanceData> dataMap)
        {
            if (0 == dataList.Count)
                return;

            int maxId = 0;
            Dictionary<PerformanceKey, PerformanceData> middleMap = new Dictionary<PerformanceKey, PerformanceData>();
            foreach (PerformanceData data in dataList)
            {
                maxId = Math.Max(data.Id, maxId);


                middleMap[new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId)] = data;

                //PerformanceData old = null;
                //PerformanceKey key = new PerformanceKey(data.StoreId, data.StatusX);
                //if (!middleMap.TryGetValue(key, out old))
                //{
                //    middleMap.Add(key, data);
                //}
                //else
                //{ 
                //    if( old.CreateTime < data.CreatedTime)
                //    {
                //         old.CreatedTime = data.CreatedTime;
                //         old.Value = data.Value;
                //    }
                //}
            }

            _logger.InfoFormat("需要更新到MAP表的数据有{0}条!", middleMap.Count);

            using (IDbTransaction transaction = _dbSupport.GetConnection().BeginTransaction())
            {

                IDbCommand update = _dbSupport.CreateCommand(SQLMapper.GetSqlString(SQLDialect.UPDATE_MAP));
                update.Transaction = transaction;

                IDbCommand insert = _dbSupport.CreateCommand(SQLMapper.GetSqlString(SQLDialect.INSERT_MAP));
                insert.Transaction = transaction;


                IDbDataParameter u1 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 1), null);
                IDbDataParameter u2 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 2), null);
                IDbDataParameter u3 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 3), null);
                IDbDataParameter u4 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 4), null);
                IDbDataParameter u5 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 5), null);
                IDbDataParameter u6 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 6), null);

                update.Parameters.Add(u1);
                update.Parameters.Add(u2);
                update.Parameters.Add(u3);
                update.Parameters.Add(u4);
                update.Parameters.Add(u5);
                update.Parameters.Add(u6);


                IDbDataParameter i1 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 1), null);
                IDbDataParameter i2 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 2), null);
                IDbDataParameter i3 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 3), null);
                IDbDataParameter i4 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 4), null);
                IDbDataParameter i5 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 5), null);
                IDbDataParameter i6 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 6), null);

                insert.Parameters.Add(i1);
                insert.Parameters.Add(i2);
                insert.Parameters.Add(i3);
                insert.Parameters.Add(i4);
                insert.Parameters.Add(i5);
                insert.Parameters.Add(i6);


                foreach (PerformanceData data in middleMap.Values)
                {
                    PerformanceData old = null;

                    PerformanceKey key = new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId);
                    if (dataMap.TryGetValue(key, out old))
                    {
                        old.CreatedTime = data.CreatedTime;
                        old.Value = data.Value;

                        u1.Value = data.CreatedTime;
                        u2.Value = data.Value;
                        u3.Value = data.StatusX;
                        u4.Value = data.MOType;
                        u5.Value = data.MOId;
                        u6.Value = data.StoreId;

                        try
                        {
                            update.ExecuteNonQuery();
                        }
                        catch
                        {
                            _logger.FatalFormat("createdTime-{0},StatusX={1},MOType={2},MOId={3},StoreId={4},Value={5}"
                                , data.CreatedTime, data.StatusX, data.MOType, data.MOId, data.StoreId, data.Value);
                            throw;
                        }
                    }
                    else
                    {
                        dataMap[key] = data;

                        i1.Value = data.CreatedTime;
                        i2.Value = data.Value;
                        i3.Value = data.StatusX;
                        i4.Value = data.StoreId;
                        i5.Value = data.MOId;
                        i6.Value = data.MOType;

                        try
                        {
                            insert.ExecuteNonQuery();
                        }
                        catch
                        {
                            _logger.FatalFormat("createdTime-{0},StatusX={1},MOType={2},MOId={3},StoreId={4},Value={5}"
                                , data.CreatedTime, data.StatusX, data.MOType, data.MOId, data.StoreId, data.Value);
                            throw;
                        }
                    }

                    maxId = Math.Max(data.Id, maxId);
                }

                IDbCommand delete = _dbSupport.CreateCommand(string.Format(SQLMapper.GetSqlString(SQLDialect.DELETE_QUEUE_BY_TIME), maxId));
                delete.Transaction = transaction;
                delete.ExecuteNonQuery();
                transaction.Commit();
            }
        }


        static double createSimulatedData(PerformanceData data)
        {
            int raw = Convert.ToInt32(data.Value);

            Random random = new Random();
            double result = random.NextDouble() * random.Next(-1, 1) * 0.1;
            double value = data.Value * (1 + result);
            if (value < 0)
                value = 0;

            if (raw <= 100)
            {
                if (value > 100)
                    value = 100;
            }

            if (raw >= 0)
            {
                if (value < 0)
                    value = 0;
            }
            return value;
        }

        /// <summary>
        /// 处理MAP表中的信息
        /// </summary>
        static List<PerformanceData> GetSendRecordFromMap(bool deleteTimeout, long now)
        {
            /// 删除MAP表中没有匹配的STOREID的纪录
            int deleteCount = _dbSupport.ExecuteNonQuery(SQLMapper.GetSqlString(SQLDialect.DELETE_MAP_NOT_IN_HISTORY));
            _logger.InfoFormat("删除MAP表中没有匹配的STOREID的纪录{0}条!", deleteCount);


            if (deleteTimeout)
            {
                /// 删除MAP表中时间已经超时的纪录
                deleteCount = _dbSupport.ExecuteNonQuery(string.Format(SQLMapper.GetSqlString(SQLDialect.DELETE_MAP_TIMEOUT), now));
                _logger.InfoFormat("删除MAP表中时间已经超时的纪录{0}条!", deleteCount);
            }

            List<PerformanceData> dataList = GetRecordFromMap(string.Format(SQLMapper.GetSqlString(SQLDialect.SELECT_MAP_SEND_TIMEOUT), now));

            foreach (PerformanceData data in dataList)
            {
                data.CreatedTime = now;

                if (data.StatusX == "Memory" || data.StatusX == "Cpu")
                {
                    if (data.Value < 0)
                        data.Value = 0;
                    else if (data.Value > 100)
                        data.Value = 100;
                }

                if (data.SendTime == 0)
                    continue;

                if (data.SendTime > data.CreatedTime)// > 5 * 60)
                    //发送模拟数据
                    data.Value = createSimulatedData(data);
            }

            return dataList;
        }

        #endregion

        #region 处理表2

        static List<PerformanceData2> GetAllReadyRecords2()
        {
            try
            {
                int count = 0;
                /// 如果数据大多时，只有丢数据了
                using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.COUNT_QUEUE2)))
                {
                    if (reader.Read())
                        count = Convert.ToInt32(reader[0]);
                }

                if (count > MAX_QUEUE)
                {
                    int result = _dbSupport.ExecuteNonQuery(SQLMapper.GetSqlString(SQLDialect.DELETE_QUEUE2));
                    _logger.ErrorFormat("因为消息太多，删除了一部分多余的数据", result);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            List<PerformanceData2> datas = new List<PerformanceData2>();
            using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.SELECT_QUEUE2)))
            {
                while (reader.Read())
                {
                    if (reader["BMOId"].GetType() != typeof(DBNull)
                        && reader["BStoreId"].GetType() != typeof(DBNull)
                        && reader["BValue"].GetType() != typeof(DBNull))
                    {
                        PerformanceData2 data = new PerformanceData2();
                        data.Id = Convert.ToInt32(reader["BID"]);
                        data.CreatedTime = ToInt64(Convert.ToDateTime(reader["BCreatedTime"]));
                        data.MOId = Convert.ToInt32(reader["BMOId"]);
                        data.MOType = reader["BMOtype"].ToString();
                        data.StatusX = reader["BStatus"].ToString();
                        data.StoreId = Convert.ToInt32(reader["BStoreId"]);
                        data.Value = Convert.ToString(reader["BValue"]);
                        datas.Add(data);
                    }
                }
            }
            return datas;
        }

        static PerformanceData2 ReadRecordFromMap2(IDataReader reader)
        {
            PerformanceData2 data = new PerformanceData2();

            data.CreatedTime = Convert.ToInt64(reader["BCreatedTime"]);
            data.MOId = Convert.ToInt32(reader["BMOId"]);
            data.MOType = reader["BMOtype"].ToString();
            data.StatusX = reader["BStatus"].ToString();
            data.StoreId = Convert.ToInt32(reader["BStoreId"]);
            data.Value = Convert.ToString(reader["BValue"]);
            data.SendTime = Convert.ToInt64(reader["BSendTime"]);
            return data;
        }
        static Dictionary<PerformanceKey, PerformanceData2> GetAllRecordFromMap2()
        {
            Dictionary<PerformanceKey, PerformanceData2> dataList = new Dictionary<PerformanceKey, PerformanceData2>();
            using (IDataReader reader = _dbSupport.ExecuteReader(SQLMapper.GetSqlString(SQLDialect.SELECT_MAP2)))
            {
                while (reader.Read())
                {
                    PerformanceData2 data = ReadRecordFromMap2(reader);
                    dataList[new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId)] = data;
                }
            }
            return dataList;
        }

        /// <summary>
        /// 返回MAP表中的所有记录
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static List<PerformanceData2> GetRecordFromMap2(string sql)
        {
            List<PerformanceData2> dataList = new List<PerformanceData2>();
            using (IDataReader reader = _dbSupport.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    PerformanceData2 data = ReadRecordFromMap2(reader);
                    dataList.Add(data);
                }
            }
            return dataList;
        }

        /// <summary>
        /// 将待发送表中的数据转移到MAP表中并清空
        /// </summary>
        /// <param name="datas"></param>
        static void MoveToMAP2(List<PerformanceData2> dataList, Dictionary<PerformanceKey, PerformanceData2> dataMap)
        {
            if (0 == dataList.Count)
                return;

            int maxId = 0;
            Dictionary<PerformanceKey, PerformanceData2> middleMap = new Dictionary<PerformanceKey, PerformanceData2>();
            foreach (PerformanceData2 data in dataList)
            {
                maxId = Math.Max(data.Id, maxId);


                middleMap[new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId)] = data;

                //PerformanceData old = null;
                //PerformanceKey key = new PerformanceKey(data.StoreId, data.StatusX);
                //if (!middleMap.TryGetValue(key, out old))
                //{
                //    middleMap.Add(key, data);
                //}
                //else
                //{ 
                //    if( old.CreateTime < data.CreatedTime)
                //    {
                //         old.CreatedTime = data.CreatedTime;
                //         old.Value = data.Value;
                //    }
                //}
            }

            _logger.InfoFormat("需要更新到MAP表的数据有{0}条!", middleMap.Count);

            using (IDbTransaction transaction = _dbSupport.GetConnection().BeginTransaction())
            {

                IDbCommand update = _dbSupport.CreateCommand(SQLMapper.GetSqlString(SQLDialect.UPDATE_MAP2));
                update.Transaction = transaction;

                IDbCommand insert = _dbSupport.CreateCommand(SQLMapper.GetSqlString(SQLDialect.INSERT_MAP2));
                insert.Transaction = transaction;

                IDbDataParameter u1 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 1), null);
                IDbDataParameter u2 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 2), null);
                IDbDataParameter u3 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 3), null);
                IDbDataParameter u4 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 4), null);
                IDbDataParameter u5 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 5), null);
                IDbDataParameter u6 = CreateParameter(update, GetParameter(_dbSupport.DriverType, "u", 6), null);

                update.Parameters.Add(u1);
                update.Parameters.Add(u2);
                update.Parameters.Add(u3);
                update.Parameters.Add(u4);
                update.Parameters.Add(u5);
                update.Parameters.Add(u6);


                IDbDataParameter i1 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 1), null);
                IDbDataParameter i2 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 2), null);
                IDbDataParameter i3 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 3), null);
                IDbDataParameter i4 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 4), null);
                IDbDataParameter i5 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 5), null);
                IDbDataParameter i6 = CreateParameter(insert, GetParameter(_dbSupport.DriverType, "i", 6), null);

                insert.Parameters.Add(i1);
                insert.Parameters.Add(i2);
                insert.Parameters.Add(i3);
                insert.Parameters.Add(i4);
                insert.Parameters.Add(i5);
                insert.Parameters.Add(i6);


                foreach (PerformanceData2 data in middleMap.Values)
                {
                    PerformanceData2 old = null;

                    PerformanceKey key = new PerformanceKey(data.MOType, data.MOId, data.StatusX, data.StoreId);
                    if (dataMap.TryGetValue(key, out old))
                    {
                        old.CreatedTime = data.CreatedTime;
                        old.Value = data.Value;

                        u1.Value = data.CreatedTime;
                        u2.Value = data.Value;
                        u3.Value = data.StatusX;
                        u4.Value = data.MOType;
                        u5.Value = data.MOId;
                        u6.Value = data.StoreId;

                        update.ExecuteNonQuery();
                    }
                    else
                    {
                        dataMap[key] = data;

                        i1.Value = data.CreatedTime;
                        i2.Value = data.Value;
                        i3.Value = data.StatusX;
                        i4.Value = data.StoreId;
                        i5.Value = data.MOId;
                        i6.Value = data.MOType;

                        insert.ExecuteNonQuery();
                    }

                    maxId = Math.Max(data.Id, maxId);
                }

                IDbCommand delete = _dbSupport.CreateCommand(string.Format(SQLMapper.GetSqlString(SQLDialect.DELETE_QUEUE_BY_TIME2), maxId));
                delete.Transaction = transaction;
                delete.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        /// <summary>
        /// 处理MAP表中的信息
        /// </summary>
        static List<PerformanceData2> GetSendRecordFromMap2(bool deleteTimeout, long now)
        {
            /// 删除MAP表中没有匹配的STOREID的纪录
            int deleteCount = _dbSupport.ExecuteNonQuery(SQLMapper.GetSqlString(SQLDialect.DELETE_MAP_NOT_IN_HISTORY2));
            _logger.InfoFormat("删除MAP表中没有匹配的STOREID的纪录{0}条!", deleteCount);


            if (deleteTimeout)
            {
                /// 删除MAP表中时间已经超时的纪录
                deleteCount = _dbSupport.ExecuteNonQuery(string.Format(SQLMapper.GetSqlString(SQLDialect.DELETE_MAP_TIMEOUT2), now));
                _logger.InfoFormat("删除MAP表中时间已经超时的纪录{0}条!", deleteCount);
            }

            List<PerformanceData2> dataList = GetRecordFromMap2(string.Format(SQLMapper.GetSqlString(SQLDialect.SELECT_MAP_SEND_TIMEOUT2), now));

            foreach (PerformanceData2 data in dataList)
            {
                data.CreatedTime = now;
            }

            return dataList;
        }

        #endregion
        #region MESSAGE

        static int GetOriginalKey(PerformanceBase data)
        {
            return data.MOId;

        }
        public static string GetIfStatus(object value)
        {
            /* 端口状态 0up,1休眠,2down*/
            return Convert.ToInt32(value) == 0 ? "1" : "0";
        }

        static bool tryParseDateTime(string s, out DateTime time)
        {
            time = DateTime.MinValue;

            if (12 > s.Length)
                return false;
            int year = 0;
            int month = 0;
            int day = 0;
            int hour = 0;
            int mintue = 0;
            int second = 0;

            if (int.TryParse(s.Substring(0, 4), out year)
                && int.TryParse(s.Substring(4, 2), out month)
                && int.TryParse(s.Substring(6, 2), out day)
                && int.TryParse(s.Substring(8, 2), out hour)
                && int.TryParse(s.Substring(10, 2), out mintue))
            {
                if (14 <= s.Length)
                    int.TryParse(s.Substring(12, 2), out second);

                time = new DateTime(year, month, day, hour, mintue, second);
                return true;
            }
            return false;
        }

        static string GetValue(object value)
        {
            if (value is double)
            {
                return ((double)value).ToString("##################################0.##");
            }
            return value.ToString();
        }

        static bool createMapMessageFromServer(MapMessage message, PerformanceBase data, TextWriter writer)
        {
            ServiceStaticPropertiesData server = _btSystem.GetService(data.MOId);
            if (null == server)
            {
                writer.WriteLine("//找不到服务{0}", data.MOId);
                return false;
            }

            Device dev = server.Device;
            if (null == dev)
            {
                writer.WriteLine("//找不到服务[{0}:{1}]所在的设备{2}", data.MOId, server.CustomName, server.DeviceId);
                return false;
            }


            writer.Write("//IP="); writer.WriteLine(server.Device.IP);
            writer.Write("//serviceId="); writer.WriteLine(server.Id);
            writer.Write("//serviceType="); writer.WriteLine(server.Type);
            writer.Write("//serviceName="); writer.WriteLine(server.CustomName);
            bool[] sa;
            if (null != _serverList && _serverList.TryGetValue(data.MOId, out sa) && !(sa[0] && sa[1]))
            {
                writer.WriteLine("//对象不在上传列表中");
                return false;
            }

            string type = null;
            _serverTypes.TryGetValue(server.Type, out type);
            if (string.IsNullOrEmpty(type))
            {
                writer.WriteLine("//服务{0}的类型不正确,不可识别 - '{1}'.", server.Id, server.Type);
                return false;
            }

            //string nanruiKey = string.Concat("OriginalKey=", manufacturer, PrefixId, "Device-", dev.Id);
            string nanruiKey = string.Concat("OriginalKey=", _manufacturer, _prefixId, "SERVER-", data.MOId);
            string dataKey = data.StatusX;

            HistoryRule rule = _btSystem.GetHistoryRuleByStoreId(data.StoreId);
            if (null != rule)
            {
                writer.WriteLine("//查询历史记录规则 storeId='{0}',修正指标 old='{1}', new='{2}'.", data.StoreId, dataKey, rule.PKIs);
                dataKey = rule.PKIs;
            }

            Context context = new Context();
            context["serviceType"] = server.Type.ToString();
            context["serviceId"] = server.Id.ToString();
            context["performanceKey"] = dataKey;

            string fieldKey = null;
            string fieldValue = null;
            foreach (IMatcher matcher in _serviceMatchers)
            {
                MatchResult matchResult = matcher.Match(dataKey);
                if (null != matchResult)
                {
                    fieldKey = matchResult.Misc[0].Key;
                    fieldValue = matchResult.Misc[0].Value;
                    context["schema"] = matchResult.Schema;
                    context["objectKey"] = fieldKey;
                    context["objectId"] = fieldValue;
                    context["performanceKey"] = matchResult.PKI;
                    dataKey = matchResult.PKI;

                    writer.WriteLine("//分解PKI, schema='{0}',performanceKey='{1}', '{2}'='{3}'."
                        , matchResult.Schema
                        , matchResult.PKI
                        , fieldKey
                        , fieldValue);
                    break;
                }
            }

            DispatchResult result = _serviceDispatcher.Find(context);
            if (null == result)
            {
                writer.WriteLine("//服务{0}的性能参数不正确,不可识别 - '{1}'.", server.Id, data.StatusX);
                return false;
            }
            if (string.IsNullOrEmpty(result.Alias) || 0 == string.Compare("null", result.Alias, true))
            {
                writer.WriteLine("//服务{0}的性能参数不正确,不可识别,可能不用发送 - '{1}'.", server.Id, data.StatusX);
                return false;
            }

            dataKey = result.Alias;
            string objectKey = null;
            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (string.IsNullOrEmpty(result.Separator))
                {
                    objectKey = string.Concat(nanruiKey, "-", fieldKey, "-", fieldValue);
                    dataKey = string.Concat(result.Alias, ":", objectKey);
                }
                else if ("null" != result.Separator)
                {
                    objectKey = string.Concat(nanruiKey, result.Separator, fieldValue);
                    dataKey = string.Concat(result.Alias, ":", objectKey);
                }
                //else
                //{
                //    objectKey = serviceKey;
                //    dataKey = string.Concat(result.Alias, ":", objectKey);
                //}

                writer.WriteLine("//修正服务的性能参数 - '{0}'.", dataKey);
            }
            //else
            //{
            //    objectKey = serviceKey;
            //    dataKey = string.Concat(result.Alias, ":", objectKey);
            //}

            string performanceValue = GetValue(data.GetValue());

            if (0 == string.Compare("open", performanceValue, true))
            {
                performanceValue = "1";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("close", performanceValue, true))
            {
                performanceValue = "0";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("up", performanceValue, true))
            {
                performanceValue = "1";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("down", performanceValue, true))
            {
                performanceValue = "0";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("running", performanceValue, true))
            {
                performanceValue = "1";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("active", performanceValue, true))
            {
                performanceValue = "1";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("inactive", performanceValue, true))
            {
                performanceValue = "0";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (data.StatusX == "PendingRequestOldestTime")
            {
                try
                {
                    DateTime timeValue;
                    if (!tryParseDateTime(performanceValue, out timeValue))
                    {
                        writer.WriteLine("//日期格式无法分析");
                        return false;
                    }

                    performanceValue = timeValue.ToFileTimeUtc().ToString();
                }
                catch (Exception ex)
                {
                    writer.WriteLine("//" + ex.ToString());
                    return false;
                }
            }

            message.Body["MAINDATA"] = _encoding.GetBytes(nanruiKey);
            message.Body["SCENE"] = _encoding.GetBytes(_manufacturer);
            message.Body["CLASSNAME"] = _encoding.GetBytes(type);
            message.Body[dataKey] = _encoding.GetBytes(performanceValue);


            writer.Write("//PKI="); writer.WriteLine(dataKey);

            writer.Write("MAINDATA="); writer.WriteLine(nanruiKey);
            writer.Write("SCENE="); writer.WriteLine(_manufacturer);
            writer.Write("CLASSNAME="); writer.WriteLine(type);
            writer.Write(dataKey); writer.Write("="); writer.WriteLine(performanceValue);
            return true;
        }

        static bool createMapMessageFromDevice(MapMessage message, PerformanceBase data, TextWriter writer)
        {
            Device dev = _btSystem.GetDevice(data.MOId);
            if (null == dev)
            {
                writer.WriteLine("//找不到设备{0}", data.MOId);
                return false;
            }

            writer.Write("//IP="); writer.WriteLine(dev.IP);
            bool[] sa;
            if (null != _deviceList && _deviceList.TryGetValue(data.MOId, out sa) && !(sa[0] && sa[1]))
            {
                writer.WriteLine("//对象不在上传列表中");
                return false;
            }

            string type = null;
            if (5 == dev.DeviceType)
            {
                if (!_osTypes.TryGetValue(dev.SystemType, out type))
                    type = "Computersystem";
            }
            else
            {
                _deviceTypes.TryGetValue(dev.DeviceType, out type);
            }

            if (string.IsNullOrEmpty(type))
            {
                writer.WriteLine("//设备{0}的类型不正确,不可识别 - '{1}:{2}'.", dev.Id, dev.DeviceType, dev.SystemType);
                return false;
            }

            string nanruiKey = string.Concat("OriginalKey=", _manufacturer, _prefixId, "Device-", dev.Id);
            string dataKey = data.StatusX;

            HistoryRule rule = _btSystem.GetHistoryRuleByStoreId(data.StoreId);
            if (null != rule)
            {
                writer.WriteLine("//查询历史记录规则 storeId='{0}',修正指标 old='{1}', new='{2}'.", data.StoreId, dataKey, rule.PKIs);
                dataKey = rule.PKIs;
            }

            Context context = new Context();
            context["deviceType"] = dev.DeviceType.ToString();
            context["deviceOS"] = dev.SystemType.ToString();
            context["deviceId"] = dev.Id.ToString();

            context["performanceKey"] = dataKey;

            string fieldKey = null;
            string fieldValue = null;
            foreach (IMatcher matcher in _serviceMatchers)
            {
                MatchResult matchResult = matcher.Match(dataKey);
                if (null != matchResult)
                {
                    fieldKey = matchResult.Misc[0].Key;
                    fieldValue = matchResult.Misc[0].Value;
                    context["schema"] = matchResult.Schema;
                    context["objectKey"] = fieldKey;
                    context["objectId"] = fieldValue;
                    context["performanceKey"] = matchResult.PKI;
                    dataKey = matchResult.PKI;

                    writer.WriteLine("//分解PKI, schema='{0}',performanceKey='{1}', '{2}'='{3}'."
                        , matchResult.Schema
                        , matchResult.PKI
                        , fieldKey
                        , fieldValue);
                    break;
                }
            }

            DispatchResult result = _deviceDispatcher.Find(context);
            if (null == result)
            {
                writer.WriteLine("//设备{0}的性能参数不正确,不可识别 - '{1}'.", dev.Id, data.StatusX);
                return false;
            }
            if (string.IsNullOrEmpty(result.Alias) || 0 == string.Compare("null", result.Alias, true))
            {
                writer.WriteLine("//设备{0}的性能参数不正确,不可识别,可能不用发送 - '{1}'.", dev.Id, data.StatusX);
                return false;
            }

            dataKey = result.Alias;
            string objectKey = null;
            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (string.IsNullOrEmpty(result.Separator))
                {
                    objectKey = string.Concat(nanruiKey, "-", fieldKey, "-", fieldValue);
                    dataKey = string.Concat(result.Alias, ":", objectKey);
                }
                else if ("null" != result.Separator)
                {
                    objectKey = string.Concat(nanruiKey, result.Separator, fieldValue);
                    dataKey = string.Concat(result.Alias, ":", objectKey);
                }
                writer.WriteLine("//修正设备的性能参数 - '{0}'.", dataKey);
            }

            //SCENE	网管厂家	Betasoft(固定是这个)
            message.Body["MAINDATA"] = _encoding.GetBytes(nanruiKey);
            message.Body["SCENE"] = _encoding.GetBytes(_manufacturer);
            message.Body["CLASSNAME"] = _encoding.GetBytes(type);


            writer.Write("MAINDATA="); writer.WriteLine(nanruiKey);
            writer.Write("SCENE="); writer.WriteLine(_manufacturer);
            writer.Write("CLASSNAME="); writer.WriteLine(type);
            object performanceValue = data.GetValue();

            if (data.StatusX == "Memory")
            {
                performanceValue = GetValue(data.GetValue());
                dataKey = "MEMLOAD";
            }
            else if (data.StatusX == "Cpu")
            {
                dataKey = string.Concat("CPULOAD:", nanruiKey, "-CPU1");
                performanceValue = GetValue(data.GetValue());
            }
            else if (data.StatusX == "Used_Per")
            {
                if (result.Alias == "FileSystemUsedRate")
                {
                    performanceValue = GetValue(data.GetValue());
                }
                else if (result.Alias == "DiskFreeRate")
                {
                    performanceValue = GetValue(100 - ((PerformanceData)data).Value);
                    writer.WriteLine("//原值为使用率,需要修正 - oldValue={0}, newValue={1}", data.GetValue(), performanceValue);
                }
            }
            else if (data.StatusX == "upmins")
            {
                if (100 == (long)(((PerformanceData)data).Value))
                {
                    writer.WriteLine("//原值单位为100,可能不正确 - oldValue={0}, Id={1}", data.GetValue(), data.Id);
                    return false;
                }

                performanceValue = ((long)(((PerformanceData)data).Value * 60)).ToString();
                writer.WriteLine("//原值单位为分钟,需要修正 - oldValue={0}, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (result.Alias == "ComputerSystemRunningTime")
            {
                performanceValue = ((long)(((PerformanceData)data).Value * 60)).ToString();
                writer.WriteLine("//原值单位为分钟,需要修正 - oldValue={0}, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("Up", data.GetValue().ToString(), true))
            {
                // 此处将所有的 Up 或 Down这样的值改成 1 或 0
                performanceValue = 1;
                writer.WriteLine("//原值需要修正 - oldValue=Up, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("Down", data.GetValue().ToString(), true))
            {
                // 此处将所有的 Up 或 Down这样的值改成 1 或 0
                performanceValue = 0;
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("active", data.GetValue().ToString(), true))
            {
                performanceValue = "1";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if (0 == string.Compare("inactive", data.GetValue().ToString(), true))
            {
                performanceValue = "0";
                writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
            }
            else if ("ProcessOccupyCPUTime" == result.Alias
                || "ProcessRunningTime" == result.Alias)
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(performanceValue.ToString().Replace('-', '.'), out timeSpan))
                {
                    performanceValue = GetValue(timeSpan.TotalSeconds);
                    writer.WriteLine("//原值需要修正 - oldValue=Down, newValue={1}", data.GetValue(), performanceValue);
                }
            }

            //dataKey = dataKey.Trim();

            message.Body[dataKey] = _encoding.GetBytes(performanceValue.ToString());

            writer.Write("//PKI="); writer.WriteLine(dataKey);
            writer.Write(dataKey); writer.Write("="); writer.WriteLine(performanceValue.ToString());
            return true;
        }

        static bool createMapMessageFromPort(MapMessage message, PerformanceBase data, TextWriter writer)
        {
            Interface port = _btSystem.GetDevicePort(GetOriginalKey(data));
            if (null == port)
            {
                writer.WriteLine("//找不到端口{0}", data.MOId);
                return false;
            }

            Device dev = port.Device;
            if (null == dev)
            {
                writer.WriteLine("//找不到设备{0}", port.DeviceId);
                return false;
            }

            writer.Write("//IP="); writer.WriteLine(dev.IP);
            writer.Write("//IfIndex="); writer.WriteLine(port.IfIndex);
            bool[] sa;
            if (null != _deviceList && _deviceList.TryGetValue(dev.Id, out sa) && !(sa[0] && sa[1]))
            {
                writer.WriteLine("//对象不在上传列表中");
                return false;
            }

            string type = null;
            if (5 == dev.DeviceType)
            {
                if (!_osTypes.TryGetValue(dev.SystemType, out type))
                    type = "Computersystem";
            }
            else
            {
                _deviceTypes.TryGetValue(dev.DeviceType, out type);
            }

            if (string.IsNullOrEmpty(type))
            {
                writer.WriteLine("//设备{0}的类型不正确,不可识别 - '{1}:{2}'.", dev.Id, dev.DeviceType, dev.SystemType);
                return false;
            }


            PerformanceData performanceData = data as PerformanceData;
            if (null == performanceData)
            {
                writer.WriteLine("//设备{0}的值类型不正确.", dev.Id);
                return false;
            }

            string nanruiKey = string.Concat("OriginalKey=", _manufacturer, _prefixId, "Device-", port.DeviceId);
            message.Body["MAINDATA"] = _encoding.GetBytes(nanruiKey);
            message.Body["SCENE"] = _encoding.GetBytes(_manufacturer);
            message.Body["CLASSNAME"] = _encoding.GetBytes(type);
            writer.Write("MAINDATA="); writer.WriteLine(nanruiKey);
            writer.Write("SCENE="); writer.WriteLine(_manufacturer);
            writer.Write("CLASSNAME="); writer.WriteLine(type);

            string ifKey = string.Concat("OriginalKey=", _manufacturer, _prefixId, "IpInterface-", port.Id);
            switch (data.StatusX)
            {
                case "ifStatus":
                    {
                        string valueKey = string.Concat("IpInterfaceStatus:", ifKey);
                        string tmp = GetIfStatus(data.GetValue()).ToString();
                        message.Body[valueKey] = _encoding.GetBytes(tmp);
                        writer.Write(valueKey); writer.Write("="); writer.WriteLine(tmp);
                        writer.Write("//PKI="); writer.WriteLine("IpInterfaceStatus");
                    }
                    break;
                default:
                    {
                        string key;
                        _performanceMapForPorts.TryGetValue(data.StatusX, out key);
                        if (string.IsNullOrEmpty(key))
                        {
                            writer.WriteLine("//端口{0}的性能参数不正确,不可识别 - '{1}'.", port.Id, data.StatusX);
                            return false;
                        }
                        string valueKey = string.Concat(key, ":", ifKey);
                        writer.Write("//PKI="); writer.WriteLine(valueKey);
                        string performanceValue = GetValue(performanceData.Value);

                        switch (data.StatusX)
                        {
                            case "ifOutOctets":
                                {
                                    performanceValue = GetValue((performanceData.Value / 8));
                                    break;
                                }
                            case "ifInOctets":
                                {
                                    performanceValue = GetValue((performanceData.Value / 8));
                                    break;
                                }
                            case "ifOctets":
                                {
                                    performanceValue = GetValue((performanceData.Value / 8));
                                    break;
                                }
                        }

                        message.Body[valueKey] = _encoding.GetBytes(performanceValue);
                        writer.Write(valueKey); writer.Write("="); writer.WriteLine(performanceValue);

                        break;
                    }
            }
            return true;
        }

        static bool createMapMessageFromLink(MapMessage message, PerformanceBase data, TextWriter writer)
        {
            Link link = _btSystem.GetLink(data.MOId);
            if (null == link)
            {
                writer.WriteLine("//找不到链路连接{0}", data.MOId);
                return false;
            }


            PerformanceData performanceData = data as PerformanceData;
            if (null == performanceData)
            {
                writer.WriteLine("//链路连接{0}的值类型不正确.", data.MOId);
                return false;
            }

            writer.Write("//LinkId="); writer.WriteLine(link.Id);
            writer.Write("//LinkName="); writer.WriteLine(link.DisplayName);
            bool[] sa;
            if (null != _linkList && _linkList.TryGetValue(data.MOId, out sa) && !(sa[0] && sa[1]))
            {
                writer.WriteLine("//对象不在上传列表中");
                return false;
            }

            string nanruiKey = string.Concat("OriginalKey=", _manufacturer, _prefixId, "L2Link-", data.MOId);
            message.Body["MAINDATA"] = _encoding.GetBytes(nanruiKey);
            message.Body["SCENE"] = _encoding.GetBytes(_manufacturer);

            string className = link.IsVirtual ? "LogicalLink" : "L2Link";
            message.Body["CLASSNAME"] = _encoding.GetBytes(className);

            writer.Write("MAINDATA="); writer.WriteLine(nanruiKey);
            writer.Write("SCENE="); writer.WriteLine(_manufacturer);
            writer.WriteLine("CLASSNAME=" + className);

            switch (data.StatusX)
            {
                case "ifStatus":
                    message.Body["L2LinkStatus"] = _encoding.GetBytes(GetIfStatus(data.GetValue()));
                    writer.Write("L2LinkStatus="); writer.WriteLine(GetIfStatus(data.GetValue()));
                    writer.Write("//PKI="); writer.WriteLine("L2LinkStatus");
                    break;
                //case "ifOutOctets":
                //    message.Body["L2LinkOutFlow5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifOutNUcastPkts":
                //    message.Body["L2LinkOutBroadcastFlow5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifOutDiscards":
                //    message.Body["L2LinkOutDropPacketNum5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifOutErrors":
                //    message.Body["L2LinkOutErrorPacketNum5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifOutPercent":
                //    message.Body["L2LinkOutBwUsedRate5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifInOctets":
                //    message.Body["L2LinkInFlow5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifInUcastPkts":
                //    message.Body["L2LinkInBroadcastFlow5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifInDiscards":
                //    message.Body["L2LinkInDropPacketNum5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifInErrors":
                //    message.Body["L2LinkInErrorPacketNum5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                //case "ifInPercent":
                //    message.Body["L2LinkInBwUsedRate5MIN"] = _encoding.GetBytes(data.Value.ToString());
                //    break;
                default:
                    {
                        string key;
                        _performanceMapForLinks.TryGetValue(data.StatusX, out key);
                        if (string.IsNullOrEmpty(key))
                        {
                            writer.WriteLine("//线路{0}的性能参数不正确,不可识别 - '{1}'.", data.MOId, data.StatusX);
                            return false;
                        }

                        writer.Write("//PKI="); writer.WriteLine(key);
                        string performanceValue = GetValue(performanceData.Value);

                        switch (data.StatusX)
                        {
                            case "ifOutOctets":
                                {
                                    performanceValue = GetValue(performanceData.Value / 8);
                                    break;
                                }
                            case "ifInOctets":
                                {
                                    performanceValue = GetValue(performanceData.Value / 8);
                                    break;
                                }
                            case "ifOctets":
                                {
                                    performanceValue = GetValue(performanceData.Value / 8);
                                    break;
                                }
                        }

                        message.Body[key] = _encoding.GetBytes(performanceValue);
                        writer.Write(key); writer.Write("="); writer.WriteLine(performanceValue);
                        break;
                    }
            }

            return true;
        }

        static MapMessage createMapMessage(Session producer, PerformanceBase data, TextWriter writer)
        {
            MapMessage message = new MapMessage();
            message.Body["TIME"] = _encoding.GetBytes(DateTime.Now.ToString());
            writer.Write("TIME="); writer.WriteLine(DateTime.Now.ToString());
            writer.Write("//rawPKI="); writer.WriteLine(data.StatusX);
            writer.Write("//rawValue="); writer.WriteLine(data.GetValue());

            //CPU/MEM
            if (data.MOType == "Device")
            {
                if (createMapMessageFromDevice(message, data, writer))
                    return message;
            }
            else if (data.MOType == "DevicePort")
            {
                if (createMapMessageFromPort(message, data, writer))
                    return message;
            }
            else if (data.MOType == "TopologyLink")
            {
                if (createMapMessageFromLink(message, data, writer))
                    return message;
            }
            else if (data.MOType == "Service")
            {
                if (createMapMessageFromServer(message, data, writer))
                    return message;
            }
            else
            {
                writer.WriteLine("//无法识别的对象类型");
            }
            return null;
        }

        #endregion //Message



        static void initialize_datebase(XmlSetting xmlSetting)
        {
            string logFile = Path.Combine(_basePath, "db_error.log");
            if (File.Exists(logFile))
                File.Delete(logFile);

            string current_sqlcmd = "null";

            try
            {
                string dbPath = Path.Combine(_basePath, "..\\..\\nms\\deploy\\BTNM\\config\\db.config");


                using (DBSupport dbSupport = new DBSupport(xmlSetting.SelectOne("/configuration/DBConfig"), _basePath))
                {
                    dbSupport.GetConnection();

                    foreach (string cmdLine in File.ReadAllLines(Path.Combine(_basePath, "Nanrui." + dbSupport.DriverType + ".sql")))
                    {
                        if (null == cmdLine)
                            continue;
                        string cmd = cmdLine.Trim();
                        if (0 == cmd.Length)
                            continue;
                        if (cmd.StartsWith("--"))
                            continue;
                        current_sqlcmd = cmd;

                        if (cmd.Trim().ToUpper().StartsWith("DROP"))
                        {
                            try
                            {
                                dbSupport.ExecuteNonQuery(cmd);
                            }
                            catch
                            { }
                        }
                        else
                        {
                            dbSupport.ExecuteNonQuery(cmd);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                File.WriteAllText(logFile, string.Concat(current_sqlcmd ?? "null", "\r\n", err.ToString()));
                throw;
            }
        }

    }
}
