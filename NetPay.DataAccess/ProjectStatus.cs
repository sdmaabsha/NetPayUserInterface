using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.DataAccess
{
    public class Condition
    {
        public string status { get; set; }
        public string metricKey { get; set; }
        public string comparator { get; set; }
        public int periodIndex { get; set; }

        public string periodIndexValue { get; set; }
        public string errorThreshold { get; set; }
        public string actualValue { get; set; }
    }

    public class Period
    {
        public int index { get; set; }
        public string mode { get; set; }
        public string date { get; set; }
    }

    public class ProjectStatus
    {
        public string status { get; set; }
        public List<Condition> conditions { get; set; }
        public List<Period> periods { get; set; }
        public bool ignoredConditions { get; set; }
        public Period period { get; set; }
        public string caycStatus { get; set; }

        public string respository { get; set; }
    }

    public class Root
    {
        public ProjectStatus projectStatus { get; set; }
    }
}
