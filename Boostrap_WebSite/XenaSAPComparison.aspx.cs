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

public partial class XenaSAPComparison : System.Web.UI.Page
{
    List<User> LoginUser = new List<User>();
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();

    ReportDocument rdoc = new ReportDocument();
    bool validateFilter = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AllCashReportLogin"] != null)
        {
            UserList = (Dictionary<string, bool>)Session["AllCashReportLogin"];

            if (UserList.First().Value.ToString() == "0")
            {
                //Session["AllCashReportLogin"] = "0";
                Response.Redirect("~/index.aspx");
            }

            //if (UserList.First().Key.Contains("acc"))
            //{
            //    Response.Redirect("~/AllCashReport_V2.aspx");
            //}
        }
        if (Session["AllCashReportLogin"] == null)
        {
            //UserList.Add(username, false);

            //Session["AllCashReportLogin"] = "0";
            Response.Redirect("~/index.aspx");
        }

        if (!IsPostBack)
        {
            InitPage();

        }
    }

    private void InitPage()
    {
        Dictionary<string, string> branchList = new Dictionary<string, string>();

        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("proc_XenaSapComparison_GetBranch", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                branchList.Add("-1", "---All---");
                while (reader.Read())
                {
                    branchList.Add(reader["RowNum"].ToString(), reader["PrcName"].ToString());
                }

                cn.Close();
            }

            ddlBranch.Items.Clear();
            ddlBranch.DataSource = branchList;
            ddlBranch.DataTextField = "Value";
            ddlBranch.DataValueField = "Key";
            ddlBranch.DataBind();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw;
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenReport();
    }

    private void GenReport()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    string dateFrom = "";
                    string dateTo = "";

                    var _dateFrom = txtStartDate.Text.Split('/').ToList();
                    dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

                    var _dateTo = txtEndDate.Text.Split('/').ToList();
                    dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);

                    var branchName = ddlBranch.SelectedItem.Text == "---All---" ? "" : ddlBranch.SelectedItem.Text;

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("proc_XenaSapComparison_GetData", cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DocDateFrom", dateFrom));
                    cmd.Parameters.Add(new SqlParameter("@DocDateTo", dateTo));
                    cmd.Parameters.Add(new SqlParameter("@PrcName", branchName));
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "XenaSapComparisonReport");

                    rdoc.Load(Server.MapPath("XenaSapComparisonReport.rpt"));

                    rdoc.SetDataSource(ds.Tables["XenaSapComparisonReport"]);

                    rdoc.SetParameterValue("@DocDateFrom", dateFrom);
                    rdoc.SetParameterValue("@DocDateTo", dateTo);
                    rdoc.SetParameterValue("@PrcName", branchName);

                    CrystalReportViewer1.RefreshReport();

                    CrystalReportViewer1.ReportSource = rdoc;

                    CrystalReportViewer1.DataBind();

                    cn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }
}