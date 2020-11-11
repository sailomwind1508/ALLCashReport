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
using System.IO;
using ClosedXML.Excel;

public partial class SalesPeriodReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    ReportFilter rft = new ReportFilter();

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
            if (Session["ReportFilter"] != null)
            {
                rft = (ReportFilter)Session["ReportFilter"];
                try
                {
                    using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                    {
                        cn.Open();

                        SqlCommand cmd = null;

                        cmd = new SqlCommand("proc_MonthlySalesSumReport_GetData_SalePreriod", cn);

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@BranchID", rft.branch));
                        cmd.Parameters.Add(new SqlParameter("@VAN_ID", rft.van));
                        cmd.Parameters.Add(new SqlParameter("@SalAreaID", rft.saleArea));
                        cmd.Parameters.Add(new SqlParameter("@PostingDateFrom", rft.dateFrom));
                        cmd.Parameters.Add(new SqlParameter("@PostingDateTo", rft.dateTo));

                        cmd.CommandTimeout = 0;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "SalesPeriodReport");
                        DataTable dt = ds.Tables["SalesPeriodReport"];

                        ViewState["SalesPeriodReport"] = dt;

                        cn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog(ex.Message);
                    throw ex;
                }

                BindGridView();
            }
        }
    }

    private void BindGridView()
    {
        try
        {
            if (ViewState["SalesPeriodReport"] != null)
            {

                GridView1.DataSource = (DataTable)ViewState["SalesPeriodReport"];

                GridView1.DataBind();
                
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void linkExportExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["SalesPeriodReport"] != null)
        {
            var _dt = (DataTable)ViewState["SalesPeriodReport"];

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_dt, "SalesPeriodReport");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SalesPeriodReport.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                }
                Response.Flush();
                Response.End();
            }
        }
    }
}