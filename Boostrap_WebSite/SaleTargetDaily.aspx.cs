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

public partial class SaleTargetDaily : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString_saletarget"].ConnectionString;
     
    List<User> LoginUser = new List<User>();

    ReportDocument rdoc1 = new ReportDocument();
    ReportDocument rdoc2 = new ReportDocument();
    ReportDocument rdoc3 = new ReportDocument();
    ReportDocument rdoc4 = new ReportDocument();

    bool validateFilter = false;
    Dictionary<string, string> empList = new Dictionary<string, string>();
    Dictionary<string, string> temp_empList = new Dictionary<string, string>();
    static int tabIndex = 0;

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

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "hideAll();", true);
        }

        rdoc1 = new ReportDocument();
        rdoc2 = new ReportDocument();
        rdoc3 = new ReportDocument();
        rdoc4 = new ReportDocument();
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            rdoc1 = new ReportDocument();
            rdoc2 = new ReportDocument();
            rdoc3 = new ReportDocument();
            rdoc4 = new ReportDocument();

            rdoc1.Close();
            rdoc1.Dispose();

            rdoc2.Close();
            rdoc2.Dispose();

            rdoc3.Close();
            rdoc3.Dispose();

            rdoc4.Close();
            rdoc4.Dispose();

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
            Dictionary<string, string> reportTypeList = new Dictionary<string, string>();
            reportTypeList.Add("emp", "---รายงานตามพนักงาน---");
            reportTypeList.Add("cust", "---รายงานตามร้านค้า---");
            reportTypeList.Add("custSum", "---รายงานสรุปตามร้านค้า---");
            reportTypeList.Add("plastic", "---รายงานยอดของแถมพลาสติก---");
            ddlReportType.BindDropdownList(reportTypeList);

            empList.StoredProceduresToList(conString, "proc_sale_target_cust_report_getEmployee_r", "SALE_ID", "SALE_NAME");
            temp_empList.StoredProceduresToList(conString, "proc_sale_target_cust_report_getEmployee_r", "SALE_ID", "SALE_NAME");

            if (Session["UserList"] != null)
            {
                Dictionary<string, bool> user = new Dictionary<string, bool>();
                user = (Dictionary<string, bool>)Session["AllCashReportLogin"];

                var _empList = new KeyValuePair<string, string>();

                if (user != null && !string.IsNullOrEmpty(user.First().Key))
                {
                    string userName = user.First().Key;
                    if (userName.Contains("M0"))
                    {
                        //string prefix_emID = userList[0].UserName.Substring(0, 1);
                        string _emID = userName.Substring(1, userName.Length - 1);
                        _emID = "S" + _emID;
                        _empList = empList.FirstOrDefault(x => x.Key == _emID);

                        empList = new Dictionary<string, string>();
                        empList.Add(_empList.Key, _empList.Value);
                    }
                    else
                    {
                        empList = temp_empList;
                    }
                }
            }

            ddlEmployee.BindDropdownList(empList);
            ddlEmployee2.BindDropdownList(empList);

            ViewState["EmpList"] = empList;

            ManageReport();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ManageReport();
    }

    private void ManageReport()
    {
        var rType = ddlReportType.SelectedValue.ToString();
        string funcName = "";
        if (rType == "emp")
        {
            funcName = "showEmp()";
        }
        else if (rType == "cust")
        {
            funcName = "showCust()";
        }
        else if (rType == "custSum")
        {
            funcName = "showCustSum()";
        }
        else if (rType == "plastic")
        {
            funcName = "showPlastic()";

            if (Session["UserList"] != null)
            {
                Dictionary<string, bool> user = new Dictionary<string, bool>();
                user = (Dictionary<string, bool>)Session["AllCashReportLogin"];

                if (user != null && !string.IsNullOrEmpty(user.First().Key))
                {
                    string userName = user.First().Key;
                    if (userName.Contains("M0"))
                    {
                        funcName = "hideAll()";
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", funcName, true);
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void btnReport2_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void btnReport3_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    private void SearchReport()
    {

    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenPattern1Report();
    }

    protected void CrystalReportViewer2_Load(object sender, EventArgs e)
    {
        GenPattern2Report();
    }

    protected void CrystalReportViewer3_Load(object sender, EventArgs e)
    {
        GenPattern3Report();
    }

    private void GenPattern1Report()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDocDate.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString_SAP"].ConnectionString))
                {
                    cn.Open();

                    string docDate = "";
                    string docDateParam = "";

                    var _docDate = txtDocDate.Text.Split('/').ToList();
                    docDate = string.Join("/", _docDate[1], _docDate[0], _docDate[2]);
                    docDateParam = string.Join("-", _docDate[2], _docDate[1], _docDate[0]);

                    SqlCommand cmd = null;

                    var empList = new Dictionary<string, string>();
                    if (ViewState["EmpList"] != null)
                    {
                        empList = (Dictionary<string, string>)ViewState["EmpList"];
                        string saleIDList = "";
                        List<string> saleIDs = new List<string>();

                        foreach (KeyValuePair<string, string> item in empList)
                        {
                            saleIDs.Add("'" + item.Key + "'");
                        }
                        saleIDList = string.Join(",", saleIDs);

                        cmd = new SqlCommand("SELECT * FROM V_TargetVSOrderVSInvoiceWS03 WHERE CAST(U_tabDate AS DATE) = CAST('" + docDateParam + "' AS DATE) AND SaleID IN (" + saleIDList + ")", cn);
                    }
                    else
                    {
                        cmd = new SqlCommand("SELECT * FROM V_TargetVSOrderVSInvoiceWS03 WHERE CAST(U_tabDate AS DATE) = CAST('" + docDateParam + "' AS DATE)", cn);
                    }

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TargetVSOrderVSInvoiceWS03-1");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TargetVSOrderVSInvoiceWS03-1"];
                    dt = dt.DefaultView.ToTable();

                    rdoc1.Load(Server.MapPath("TargetVsOrderWS-2.rpt"));

                    rdoc1.SetDataSource(dt);

                    rdoc1.SetParameterValue("?Date", docDate);

                    CrystalReportViewer1.RefreshReport();

                    //CrystalReportViewer1.ParameterFieldInfo.Clear();

                    //CrystalReportViewer1.EnableParameterPrompt = false;

                    CrystalReportViewer1.ReportSource = rdoc1;

                    CrystalReportViewer1.DataBind();

                    //rdoc1.Close();
                    //rdoc1.Dispose();

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

    private void GenPattern2Report()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDocDate2.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString_SAP"].ConnectionString))
                {
                    cn.Open();

                    string docDate = "";
                    string docDateParam = "";

                    var _docDate = txtDocDate2.Text.Split('/').ToList();
                    docDate = string.Join("/", _docDate[1], _docDate[0], _docDate[2]);
                    docDateParam = string.Join("-", _docDate[2], _docDate[1], _docDate[0]);

                    string sale_id = ddlEmployee.SelectedValue.ToString() == "-1" ? "" : ddlEmployee.SelectedValue.ToString();

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("proc_SaleTarget_Report_WS2", cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;   
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@Date", docDate));
                    cmd.Parameters.Add(new SqlParameter("@SaleID", sale_id));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TargetVSOrderVSInvoiceWS03-2");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TargetVSOrderVSInvoiceWS03-2"];
                    dt = dt.DefaultView.ToTable();

                    rdoc2.Load(Server.MapPath("TargetVsOrderWS2.rpt"));

                    rdoc2.SetDataSource(dt);

                    //rdoc2.SetParameterValue("Date", docDate);
                    //rdoc2.SetParameterValue("SalePerson", sale_id);

                    CrystalReportViewer2.RefreshReport();

                    CrystalReportViewer2.ReportSource = rdoc2;

                    CrystalReportViewer2.DataBind();

                    //rdoc2.Close();
                    //rdoc2.Dispose();

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

    private void GenPattern3Report()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDocDate3.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString_SAP"].ConnectionString))
                {
                    cn.Open();

                    string docDate = "";
                    string docDateParam = "";

                    var _docDate = txtDocDate3.Text.Split('/').ToList();
                    docDate = string.Join("/", _docDate[1], _docDate[0], _docDate[2]);
                    docDateParam = string.Join("-", _docDate[2], _docDate[1], _docDate[0]);

                    string sale_id = ddlEmployee2.SelectedValue.ToString() == "-1" ? "" : ddlEmployee2.SelectedValue.ToString();

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("select *,'' 'Remark'  FROM V_TargetVSOrderVSInvoiceWS03 WHERE CAST(U_tabDate AS DATE) = CAST('" + docDateParam + "' AS DATE) AND SaleID = '" + sale_id + "'", cn);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TargetVSOrderVSInvoiceWS03-3");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TargetVSOrderVSInvoiceWS03-3"];
                    dt = dt.DefaultView.ToTable();

                    rdoc3.Load(Server.MapPath("TargetVsOrderWS3.rpt"));

                    rdoc3.SetDataSource(dt);

                    rdoc3.SetParameterValue("Date", docDate);
                    rdoc3.SetParameterValue("SalePerson", sale_id);

                    CrystalReportViewer3.RefreshReport();

                    CrystalReportViewer3.ReportSource = rdoc3;

                    CrystalReportViewer3.DataBind();

                    //rdoc3.Close();
                    //rdoc3.Dispose();

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

    private void GenPattern4Report()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDocDateP.Text))
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    cn.Open();

                    var _docDate = txtDocDateP.Text.Split('/').ToList();
                    string day = _docDate[0].ToString();
                    string month = _docDate[1].ToString();
                    string year = _docDate[2].ToString();

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("select * FROM V_Actual_Plastic_promotion WHERE day = '"+ day + "' AND month = '" + month + "' AND year = '" + year + "'", cn);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ActualSalesPlasticPromotion");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["ActualSalesPlasticPromotion"];
                    dt = dt.DefaultView.ToTable();

                    rdoc4.Load(Server.MapPath("ActualSalesPlasticPromotion.rpt"));

                    rdoc4.SetDataSource(dt);

                    rdoc4.SetParameterValue("day", day);
                    rdoc4.SetParameterValue("month", month);
                    rdoc4.SetParameterValue("year", year);

                    CrystalReportViewer4.RefreshReport();

                    CrystalReportViewer4.ReportSource = rdoc4;

                    CrystalReportViewer4.DataBind();

                    //rdoc3.Close();
                    //rdoc3.Dispose();

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

    private ParameterField CreateCystalParam(string name, string value)
    {
        ParameterField DateFrom = new ParameterField();
        DateFrom.Name = name;
        ParameterDiscreteValue val = new ParameterDiscreteValue();
        val.Value = value;
        DateFrom.CurrentValues.Add(val);

        return DateFrom;
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        //base.OnUnload(e);

        //rdoc1.Close();
        //rdoc1.Dispose();
        //GC.Collect();
    }

    protected void CrystalReportViewer2_Unload(object sender, EventArgs e)
    {
        //base.OnUnload(e);

        //rdoc2.Close();
        //rdoc2.Dispose();
        //GC.Collect();
    }

    protected void CrystalReportViewer3_Unload(object sender, EventArgs e)
    {
        //base.OnUnload(e);

        //rdoc3.Close();
        //rdoc3.Dispose();
        //GC.Collect();
    }

    protected void btnReport4_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void CrystalReportViewer4_Load(object sender, EventArgs e)
    {
        GenPattern4Report();
    }

    protected void CrystalReportViewer4_Unload(object sender, EventArgs e)
    {

    }
}