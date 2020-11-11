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


public partial class SalesOrderReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    List<User> LoginUser = new List<User>();
    static List<VanDetails> vanList = new List<VanDetails>();
    static Dictionary<string, string> productList = new Dictionary<string, string>();

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

            InitPage();
        }
    }

    private void InitPage()
    {
        Dictionary<string, string> prodList = new Dictionary<string, string>();

        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("proc_SaleOrderReport_GetProduct", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                prodList.Add("-1", "---All---");
                while (reader.Read())
                {
                    prodList.Add(reader["PrdCode"].ToString(), reader["PrdCode"].ToString() + " : " + reader["PrdName"].ToString());
                }

                cn.Close(); 
            }

            ddlProduct.Items.Clear();
            ddlProduct.DataSource = prodList;
            ddlProduct.DataTextField = "Value";
            ddlProduct.DataValueField = "Key";
            ddlProduct.DataBind();
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
            if (!string.IsNullOrEmpty(txtDocDateFrom.Text) && !string.IsNullOrEmpty(txtDocDateTo.Text))
            {
                string prdCode = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();

                string docDateFrom = "";
                string docDateTo = "";

                var _docDateFrom = txtDocDateFrom.Text.Split('/').ToList();
                docDateFrom = string.Join("/", _docDateFrom[1], _docDateFrom[0], _docDateFrom[2]);

                var _docDateTo = txtDocDateTo.Text.Split('/').ToList();
                docDateTo = string.Join("/", _docDateTo[1], _docDateTo[0], _docDateTo[2]);

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    SqlCommand cmd = null;

                    cn.Open();
                    cmd = new SqlCommand("proc_SaleOrderReport_GetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DocDateFrom", docDateFrom));
                    cmd.Parameters.Add(new SqlParameter("@DocDateTo", docDateTo));
                    cmd.Parameters.Add(new SqlParameter("@PrdCode", prdCode));

                    cmd.ExecuteNonQuery();

                    cn.Close();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SalesOrderReport");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["SalesOrderReport"];

                    rdoc.Load(Server.MapPath("SalesOrderReport.rpt"));

                    rdoc.SetDataSource(dt);

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