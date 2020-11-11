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
using Report_ClassLibrary;
using System.Collections;

public partial class MonthlySalesSumReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    static List<VanDetails> vanList = new List<VanDetails>();
    static List<SaleArea> salArea = new List<SaleArea>();
    static DataSet monthlySalesData;
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
            
        }
    }

    private void RenderReport(DataSet ds, ReportFilter rft)
    {
        try
        {
            rdoc.Load(Server.MapPath("MonthlySalesSumReport.rpt"));

            rdoc.SetDataSource(ds.Tables["MonthlySalesSumReport"]);

            rdoc.SetParameterValue("@BranchID", rft.branch);
            rdoc.SetParameterValue("@VAN_ID", rft.van);
            rdoc.SetParameterValue("@SalAreaID", rft.saleArea);
            rdoc.SetParameterValue("@PostingDateFrom", rft.dateFrom);
            rdoc.SetParameterValue("@PostingDateTo", rft.dateTo);
            rdoc.SetParameterValue("@PatternFlag", rft.reportType);


            CrystalReportViewer1.RefreshReport();

            CrystalReportViewer1.ReportSource = rdoc;

            CrystalReportViewer1.DataBind();
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


    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        if (Session["MonthlySalesSumReport_Data"] != null && Session["ReportFilter"] != null)
        {
            DataSet ds = (DataSet)Session["MonthlySalesSumReport_Data"];
            ReportFilter rft = (ReportFilter)Session["ReportFilter"];

            RenderReport(ds, rft);
        }
    }
}
