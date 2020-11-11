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

public partial class SalesPromotionReport : System.Web.UI.Page
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
        Dictionary<string, string> reportTypeList = new Dictionary<string, string>();
        reportTypeList.Add("1", "รายงานแบบ step 150");
        reportTypeList.Add("2", "รายงานแบบ step 400");

        ddlReportType.BindDropdownList(reportTypeList);

        ddlMonth.PrepareMonthDropdown();

        ddlYear.PrepareYearDropdown();
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        int _month = Convert.ToInt32(ddlMonth.SelectedValue);
        int _year = Convert.ToInt32(ddlYear.SelectedValue);

        if (ddlReportType.SelectedValue == "1")
        {
            GenPatternReport(rdoc1, CrystalReportViewer1, _month, _year, "proc_sales_promotion_get", "Actual Sales By Promotion Bill Type.rpt");
        }
        else
        {
            GenPatternReport(rdoc1, CrystalReportViewer1, _month, _year, "proc_sales_promotion_get_400", "Actual Sales By Promotion Bill Type.rpt");
        }

    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {

    }


    private void GenPatternReport(ReportDocument rdoc, CrystalDecisions.Web.CrystalReportViewer crys, int month, int year, string storeName, string reportName)
    {
        try
        {
            Dictionary<string, object> _p = new Dictionary<string, object>();

            _p.Add("Month", month);
            _p.Add("Year", year);

            DataTable dt = new DataTable();
            dt = Helper.ExecuteProcedureToTable(conString, storeName, _p);

            if (dt != null && dt.Rows.Count > 0)
            {
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
}