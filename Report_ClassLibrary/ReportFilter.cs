using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Report_ClassLibrary
{
    public class ReportFilter
    {
        public string branch { get; set; }
        public string van { get; set; }
        public string saleArea { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string reportType { get; set; }
        public string customerStatus { get; set; }
    }
}
