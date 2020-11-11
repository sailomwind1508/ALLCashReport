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

public partial class TransferStockBySKU : System.Web.UI.Page
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
        Dictionary<string, string> branchList = new Dictionary<string, string>();
        vanList = new List<VanDetails>();
        productList = new Dictionary<string, string>();

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

                cn.Open();
                cmd = new SqlCommand("proc_TransferStockDaily_BySKU_GetVAN", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    VanDetails vd = new VanDetails();
                    vd.BranchID = reader["BranchID"].ToString();
                    vd.VAN_ID = reader["VAN_ID"].ToString();
                    vd.VanCode = reader["VAN_ID"].ToString();

                    vanList.Add(vd);
                }
                cn.Close();

                cn.Open();
                cmd = new SqlCommand("proc_TransferStockDaily_BySKU_GetProduct", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                reader = cmd.ExecuteReader();

                productList.Add("-1", "---All---");
                while (reader.Read())
                {
                    productList.Add(reader["ProductID"].ToString(), reader["ProductID"].ToString() + " - " + reader["ProductName"].ToString());
                }
                cn.Close();
            }

            ddlProduct.Items.Clear();
            ddlProduct.DataSource = productList;
            ddlProduct.DataTextField = "Value";
            ddlProduct.DataValueField = "Key";
            ddlProduct.DataBind();


            ddlBranch.Items.Clear();
            ddlBranch.DataSource = branchList;
            ddlBranch.DataTextField = "Value";
            ddlBranch.DataValueField = "Key";
            ddlBranch.DataBind();

            string _branchID = branchList.FirstOrDefault().Key;
            if (!string.IsNullOrEmpty(_branchID))
            {
                ddlVan.Items.Clear();
                List<VanDetails> _vanList = new List<VanDetails>();
                _vanList.Add(new VanDetails { BranchID = "-1", VAN_ID = "-1", VanCode = "---All---" });
                _vanList.AddRange(vanList.Where(x => x.BranchID == _branchID).OrderBy(y => y.VAN_ID).ToList());

                ddlVan.DataSource = _vanList;
                ddlVan.DataTextField = "VanCode";
                ddlVan.DataValueField = "VAN_ID";
                ddlVan.DataBind();
            }

            Dictionary<string, string> repportTypeList = new Dictionary<string, string>();
            repportTypeList.Add("PAT1", "MonthlyReport");
            repportTypeList.Add("PAT2", "YearlyReport");

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

                string branch = ddlBranch.SelectedValue.ToString() == "-1" ? "" : ddlBranch.SelectedValue.ToString();
                string van = ddlVan.SelectedValue.ToString() == "-1" ? "" : ddlVan.SelectedValue.ToString();
                string prdId = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();

                string docDate = "";

                var _docDate = txtDocDate.Text.Split('/').ToList();
                docDate = string.Join("/", _docDate[1], _docDate[0], _docDate[2]);


                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
                {
                    SqlCommand cmd = null;

                    cn.Open();
                    cmd = new SqlCommand("proc_TransferStockDaily_BySKU_SubGetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@BranchID", branch));
                    cmd.Parameters.Add(new SqlParameter("@VAN_ID", van));
                    cmd.Parameters.Add(new SqlParameter("@DocDate", docDate));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", prdId));

                    cmd.ExecuteNonQuery();
                    
                    cn.Close();

                    cn.Open();
                    cmd = new SqlCommand("proc_TransferStockDaily_BySKU_GetData", cn);
                    cmd.CommandTimeout = 0;

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@BranchID", branch));
                    //cmd.Parameters.Add(new SqlParameter("@VAN_ID", van));
                    //cmd.Parameters.Add(new SqlParameter("@DocDate", docDate));
                    //cmd.Parameters.Add(new SqlParameter("@DocDate", prdId));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "TransferStockBySKUReport");

                    DataTable dt = new DataTable();
                    dt = ds.Tables["TransferStockBySKUReport"];

                    rdoc.Load(Server.MapPath("TransferStockBySKUReport.rpt"));

                    rdoc.SetDataSource(dt);

                    //rdoc.SetParameterValue("@BranchID", branch);
                    //rdoc.SetParameterValue("@VAN_ID", van);
                    //rdoc.SetParameterValue("@DocDate", docDate);

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


    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitddlBranch();
    }

    private void InitddlBranch()
    {
        var _vanList = new List<VanDetails>();
        var _salArea = new List<SaleArea>();
        if (ddlBranch.SelectedValue != "-1")
        {
            _vanList.Add(new VanDetails { BranchID = "-1", VAN_ID = "-1", VanCode = "---All---" });
            _vanList.AddRange(vanList.Where(x => x.BranchID == ddlBranch.SelectedItem.Value).OrderBy(y => y.VAN_ID).ToList());
        }
        else
        {
            _vanList.Add(new VanDetails { BranchID = "-1", VAN_ID = "-1", VanCode = "---All---" });
            _vanList.AddRange(vanList.OrderBy(y => y.VAN_ID).ToList());
        }

        ddlVan.Items.Clear();
        ddlVan.DataSource = _vanList;
        ddlVan.DataTextField = "VanCode";
        ddlVan.DataValueField = "VAN_ID";
        ddlVan.DataBind();
    }
}