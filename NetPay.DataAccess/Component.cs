using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.DataAccess
{
    public class Component
    {
        public string key { get; set; }
        public string name { get; set; }
        public string qualifier { get; set; }
        public List<Measure2> measures { get; set; }
        public string branch { get; set; }
    }

    public class Measure2
    {
        public string metric { get; set; }
        public string value { get; set; }
        public bool? bestValue { get; set; }
        public List<Period2> periods { get; set; }
        public Period3 period { get; set; }
    }

    public class Metric2
    {
        public string key { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string domain { get; set; }
        public string type { get; set; }
        public bool higherValuesAreBetter { get; set; }
        public bool qualitative { get; set; }
        public bool hidden { get; set; }
        public string bestValue { get; set; }
        public int? decimalScale { get; set; }
        public string worstValue { get; set; }
    }

    public class Period2
    {
        public int index { get; set; }
        public string value { get; set; }
        public bool? bestValue { get; set; }
    }

    public class Period3
    {
        public int index { get; set; }
        public string value { get; set; }
        public bool? bestValue { get; set; }
        public string mode { get; set; }
        public string date { get; set; }
    }

    public class RootComponent
    {
        public Component component { get; set; }
        public List<Metric2> metrics { get; set; }
        public Period period { get; set; }
    }
}
