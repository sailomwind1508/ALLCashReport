using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AllCashReport : System.Web.UI.Page
{
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
                Response.Redirect("~/Index.aspx");
            }
            if (UserList.First().Key.Contains("sell"))
            {
                Response.Redirect("~/ForecashReportTK.aspx");
            }
        }
        if (Session["AllCashReportLogin"] == null)
        {
            //UserList.Add(username, false);

            //Session["AllCashReportLogin"] = "0";
            Response.Redirect("~/Index.aspx");
        }

        if (!IsPostBack)
        {
            Dictionary<string, string> areaList = new Dictionary<string, string>();
            Dictionary<string, string> provinceList = new Dictionary<string, string>();

            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("proc_AllCenterReport_GetRegion", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader reader = cmd.ExecuteReader();
                    areaList.Add("-1", "---All---");
                    while (reader.Read())
                    {
                        areaList.Add(reader["RegionID"].ToString(), reader["RegionName"].ToString());
                    }

                    cn.Close();

                    cn.Open();
                    cmd = new SqlCommand("proc_AllCenterReport_GetProvince", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    reader = cmd.ExecuteReader();
                    provinceList.Add("-1", "---All---");
                    while (reader.Read())
                    {
                        provinceList.Add(reader["PrcCode"].ToString(), reader["ProvinceName"].ToString());
                    }
                    cn.Close();
                }

                ddlArea.DataSource = areaList;
                ddlArea.DataTextField = "Value";
                ddlArea.DataValueField = "Key";
                ddlArea.DataBind();

                ddlProvince.DataSource = provinceList;
                ddlProvince.DataTextField = "Value";
                ddlProvince.DataValueField = "Key";
                ddlProvince.DataBind();


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            //if (ValidateFilter())
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

                    string area = ddlArea.SelectedValue.ToString() == "-1" ? "" : ddlArea.SelectedValue.ToString();
                    string province = ddlProvince.SelectedValue.ToString() == "-1" ? "" : ddlProvince.SelectedValue.ToString();

                    SqlCommand cmd = new SqlCommand("proc_AllCenterReport_GetData_ByCondition", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DateFrom", dateFrom));
                    cmd.Parameters.Add(new SqlParameter("@DateTo", dateTo));


                    cmd.Parameters.Add(new SqlParameter("@RegionID", area));
                    cmd.Parameters.Add(new SqlParameter("@PrcCode", province));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "AllCashReport");

                    rdoc.Load(Server.MapPath("CrystalReport1.rpt"));


                    rdoc.SetDataSource(ds.Tables["AllCashReport"]);



                    rdoc.SetParameterValue("DateFrom", dateFrom);
                    rdoc.SetParameterValue("DateTo", dateTo);

                    rdoc.SetParameterValue("RegionID", area);
                    rdoc.SetParameterValue("PrcCode", province);


                    CrystalReportViewer1.RefreshReport();

                    CrystalReportViewer1.ReportSource = rdoc;

                    CrystalReportViewer1.DataBind();


                    cn.Close();

                    txtStartDate.Text = dateFrom;
                    txtEndDate.Text = dateTo;

                }
            }
        }
        catch (Exception ex)
        {

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

    private bool ValidateFilter()
    {
        bool result = true;
        if (string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showValidateMsg();", true);

            result = false;
        }

        return result;
    }
}