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

public partial class ForecashReportTK : System.Web.UI.Page
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
            try
            {

                Dictionary<string, string> gAmtFilter = new Dictionary<string, string>();
                gAmtFilter.Add("-1", "--All--");
                gAmtFilter.Add("UNI", "สินค้ายูไนเต็ด");
                gAmtFilter.Add("OTHER", "สินค้าอื่น ๆ");

                ddlProductType.DataSource = gAmtFilter;
                ddlProductType.DataTextField = "Value";
                ddlProductType.DataValueField = "Key";
                ddlProductType.DataBind();

                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT BranchID, BranchName FROM dbo.tbl_ForecastReport_Branch", cn);
                    cmd.CommandType = System.Data.CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    cn.Close();
                }
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchID";
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

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();

                string productType = ddlProductType.SelectedValue.ToString() == "-1" ? "" : ddlProductType.SelectedValue.ToString();
                string Branch = ddlBranch.SelectedItem.Text;

                SqlCommand cmd = null;
                if (ddlBranch.SelectedValue.ToString() == "101")
                {
                    cmd = new SqlCommand("proc_ForecashReport_GetForecashData", cn);
                }
                else if (ddlBranch.SelectedValue.ToString() == "102")
                {
                    cmd = new SqlCommand("proc_ForecashReport_GetForecashData_BP", cn);
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProductType", productType));
                cmd.Parameters.Add(new SqlParameter("@Branch", Branch));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ForecashReport");

                DataTable dt = new DataTable();
                dt = ds.Tables["ForecashReport"];
                dt.DefaultView.Sort = "PO asc, ItemCode asc";
                dt = dt.DefaultView.ToTable();

                rdoc.Load(Server.MapPath("ForecashReport.rpt"));

                rdoc.SetDataSource(dt);

                rdoc.SetParameterValue("@ProductType", productType);
                rdoc.SetParameterValue("@Branch", Branch);

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

    private ParameterField CreateCystalParam(string name, string value)
    {
        ParameterField DateFrom = new ParameterField();
        DateFrom.Name = name;
        ParameterDiscreteValue val = new ParameterDiscreteValue();
        val.Value = value;
        DateFrom.CurrentValues.Add(val);

        return DateFrom;
    }
}