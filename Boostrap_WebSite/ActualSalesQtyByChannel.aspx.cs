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

public partial class ActualSalesQtyByChannel : System.Web.UI.Page
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
                    SqlCommand cmd = new SqlCommand("proc_ActualSalesQtyByChannel_GetProductionLine", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    branchList.Add("-1", "---All---");
                    while (reader.Read())
                    {
                        branchList.Add(reader["RowNum"].ToString(), reader["production_line"].ToString());
                    }

                    cn.Close();
                }

                ddlProductLine.Items.Clear();
                ddlProductLine.DataSource = branchList;
                ddlProductLine.DataTextField = "Value";
                ddlProductLine.DataValueField = "Key";
                ddlProductLine.DataBind();
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
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    string productLine = ddlProductLine.SelectedValue.ToString() == "-1" ? "" : ddlProductLine.SelectedItem.Text.ToString();
                    string dateFrom = "";
                    string dateTo = "";

                    var _dateFrom = txtStartDate.Text.Split('/').ToList();
                    dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

                    var _dateTo = txtEndDate.Text.Split('/').ToList();
                    dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("proc_ActualSalesQtyByChannel_GetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PostingDateFrom", dateFrom));
                    cmd.Parameters.Add(new SqlParameter("@PostingDateTo", dateTo));
                    cmd.Parameters.Add(new SqlParameter("@ProductionLine", productLine));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ActualSalesQtyByChannelReport");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["ActualSalesQtyByChannelReport"];
        

                    rdoc.Load(Server.MapPath("ActualSalesQtyByChannelReport.rpt"));

                    rdoc.SetDataSource(dt);

                    rdoc.SetParameterValue("@PostingDateFrom", dateFrom);
                    rdoc.SetParameterValue("@PostingDateTo", dateTo);
                    rdoc.SetParameterValue("@ProductionLine", productLine);

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