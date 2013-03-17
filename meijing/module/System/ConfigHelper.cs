using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace meijing.ui.module
{
    [Serializable]
    public class ConfigHelper
    {
        /// <summary>
        /// 服务器类型
        /// </summary>
        public enum SvrRoleType
        {
            /// <summary>
            /// 数据服务器[mongod]
            /// </summary>
            DataSvr,
            /// <summary>
            /// Sharding服务器[mongos]
            /// </summary>
            ShardSvr,
            /// <summary>
            /// 副本服务器[Virtul]
            /// </summary>
            ReplsetSvr,
            /// <summary>
            /// Master主服务器
            /// </summary>
            MasterSvr,
            /// <summary>
            /// Slave从属服务器
            /// </summary>
            SlaveSvr
            /// <summary>
            /// 配置服务器[mongod]
            /// </summary>
            ///ConfigSvr,
            /// <summary>
            /// 仲裁服务器[mongod --replset,without data]
            /// </summary>
            ///ArbiterSvr,
        }


        /// <summary>
        /// Config Format Version
        /// </summary>
        public byte ConfigVer = 1;
        /// <summary>
        /// MongoBin的路径，用于Dos命令
        /// </summary>
        public String MongoBinPath = String.Empty;
        /// <summary>
        /// 状态刷新间隔时间
        /// </summary>
        public int RefreshStatusTimer = 30;
        /// <summary>
        /// 语言
        /// </summary>
        public String LanguageFileName = String.Empty;
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public static String _configFilename = "config.xml";
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public static ConfigHelper LoadFromConfigFile(String configFileName)
        {
            FileStream fs = new FileStream(configFileName, FileMode.Open, FileAccess.Read);
            XmlSerializer xs = new XmlSerializer(typeof(ConfigHelper));
            ConfigHelper t = (ConfigHelper)xs.Deserialize(fs);
            fs.Close();
            _configFilename = configFileName;
            return t;
        }
        /// <summary>
        /// 写入配置
        /// </summary>
        public void SaveToConfigFile()
        {
            SaveToConfigFile(_configFilename);
        }
        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="configFileName"></param>
        public void SaveToConfigFile(String configFileName)
        {
            FileStream fs = null;
            XmlSerializer xs = new XmlSerializer(typeof(ConfigHelper));
            fs = new FileStream(configFileName, FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, this);
            fs.Close();
        }
    }
}
