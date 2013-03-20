using System;
using System.Collections.Generic;
using System.Text;

namespace meijing
{
    using System.Net;
    using System.Xml.Serialization;

    /// <summary>
    /// IP网段的枚举器
    /// </summary>
    [Serializable]
    public class IPSeg //: IEnumerable<IPAddress>
    {
        private IPAddress from_;
        private IPAddress to_;

        [XmlIgnore]
        public IPAddress From
        {
            get { return from_??IPAddress.None; }
            set { from_ = value; }
        }

        [XmlIgnore]
        public IPAddress To
        {
            get { return to_ ?? From; }
            set { to_ = value; }
        }

        [XmlElement("From")]
        public string _From_
        {
            get { return From.ToString(); }
            set { From = IPAddress.Parse(value); }
        }

        [XmlElement("To")]
        public string _To_
        {
            get { return To.ToString(); }
            set { To = IPAddress.Parse(value); }
        }

        public IPSeg( )
        {
        }

        public IPSeg(KeyValuePair<IPAddress, IPAddress> seg)
        {
            From = seg.Key;
            To = seg.Value;
        }

        public IPSeg(IPAddress from, IPAddress to)
        {
            From = from;
            To = to;
        }
        public IPSeg(string from, string to)
        {
            From = IPAddress.Parse(from);
            if (!string.IsNullOrEmpty( to) )
                To = IPAddress.Parse(to);
        }


        public int Count
        {
            get
            {
                if (IsNull( from_) )
                    return 0;
                if (IsNull( to_ ))
                    return 1;
                return IPSeg.Distance(From, To);
            }
        }

        public IEnumerator<IPAddress> internalGetEnumerator()
        {
            if (!IsNull(from_))
            {
                if (IsNull(to_))
                {
                    yield return From;
                }
                else
                {
                    IPAddress from = From;
                    while (true)
                    {
                        yield return from;
                        if (IPSeg.CompareFor(from, To) >= 0)
                            break;
                        from = IPSeg.Next(from);
                    }
                }
            }
        }

        public bool In(IPAddress addr)
        {
            if (IsNull(from_))
                return false;
            if (IsNull(to_))
                return (0 == IPSeg.CompareFor(From, addr));

            if (IPSeg.CompareFor(From, addr) > 0)
                return false;
            return IPSeg.CompareFor(To, addr) >= 0;
        }

        #region IEnumerable<IPAddress> Members

        public IEnumerator<IPAddress> GetEnumerator()
        {
            return internalGetEnumerator();
        }

        #endregion

     
        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            if (this == obj) return true;

            return Equals( obj as IPSeg );
        }

        public bool Equals(IPSeg ipSeg)
        {
            if (null == ipSeg)
                return false;

            return this.From.Equals(ipSeg.From)
            && this.To.Equals(ipSeg.To);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (IsNull(this.from_) || IsNull(this.to_) || from_.Equals(to_))
                return From.ToString();
            return string.Format("{0}-{1}", From, To);
        }

        private bool IsNull( IPAddress addr )
        { 
            if( null == addr )
                return true;
            return addr == IPAddress.None;
        }

        public static bool In(IPAddress address, IPSeg[] segments)
        {
            foreach (IPSeg seg in segments)
            {
                if (seg.In(address))
                    return true;
            }

            return false;
        }

        #region 运算

        static public int CompareFor(byte[] x, byte[] y)
        {
            int xCount = x != null ? x.Length : 0;
            int yCount = y != null ? y.Length : 0;
            int xi, yi;
            for (xi = 0, yi = 0; xi < xCount && yi < yCount; xi++, yi++)
            {
                if (x[xi] == y[yi])
                    continue;
                if (x[xi] < y[yi])
                    return -1;
                if (x[xi] > y[yi])
                    return 1;
            }
            return xCount == yCount ? 0 : (xCount > yCount ? 1 : -1);
        }

