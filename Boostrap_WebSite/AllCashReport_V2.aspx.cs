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

public partial class AllCashReport_V2 : System.Web.UI.Page
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


                Dictionary<string, string> gAmtFilter = new Dictionary<string, string>();
                gAmtFilter.Add("500", "500");
                gAmtFilter.Add("1500", "1500");
                gAmtFilter.Add("3000", "3000");
                gAmtFilter.Add("5000", "5000");

                ddlFilter.DataSource = gAmtFilter;
                ddlFilter.DataTextField = "Value";
                ddlFilter.DataValueField = "Key";
                ddlFilter.DataBind();


                Dictionary<string, string> amtTypeList = new Dictionary<string, string>();
                amtTypeList.Add("Gross", "Gross Amt.");
                amtTypeList.Add("Net", "Net Amt.");

                ddlAmt.DataSource = amtTypeList;
                ddlAmt.DataTextField = "Value";
                ddlAmt.DataValueField = "Key";
                ddlAmt.DataBind();
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.Message);
                throw;
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
            if (!string.IsNullOrEmpty(txtPrevStartDate.Text) && !string.IsNullOrEmpty(txtPrevEndDate.Text) &&
                !string.IsNullOrEmpty(txtCurStartDate.Text) && !string.IsNullOrEmpty(txtCurEndDate.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    string datePrevFrom = "";
                    string datePrevTo = "";
                    string dateCurFrom = "";
                    string dateCurTo = "";

                    var _datePrevFrom = txtPrevStartDate.Text.Split('/').ToList();
                    datePrevFrom = string.Join("/", _datePrevFrom[1], _datePrevFrom[0], _datePrevFrom[2]);

                    var _datePrevTo = txtPrevEndDate.Text.Split('/').ToList();
                    datePrevTo = string.Join("/", _datePrevTo[1], _datePrevTo[0], _datePrevTo[2]);

                    var _dateCurFrom = txtCurStartDate.Text.Split('/').ToList();
                    dateCurFrom = string.Join("/", _dateCurFrom[1], _dateCurFrom[0], _dateCurFrom[2]);

                    var _dateCurTo = txtCurEndDate.Text.Split('/').ToList();
                    dateCurTo = string.Join("/", _dateCurTo[1], _dateCurTo[0], _dateCurTo[2]);

                    string area = ddlArea.SelectedValue.ToString() == "-1" ? "" : ddlArea.SelectedValue.ToString();
                    string province = ddlProvince.SelectedValue.ToString() == "-1" ? "" : ddlProvince.SelectedValue.ToString();
                    string criteriaFilter = ddlFilter.SelectedValue.ToString();

                    SqlCommand cmd = null;
                    if (ddlAmt.SelectedValue.ToString() == "Gross")
                    {
                        cmd = new SqlCommand("proc_AllCenterReport_GetData_ByCondition_V2", cn);
                    }
                    else
                    {
                        cmd = new SqlCommand("proc_AllCenterReport_GetData_ByCondition_NAmt", cn);
                    }

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PrevDateFrom", datePrevFrom));
                    cmd.Parameters.Add(new SqlParameter("@PrevDateTo", datePrevTo));

                    cmd.Parameters.Add(new SqlParameter("@CurrentDateFrom", dateCurFrom));
                    cmd.Parameters.Add(new SqlParameter("@CurrentDateTo", dateCurTo));

                    cmd.Parameters.Add(new SqlParameter("@RegionID", area));
                    cmd.Parameters.Add(new SqlParameter("@PrcCode", province));

                    cmd.Parameters.Add(new SqlParameter("@CriteriaFilter", criteriaFilter));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "AllCashReport");

                    rdoc.Load(Server.MapPath("CrystalReport2.rpt"));


                    rdoc.SetDataSource(ds.Tables["AllCashReport"]);

                    rdoc.SetParameterValue("@PrevDateFrom", datePrevFrom);
                    rdoc.SetParameterValue("@PrevDateTo", datePrevTo);
                    rdoc.SetParameterValue("@CurrentDateFrom", dateCurFrom);
                    rdoc.SetParameterValue("@CurrentDateTo", dateCurTo);

                    rdoc.SetParameterValue("@RegionID", area);
                    rdoc.SetParameterValue("@PrcCode", province);
                    rdoc.SetParameterValue("@CriteriaFilter", criteriaFilter);

                    CrystalReportViewer1.RefreshReport();

                    CrystalReportViewer1.ReportSource = rdoc;

                    CrystalReportViewer1.DataBind();

                    cn.Close();

                    //txtPrevStartDate.Text = datePrevFrom;
                    //txtPrevEndDate.Text = datePrevTo;
                    //txtCurStartDate.Text = dateCurFrom;
                    //txtCurEndDate.Text = dateCurTo;
                }
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