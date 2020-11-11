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

public partial class StockRemainingReport : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    Dictionary<string, string> permissionList = new Dictionary<string, string>();

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
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            rdoc1 = new ReportDocument();

            rdoc1.Close();
            rdoc1.Dispose();

            GC.Collect();
        }
        catch
        {

        }
    }

    private void InitPage()
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> b_prmt = new Dictionary<string, object>();

        b_prmt.Add("BranchID", "-1");

        _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);

        DataRow sRow = _dt.NewRow();
        sRow["BranchID"] = "-1";
        sRow["BranchName"] = "---ทั้งหมด---";
        _dt.Rows.InsertAt(sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlSalesArea.BindDropdownList(_dt, "BranchName", "BranchID");
            ddlSalesArea.SelectedIndex = 0;
        }
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenPattern1Report();
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {

    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

    }

    private void GenPattern1Report()
    {
        try
        {
            string branchID = ddlSalesArea.SelectedValue.ToString();

            Dictionary<string, object> _p = new Dictionary<string, object>();
            _p.Add("BranchID", branchID);

            DataTable dt = new DataTable();
            dt = Helper.ExecuteProcedureToTable(conString, "proc_stockRemainning_report_get", _p);

            if (dt != null && dt.Rows.Count > 0)
            {
                rdoc1.Load(Server.MapPath("StockRemainingAll2DC.rpt"));

                rdoc1.SetDataSource(dt);

                rdoc1.SetParameterValue("BranchID", branchID);

                CrystalReportViewer1.RefreshReport();

                CrystalReportViewer1.ReportSource = rdoc1;

                CrystalReportViewer1.DataBind();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }
}