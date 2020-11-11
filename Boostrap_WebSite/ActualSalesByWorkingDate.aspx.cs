using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActualSalesByWorkingDate : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString_8"].ConnectionString;
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
        try
        {
            ddlMonth_F.PrepareMonthDropdown();
            ddlMonth_T.PrepareMonthDropdown();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }


    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenReport();
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

    }

    private void GenReport()
    {
        try
        {
            Dictionary<string, object> _p = new Dictionary<string, object>();

            int from_month = Convert.ToInt32(ddlMonth_F.SelectedValue);
            int to_month = Convert.ToInt32(ddlMonth_T.SelectedValue);

            _p.Add("Month_F", from_month);
            _p.Add("Month_T", to_month);

            DataTable dt = new DataTable();
            dt = Helper.ExecuteProcedureToTable(conString, "proc_actualsales_by_working", _p);

            if (dt != null && dt.Rows.Count > 0)
            {
                rdoc1.Load(Server.MapPath("ActualSales_ByWorkingDate - Step 400 Baht.rpt"));

                rdoc1.SetDataSource(dt);

                //rdoc1.SetParameterValue("Month", branchID);

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