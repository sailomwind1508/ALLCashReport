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

public partial class TransferStockByVAN : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(txtDocDate.Text))
            {
                //DateTime _checkDateF = DateTime.ParseExact(txtDocDate.Text, "dd/MM/yyyy", null);  //Convert.ToDateTime(txtStartDate.Text);

                //short updateDataFlag = 1;
                //if (_checkDateF.ToShortDateString() == DateTime.Now.ToShortDateString())
                //{
                //    DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 45, 00);
                //    DateTime date2 = DateTime.Now;
                //    int value = DateTime.Compare(date1, date2);
                //    if (value > 0)
                //    {
                //        updateDataFlag = 0;
                //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showStockAlertMsg()", true);
                //    }

                //}

                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    
                    cn.Open();
                    SqlCommand _cmd = new SqlCommand("proc_VBanknote_All_DC_02_TestBegining", cn);
                    _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    _cmd.CommandTimeout = 0;
                    _cmd.ExecuteNonQuery();
                    cn.Close();

                
                    cn.Open();

                    string BranchID = ddlBranch.SelectedValue.ToString() == "-1" ? "" : ddlBranch.SelectedValue.ToString();

                    string docDate = "";

                    var _docDate = txtDocDate.Text.Split('/').ToList();
                    docDate = string.Join("/", _docDate[1], _docDate[0], _docDate[2]);

                    SqlCommand cmd = null;
                    cmd = new SqlCommand("proc_TransferStockDaily_ByVAN_GetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@BranchID", BranchID));
                    cmd.Parameters.Add(new SqlParameter("@DocDate", docDate));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TransferStockByVANReport");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TransferStockByVANReport"];
                    //dt.DefaultView.Sort = "PO asc, ItemCode asc";
                    //dt = dt.DefaultView.ToTable();

                    rdoc.Load(Server.MapPath("TransferStockByVANReport.rpt"));

                    rdoc.SetDataSource(dt);

                    rdoc.SetParameterValue("@BranchID", BranchID);
                    rdoc.SetParameterValue("@DocDate", docDate);

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