//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerInfo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSalAreaToProvince
    {
        public int ID { get; set; }
        public string BranchID { get; set; }
        public string slpCode { get; set; }
        public string SalAreaID { get; set; }
        public string ProvinceID { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual SalArea SalArea { get; set; }
        public virtual tblProvince tblProvince { get; set; }
    }
}
