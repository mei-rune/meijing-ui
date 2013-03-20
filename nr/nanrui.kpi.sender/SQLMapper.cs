using System;
using System.Collections.Generic;
using System.Text;

namespace Betanetworks.Nanrui.Server
{
    public enum SQLDialect : int
    {
        SELECT_QUEUE,
        DELETE_QUEUE_BY_TIME,
        COUNT_QUEUE,
        DELETE_QUEUE,

        SELECT_MAP,
        DELETE_MAP_TIMEOUT,
        SELECT_MAP_SEND_TIMEOUT,
        UPDATE_MAP_SEND_TIME,
        UPDATE_MAP,
        INSERT_MAP,
        DELETE_MAP_NOT_IN_HISTORY,

        SELECT_DEVICE,
        SELECT_PORT,
        SELECT_LINK,
        SELECT_DEVICEIP,
        MAX
    }

    public class SQLMapper
    {
        static string[] sqlList = new string[(int)SQLDialect.MAX+1];

        public static void Initialize( XmlSetting config )
        {
            sqlList[(int)SQLDialect.SELECT_QUEUE] = "select * from BHistoryReadySend order by BCreatedTime";
            sqlList[(int)SQLDialect.DELETE_QUEUE_BY_TIME] = "delete from BHistoryReadySend where BID <= {0}";
            sqlList[(int)SQLDialect.COUNT_QUEUE] = "select count(*) from BHistoryReadySend";
            sqlList[(int)SQLDialect.DELETE_QUEUE] = "delete from BHistoryReadySend";

            sqlList[(int)SQLDialect.SELECT_MAP] = "select * from BHistorySendMap";
            sqlList[(int)SQLDialect.DELETE_MAP_TIMEOUT] = "delete from BHistorySendMap where ( {0} - BCreatedTime ) > 3000 ";
            sqlList[(int)SQLDialect.SELECT_MAP_SEND_TIMEOUT] = "select * from BHistorySendMap where ( {0} - BSendTime ) > 300 ";
            sqlList[(int)SQLDialect.UPDATE_MAP_SEND_TIME] = "update BHistorySendMap set BSendTime={0} where ( {0} - BSendTime ) > 300 ";
            sqlList[(int)SQLDialect.UPDATE_MAP] = "update BHistorySendMap set BCreatedTime=@1,BValue=@2 where BStatus=@3 and BStoreId=@4";
            sqlList[(int)SQLDialect.INSERT_MAP] = "insert into BHistorySendMap(BCreatedTime,BValue,BStatus,BStoreId,BMOId,BMOtype,BSendTime) values(@21,@22,@23,@24,@25,@26,0)";

            sqlList[(int)SQLDialect.DELETE_MAP_NOT_IN_HISTORY] = "delete from BHistorySendMap where BStoreId not in ( select BStoreId from BHistoryTarget )";
            sqlList[(int)SQLDialect.SELECT_DEVICE] = "select BOwnerId,BIP,BName,BDevType from DeviceStaticProperties";
            sqlList[(int)SQLDialect.SELECT_PORT] = "select BId,BDeviceId,BIfIndex, BIfMAC from PortStaticProperties";
            sqlList[(int)SQLDialect.SELECT_LINK] = "select BOwnerId,BDevice1,BPort1,BDevice2,BPort2,BName from LinkStaticProperties";
            sqlList[(int)SQLDialect.SELECT_DEVICEIP] = "select BOwnerId,BIP,BifIndex from DeviceIP";

            if (null != config)
            {
                foreach (int i in Enum.GetValues(typeof(SQLDialect)))
                {
                    string sql = config.ReadSetting(Enum.GetName(typeof(SQLDialect), i) + "/text()", null);
                    if (!string.IsNullOrEmpty(sql))
                        sqlList[i] = sql;
                }
            }
        }

        public static string GetSqlString(SQLDialect dialect)
        {
            return sqlList[(int)dialect];
        }


        public static string GetSqlString(SQLDialect dialect, string defaultValue)
        {
            return sqlList[(int)dialect] ?? defaultValue;
        }
    }
}