        static public int CompareFor(IPAddress x, IPAddress y)
        {
            if ((x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                   y.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
               || (x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
                   y.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6))
            {
                return IPSeg.CompareFor(x.GetAddressBytes(), y.GetAddressBytes());



            }

            throw new Exception("IP地址类型必须是IPv4或者IPv6才能进行比较");

        }

        static private byte[] Plus(byte[] bs, byte value)
        {
            if (value == 0)
                return bs;

            int length = bs.Length;
            byte[] newvalue = new byte[length];
            bs.CopyTo(newvalue, 0);
            newvalue[length - 1] += value;

            int i;
            for (i = length - 2; i >= 0; i--)
            {
                if (newvalue[i + 1] > bs[i + 1])
                    break;

                newvalue[i] += 1;
            }

            return newvalue;
        }

        static private byte[] Minus(byte[] bs, byte value)
        {
            if (value == 0)
                return bs;

            int length = bs.Length;
            byte[] newvalue = new byte[length];
            bs.CopyTo(newvalue, 0);
            newvalue[length - 1] -= value;

            int i;
            for (i = length - 2; i >= 0; i--)
            {
                if (newvalue[i + 1] < bs[i + 1])
                    break;

                newvalue[i] -= 1;
            }

            return newvalue;

        }


        static public IPAddress Prev(IPAddress x)
        {
            byte[] New = Minus(x.GetAddressBytes(), 1);
            string ni = New.Length == 4 ? string.Format("{0}.{1}.{2}.{3}", New[0], New[1], New[2], New[3])
                                        : string.Format("{0:x2}{1:x2}:{2:x2}{3:x2}:{4:x2}{5:x2}:{6:x2}{7:x2}:{8:x2}{9:x2}:{10:x2}{11:x2}:{12:x2}{13:x2}:{14:x2}{15:x2}",
                                                         New[0], New[1], New[2], New[3], New[4], New[5], New[6], New[7],
                                                         New[8], New[9], New[10], New[11], New[12], New[13], New[14], New[15]);

            return IPAddress.Parse(ni);
        }

        static public IPAddress Next(IPAddress x)
        {
            byte[] New = Plus(x.GetAddressBytes(), 1);
            string ni = New.Length == 4 ? string.Format("{0}.{1}.{2}.{3}", New[0], New[1], New[2], New[3])
                                        : string.Format("{0:x2}{1:x2}:{2:x2}{3:x2}:{4:x2}{5:x2}:{6:x2}{7:x2}:{8:x2}{9:x2}:{10:x2}{11:x2}:{12:x2}{13:x2}:{14:x2}{15:x2}",
                                                         New[0], New[1], New[2], New[3], New[4], New[5], New[6], New[7],
                                                         New[8], New[9], New[10], New[11], New[12], New[13], New[14], New[15]);

            return IPAddress.Parse(ni);
        }

        static public IPAddress Plus(IPAddress x, byte y)
        {
            byte[] New = Plus(x.GetAddressBytes(), y);
            string ni = New.Length == 4 ? string.Format("{0}.{1}.{2}.{3}", New[0], New[1], New[2], New[3])
                                        : string.Format("{0:x2}{1:x2}:{2:x2}{3:x2}:{4:x2}{5:x2}:{6:x2}{7:x2}:{8:x2}{9:x2}:{10:x2}{11:x2}:{12:x2}{13:x2}:{14:x2}{15:x2}",
                                                         New[0], New[1], New[2], New[3], New[4], New[5], New[6], New[7],
                                                         New[8], New[9], New[10], New[11], New[12], New[13], New[14], New[15]);

            return IPAddress.Parse(ni);
        }

        static public IPAddress Minus(IPAddress x, byte y)
        {
            byte[] New = Minus(x.GetAddressBytes(), y);
            string ni = New.Length == 4 ? string.Format("{0}.{1}.{2}.{3}", New[0], New[1], New[2], New[3])
                                        : string.Format("{0:x2}{1:x2}:{2:x2}{3:x2}:{4:x2}{5:x2}:{6:x2}{7:x2}:{8:x2}{9:x2}:{10:x2}{11:x2}:{12:x2}{13:x2}:{14:x2}{15:x2}",
                                                         New[0], New[1], New[2], New[3], New[4], New[5], New[6], New[7],
                                                         New[8], New[9], New[10], New[11], New[12], New[13], New[14], New[15]);

            return IPAddress.Parse(ni);
        }

        static ulong GetValue(byte[] x)
        {
            ulong ret = 0;
            for (int i = 0; i < x.Length; i++)
                ret = ret * 256 + x[i];
            return ret;
        }

        static int Distance(byte[] x, byte[] y)
        {

            ulong x1 = GetValue(x);
            ulong y1 = GetValue(y);
            return (int)(y1 - x1 + 1);
        }

        static public int Distance(IPAddress x, IPAddress y)
        {
            return IPSeg.Distance(x.GetAddressBytes(), y.GetAddressBytes());
        }

        #endregion

        #region 位运算

        public delegate byte Operate(byte x, byte y);

        private static System.Net.IPAddress BitOperate(System.Net.IPAddress x, System.Net.IPAddress mask, Operate op)
        {
            byte[] xx = x.GetAddressBytes();
            byte[] mm = mask.GetAddressBytes();
            if (xx.Length != mm.Length)
                throw new ApplicationException("IP地址版本不一致，不能执行“或”操作");
            int i = 0;

            for (i = 0; i < xx.Length; i++)
                xx[i] = op(xx[i], mm[i]);
            return new IPAddress(xx);
        }

        public static System.Net.IPAddress And(System.Net.IPAddress x, System.Net.IPAddress mask)
        {
            return BitOperate(x, mask, delegate(byte bx, byte by) { return (byte)(bx & by); });
        }
        public static System.Net.IPAddress Or(System.Net.IPAddress x, System.Net.IPAddress mask)
        {
            return BitOperate(x, mask, delegate(byte bx, byte by) { return (byte)(bx | by); });
        }

        public static System.Net.IPAddress Not(System.Net.IPAddress mask)
        {
            byte[] mm = mask.GetAddressBytes();

            for (int i = 0; i < mm.Length; i++)
                mm[i] = (byte)(~mm[i]);
            return new System.Net.IPAddress(mm);

        }

        #endregion
    }
}
