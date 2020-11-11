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

public partial class TransferStockDaily : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    List<User> LoginUser = new List<User>();

    ReportDocument rdoc = new ReportDocument();
    bool validateFilter = false;
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
            Dictionary<string, string> branchList = new Dictionary<string, string>();
           
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("proc_TransferStockDaily_GetBranch", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    branchList.Add("-1", "---All---");
                    while (reader.Read())
                    {
                        branchList.Add(reader["BranchID"].ToString(), reader["BranchName"].ToString());
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
                throw ex;
            }
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
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                DateTime _checkDateF = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", null);  //Convert.ToDateTime(txtStartDate.Text);
                DateTime _checkDateT = DateTime.ParseExact(txtEndDate.Text, "dd/MM/yyyy", null); //Convert.ToDateTime(txtEndDate.Text);

                short updateDataFlag = 1;
                if (_checkDateF.ToShortDateString() == DateTime.Now.ToShortDateString() ||
                    _checkDateT.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 45, 00);
                    DateTime date2 = DateTime.Now;
                    int value = DateTime.Compare(date1, date2);
                    if (value > 0)
                    {
                        updateDataFlag = 0;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showStockAlertMsg()", true); 
                    }
                    
                }

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    string BranchID = ddlBranch.SelectedValue.ToString() == "-1" ? "" : ddlBranch.SelectedValue.ToString();

                    string dateFrom = "";
                    string dateTo = "";

                    var _dateFrom = txtStartDate.Text.Split('/').ToList();
                    dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

                    var _dateTo = txtEndDate.Text.Split('/').ToList();
                    dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("proc_TransferStockDaily_GetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    cmd.Parameters.Add(new SqlParameter("@DocDateFrom", dateFrom));
                    cmd.Parameters.Add(new SqlParameter("@DocDateTo", dateTo));
                    cmd.Parameters.Add(new SqlParameter("@UpdateDataFlag", updateDataFlag)); 

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TransferStockDailyReport");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TransferStockDailyReport"];
                    //dt.DefaultView.Sort = "PO asc, ItemCode asc";
                    //dt = dt.DefaultView.ToTable();

                    rdoc.Load(Server.MapPath("TransferStockDailyReport.rpt"));

                    rdoc.SetDataSource(dt);

                    rdoc.SetParameterValue("@BranchID", BranchID);
                    rdoc.SetParameterValue("@DocDateFrom", dateFrom);
                    rdoc.SetParameterValue("@DocDateTo", dateTo);

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