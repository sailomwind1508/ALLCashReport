using CrystalDecisions.CrystalReports.Engine;
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

public partial class RefreshTransferStockReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    List<User> LoginUser = new List<User>();

    ReportDocument rdoc = new ReportDocument();

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
            
        }

    }

    protected void linkRefreshTransferStock_Click(object sender, EventArgs e)
    {

        int ret = 0;
        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("proc_TransferStockDaily_PrepareData", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                ret = cmd.ExecuteNonQuery();

                cn.Close();

                if (ret != 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showRefreshAlertMsg();", true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RefreshPage();", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showRefreshErrAlertMsg();", true);
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenReport();
    }

    private void GenReport()
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();

                string dateFrom = "";
                string dateTo = "";

                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var _dateFrom = firstDayOfMonth.AddMonths(-1).ToString("MM/dd/yyyy").Split('/').ToList();
                dateFrom = firstDayOfMonth.AddMonths(-1).ToString("MM/dd/yyyy"); // string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

                var _dateTo = lastDayOfMonth.ToString("MM/dd/yyyy").Split('/').ToList();
                dateTo = lastDayOfMonth.ToString("MM/dd/yyyy"); // string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);

                SqlCommand cmd = null;
                cmd = new SqlCommand("proc_TransferStockDaily_GetData", cn);
                cmd.CommandTimeout = 0;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BranchID", ""));
                cmd.Parameters.Add(new SqlParameter("@DocDateFrom", dateFrom));
                cmd.Parameters.Add(new SqlParameter("@DocDateTo", dateTo));
                cmd.Parameters.Add(new SqlParameter("@UpdateDataFlag", 1));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "TransferStockDailyReport");

                DataTable dt = new DataTable();
                dt = ds.Tables["TransferStockDailyReport"];
                //dt.DefaultView.Sort = "PO asc, ItemCode asc";
                //dt = dt.DefaultView.ToTable();

                rdoc.Load(Server.MapPath("TransferStockDailyReport.rpt"));

                rdoc.SetDataSource(dt);

                rdoc.SetParameterValue("@BranchID", "");
                rdoc.SetParameterValue("@DocDateFrom", dateFrom);
                rdoc.SetParameterValue("@DocDateTo", dateTo);

                CrystalReportViewer1.RefreshReport();

                CrystalReportViewer1.ReportSource = rdoc;

                CrystalReportViewer1.DataBind();

                cn.Close();
            }


        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

}