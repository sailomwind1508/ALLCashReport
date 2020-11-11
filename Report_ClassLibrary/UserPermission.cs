using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Report_ClassLibrary
{
    public class UserPermission
    {
        public int PerPK { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserPK { get; set; }
        public string PageName { get; set; }
        public string PageDesc { get; set; }
    }
}
