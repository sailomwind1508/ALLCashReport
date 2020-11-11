using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Report_ClassLibrary
{
    public class CustInfo
    {
        public string custtomerID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string custImg { get; set; }

        public int rowCount { get; set; }
    }
}
