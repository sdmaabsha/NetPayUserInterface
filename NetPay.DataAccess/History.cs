using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPay.DataAccess
{
    public class History
    {
        public string date { get; set; }
        public string value { get; set; }
    }

    public class Measure
    {
        public string metric { get; set; }
        public List<History> history { get; set; }
    }

    public class Paging
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
    }

    public class RootHistory
    {
        public Paging paging { get; set; }
        public List<Measure> measures { get; set; }
    }
}
