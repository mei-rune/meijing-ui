using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace meijing.ui.module
{
    public class Metric
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public override string ToString()
        {
            return Display;
        }

        public static Metric[] DEVICES = new Metric[] {
            new Metric{Name="cpu", Display="CPU"},
            new Metric{Name="mem", Display="MEM"},
            new Metric{Name="sys.upTime", Display="连接运行时间"}
        };
        public static Metric[] INTERFACES = new Metric[] {
            new Metric{Name="interface", Display="流量"}
        };
        public static Metric[] LINKS = new Metric[] {
            new Metric{Name="interface", Display="流量"}
        };
    }
}
