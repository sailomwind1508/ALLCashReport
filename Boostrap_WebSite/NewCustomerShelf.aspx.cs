using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;

public partial class NewCustomerShelf : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();
    ReportDocument rdoc2 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string conString8 = ConfigurationManager.ConnectionStrings["myConnectionString_8"].ConnectionString;
    Dictionary<string, string> permissionList = new Dictionary<string, string>();
    static List<VanDetails> vanList = new List<VanDetails>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AllCashReportLogin"] != null)
        {
            UserList = (Dictionary<string, bool>)Session["AllCashReportLogin"];

            if (UserList.First().Value.ToString() == "0")
            {
                Response.Redirect("~/index.aspx");
            }
        }
        if (Session["AllCashReportLogin"] == null)
        {
            Response.Redirect("~/index.aspx");
        }

        if (!IsPostBack)
        {
            InitPage();
        }

        rdoc1 = new ReportDocument();
        rdoc2 = new ReportDocument();
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            rdoc1 = new ReportDocument();

            rdoc1.Close();
            rdoc1.Dispose();

            rdoc2 = new ReportDocument();

            rdoc2.Close();
            rdoc2.Dispose();

            GC.Collect();


        }
        catch
        {

        }
    }

    private void InitPage()
    {
        var _date = DateTime.Now;
        Dictionary<string, string> branchList = new Dictionary<string, string>();

        Dictionary<string, string> custTypeList = new Dictionary<string, string>();
        custTypeList.Add("All", "---All---");
        custTypeList.Add("NewCustomer", "NewCustomer");
        custTypeList.Add("OldCustomer", "OldCustomer");
        ddlCustType.BindDropdownList(custTypeList);

        //ddlMonth.PrepareMonthDropdown();

        //ddlYear.PrepareYearDropdown();

        ddlMonth_2.PrepareMonthDropdown();

        ddlYear_2.PrepareYearDropdown();

        DataTable dt = new DataTable();
        dt = Helper.ExecuteProcedureToTable(conString, "proc_MonthlySalesSumReport_GetBranch", null);
        DataRow cRow = dt.NewRow();
        cRow["BranchID"] = "-1";
        cRow["BranchName"] = "---All---";
        dt.Rows.InsertAt(cRow, 0);

        //ddlBranch.BindDropdownList(dt, "BranchName", "BranchID");
        //ddlBranch.SelectedIndex = 0;

        ddlBranch_2.BindDropdownList(dt, "BranchName", "BranchID");
        ddlBranch_2.SelectedIndex = 0;

        //ddlVan.Items.Clear();
        ddlVan_2.Items.Clear();

        DataTable dt2 = new DataTable();
        string cmd = "";
        cmd += " SELECT DISTINCT RIGHT(WHCode, 3) AS 'VAN_ID', RIGHT(WHCode, 3) AS 'VAN_CODE' FROM [dbo].[BranchWarehouse] WHERE RIGHT(WHCode, 3) LIKE 'V%'";
        dt2 = Helper.ExecuteProcedureToTable(conString8, cmd, CommandType.Text, null);

        //DataView dv = new DataView(dt2);
        //dv.RowFilter = "BranchID = " + ddlBranch.SelectedValue;

        //dt2 = dv.ToTable();

        DataRow cRow2 = dt2.NewRow();
        cRow2["VAN_ID"] = "-1";
        cRow2["VAN_CODE"] = "---All---";
        dt2.Rows.InsertAt(cRow2, 0);

        //ddlVan.BindDropdownList(dt2, "VAN_CODE", "VAN_ID");
        //ddlVan.SelectedIndex = 0;

        ddlVan_2.BindDropdownList(dt2, "VAN_CODE", "VAN_ID");
        ddlVan_2.SelectedIndex = 0;
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        //int _month = Convert.ToInt32(ddlMonth.SelectedValue);
        //int _year = Convert.ToInt32(ddlYear.SelectedValue);
        //string branchID = ddlBranch.SelectedValue;
        //string vanID = ddlVan.SelectedValue;
        //GenPatternReport(rdoc1, CrystalReportViewer1, _month, _year, branchID, vanID, "proc_new_customer_shelf_get", "NewCustomerShelf.rpt");
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {

    }

    private void GenPatternReport(ReportDocument rdoc, CrystalDecisions.Web.CrystalReportViewer crys, int month, int year, string branchID, string vanID, string storeName, string reportName)
    {
        ViewState["CrystalReport_Data"] = null;
        try
        {
            Dictionary<string, object> _p = new Dictionary<string, object>();

            _p.Add("Month", month);
            _p.Add("Year", year);
            _p.Add("BrandID", branchID);
            _p.Add("VanID", vanID);
            if (crys.ID == "CrystalReportViewer2")
            {
                _p.Add("CustType", ddlCustType.SelectedValue);
            }

            DataTable dt = new DataTable();
            dt = Helper.ExecuteProcedureToTable(conString8, storeName, _p);

            if (dt != null && dt.Rows.Count > 0)
            {
                ViewState["CrystalReport_Data"] = dt;
                rdoc.Load(Server.MapPath(reportName));

                rdoc.SetDataSource(dt);

                //rdoc1.SetParameterValue("Month", branchID);

                crys.RefreshReport();

                crys.ReportSource = rdoc;

                crys.DataBind();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSearch_2_Click(object sender, EventArgs e)
    {

    }

    protected void CrystalReportViewer2_Load(object sender, EventArgs e)
    {
        int _month = Convert.ToInt32(ddlMonth_2.SelectedValue);
        int _year = Convert.ToInt32(ddlYear_2.SelectedValue);
        string branchID = ddlBranch_2.SelectedValue;
        string vanID = ddlVan_2.SelectedValue;
        GenPatternReport(rdoc2, CrystalReportViewer2, _month, _year, branchID, vanID, "proc_customer_shelf_get", "CustomerShelf.rpt");
    }

    protected void CrystalReportViewer2_Unload(object sender, EventArgs e)
    {

    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["CrystalReport_Data"] != null)
            {
                int _month = Convert.ToInt32(ddlMonth_2.SelectedValue);
                int _year = Convert.ToInt32(ddlYear_2.SelectedValue);
                string _branchName = ddlBranch_2.SelectedValue == "-1" ? "AllBranch" : ddlBranch_2.SelectedValue.ToString();
                string docDateTemp = string.Join("-", _year.ToString(), _month.ToString(), "01");
                string docDate = Convert.ToDateTime(docDateTemp).ToString("MMM-yyyy");

                DataTable _dt = (DataTable)ViewState["CrystalReport_Data"];
                var readDT = new DataTable();
                readDT.Columns.Add("เดือน", typeof(string));
                readDT.Columns.Add("ปี", typeof(string));
                readDT.Columns.Add("รหัสสาขา", typeof(string));
                readDT.Columns.Add("สาขา", typeof(string));
                readDT.Columns.Add("รหัสร้านค้า", typeof(string));
                readDT.Columns.Add("ร้านค้า", typeof(string));
                readDT.Columns.Add("ที่อยู่", typeof(string));
                readDT.Columns.Add("ตำบล", typeof(string));
                readDT.Columns.Add("อำเภอ", typeof(string));
                readDT.Columns.Add("จังหวัด", typeof(string));
                readDT.Columns.Add("รหัสแวน", typeof(string));
                readDT.Columns.Add("Latitude", typeof(string));
                readDT.Columns.Add("Longitude", typeof(string));
                readDT.Columns.Add("Shelf", typeof(string));
                readDT.Columns.Add("Bill", typeof(string));
                readDT.Columns.Add("*NAmt", typeof(string));
                readDT.Columns.Add("ประเภทรายงาน", typeof(string));

                foreach (DataRow r in _dt.Rows)
                {
                    readDT.Rows.Add(r["MonthIn"].ToString(), r["YearIn"].ToString(), r["BranchID"].ToString(), r["BranchName"].ToString(), r["CustomerID"].ToString()
                        , r["CustName"].ToString(), r["AddressNo"].ToString(), r["DistrictName"].ToString(), r["AreaName"].ToString(), r["ProvinceName"].ToString()
                        , r["VanID"].ToString(), r["Latitude"].ToString(), r["Longitude"].ToString(), r["Shelf"].ToString(), r["Bill"].ToString(), r["NAmt"].ToString()
                        , r["CustType"].ToString());
                }

                DataView dv = readDT.DefaultView;
                dv.Sort = "ประเภทรายงาน, รหัสแวน asc";
                DataTable sortedDT = dv.ToTable();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(sortedDT, docDate);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Customer_Shelf_Report_" + _branchName + "_" + docDate + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}