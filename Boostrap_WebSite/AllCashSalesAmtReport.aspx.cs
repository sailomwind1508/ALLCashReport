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

public partial class AllCashSalesAmtReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    List<User> LoginUser = new List<User>();

    ReportDocument rdoc1 = new ReportDocument();
    ReportDocument rdoc2 = new ReportDocument();
    ReportDocument rdoc3 = new ReportDocument();

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
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            rdoc1 = new ReportDocument();

            rdoc1.Close();
            rdoc1.Dispose();

            rdoc2 = new ReportDocument();

            rdoc2.Close();
            rdoc2.Dispose();

            rdoc3 = new ReportDocument();

            rdoc3.Close();
            rdoc3.Dispose();

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
            reportTypeList.Add("r1", "---รายงานจำนวนขายสินค้า all cash เฉลี่ยต่อวัน(หีบ)---");
            reportTypeList.Add("r2", "---รายงานจำนวนแถมสินค้า all cash เฉลี่ยต่อวัน(หีบ)---");
            reportTypeList.Add("r3", "---รายงานยอดขายสินค้า all cash เฉลี่ยต่อวัน(บาท)---");
            //reportTypeList.Add("custSum", "---รายงานสรุปตามร้านค้า---");
            //reportTypeList.Add("plastic", "---รายงานยอดของแถมพลาสติก---");
            ddlReportType.BindDropdownList(reportTypeList);

            ManageReport();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    private void ManageReport()
    {
        var rType = ddlReportType.SelectedValue.ToString();
        string funcName = "";
        if (rType == "r1")
        {
            funcName = "showR1()";
        }
        else if (rType == "r2")
        {
            funcName = "showR2()";
        }
        else if (rType == "r3")
        {
            funcName = "showR3()";
        }
        //else if (rType == "custSum")
        //{
        //    funcName = "showCustSum()";
        //}
        //else if (rType == "plastic")
        //{
        //    funcName = "showPlastic()";

        //    if (Session["UserList"] != null)
        //    {
        //        Dictionary<string, bool> user = new Dictionary<string, bool>();
        //        user = (Dictionary<string, bool>)Session["AllCashReportLogin"];

        //        if (user != null && !string.IsNullOrEmpty(user.First().Key))
        //        {
        //            string userName = user.First().Key;
        //            if (userName.Contains("M0"))
        //            {
        //                funcName = "hideAll()";
        //            }
        //        }
        //    }
        //}

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", funcName, true);
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        //if (ddlReportType.SelectedValue.ToString() == "r1")
            GenPatternReport(CrystalReportViewer1, rdoc1, txtDateFrom, txtDateTo, "proc_Qty_AllC_PerDay", "Qty_AllC_PerDay.rpt");
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {

    }

    private void GenPatternReport(CrystalDecisions.Web.CrystalReportViewer crytalvw, ReportDocument rdoc, TextBox txt1, TextBox txt2, string procName, string reportName)
    {
        try
        {
            if (!string.IsNullOrEmpty(txt1.Text) && !string.IsNullOrEmpty(txt2.Text))
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    string docDateF = "";
                    string docDateT = "";
           
                    var _docDateF = txt1.Text.Split('/').ToList();
                    docDateF = string.Join("/", _docDateF[1], _docDateF[0], _docDateF[2]);

                    var _docDateT = txt2.Text.Split('/').ToList();
                    docDateT = string.Join("/", _docDateT[1], _docDateT[0], _docDateT[2]);

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DateFrom", docDateF));
                    cmd.Parameters.Add(new SqlParameter("@DateTo", docDateT));

                    cmd.CommandTimeout = 0;
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, procName);

                    DataTable dt = new DataTable();
                    dt = ds.Tables[procName];
                    dt = dt.DefaultView.ToTable();

                    rdoc.Load(Server.MapPath(reportName));

                    rdoc.SetDataSource(dt);

                    //rdoc.SetParameterValue("Date From", docDateF);
                    //rdoc.SetParameterValue("Date To", docDateT);
                    //rdoc.SetParameterValue("DF", docDateF);
                    //rdoc.SetParameterValue("DT", docDateT);

                    //ParameterFields paramFields = new ParameterFields();
                    //paramFields.Add(SetCrystalParam("Date From", docDateF));
                    //paramFields.Add(SetCrystalParam("Date To", docDateT));
                    //crytalvw.ParameterFieldInfo = paramFields;

                    crytalvw.ReportSource = rdoc;

                    crytalvw.DataBind();

                    crytalvw.RefreshReport();

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

    private ParameterField SetCrystalParam(string paramName, string paramValue)
    {
        ParameterField pfItemYr = new ParameterField();

        pfItemYr.ParameterFieldName = paramName;

        ParameterDiscreteValue dcItemYr = new ParameterDiscreteValue();

        dcItemYr.Value = paramValue;

        pfItemYr.CurrentValues.Add(dcItemYr);

        return pfItemYr;
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

    protected void CrystalReportViewer2_Load(object sender, EventArgs e)
    {
        //if (ddlReportType.SelectedValue.ToString() == "r2")
            GenPatternReport(CrystalReportViewer2, rdoc2, txtDateFrom2, txtDateTo2, "proc_QtyFOC_AllC_PerDay", "QtyFOC_AllC_PerDay.rpt");
    }

    protected void CrystalReportViewer2_Unload(object sender, EventArgs e)
    {

    }

    protected void btnReport2_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void btnReport3_Click(object sender, EventArgs e)
    {
        ManageReport();
    }

    protected void CrystalReportViewer3_Load(object sender, EventArgs e)
    {
        //if (ddlReportType.SelectedValue.ToString() == "r3")
            GenPatternReport(CrystalReportViewer3, rdoc3, txtDateFrom3, txtDateTo3, "proc_SaleAmt_AllC_PerDay", "SaleAmt_AllC_PerDay.rpt");
    }

    protected void CrystalReportViewer3_Unload(object sender, EventArgs e)
    {

    }
}