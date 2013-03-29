using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Apache.NMS;

namespace meijing.nanrui.kpi.sender
{
    [XmlType("PerformanceBase")]
    public abstract class PerformanceBase
    {
        private int id;
        private int _moid;
        private string _status;
        private string _moType;
        private int _storeId;
        private long createdTime;
        private long sendTime;

        public long SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        public long CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlElement("Status")]
        public string StatusX
        {
            get { return _status; }
            set { _status = value; }
        }

        [XmlElement("StoreId")]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        [XmlElement("MOId")]
        public int MOId
        {
            get { return _moid; }
            set { _moid = value; }
        }

        [XmlElement("MOType")]
        public string MOType
        {
            get { return _moType; }
            set { _moType = value; }
        }

        public abstract object GetValue();
    }

    [XmlType("StatusData")]
    public class PerformanceData : PerformanceBase
    {
        private double _value;

        [XmlElement("Value")]
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }


        public override object GetValue()
        {
            return _value;
        }
    }



    [XmlType("StatusData2")]
    public class PerformanceData2 : PerformanceBase
    {
        private string _value;

        [XmlElement("Value")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public override object GetValue()
        {
            return _value;
        }
    }
}
