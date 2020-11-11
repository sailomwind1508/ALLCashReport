using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Report_ClassLibrary
{
    public class AllCashDailyPL
    {
        public int PK { get; set; }

        public DateTime DocDate { get; set; }

        public string BranchID { get; set; }

        public string VANID { get; set; }

        public string VersionID { get; set; }

        public int AccountingID { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
