﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CustInfoEntities : DbContext
    {
        public CustInfoEntities()
            : base("name=CustInfoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<SalArea> SalAreas { get; set; }
        public virtual DbSet<tblAreaToDistrict> tblAreaToDistricts { get; set; }
        public virtual DbSet<tblBranchToProvince> tblBranchToProvinces { get; set; }
        public virtual DbSet<tblCustomerAM2> tblCustomerAM2 { get; set; }
        public virtual DbSet<tblCustomerW> tblCustomerWS { get; set; }
        public virtual DbSet<tblProvince> tblProvinces { get; set; }
        public virtual DbSet<tblProvinceToArea> tblProvinceToAreas { get; set; }
        public virtual DbSet<tblSalAreaToProvince> tblSalAreaToProvinces { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<tblArea> tblAreas { get; set; }
        public virtual DbSet<tblDistrict> tblDistricts { get; set; }
    }
}
