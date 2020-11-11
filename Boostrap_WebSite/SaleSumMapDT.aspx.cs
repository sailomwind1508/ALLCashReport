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

public partial class SaleSumMapDT : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    static List<VanDetails> vanList = new List<VanDetails>();
    static List<SaleArea> salArea = new List<SaleArea>();
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
            InitPage();

            ValidateInputDate();
        }
    }

    private void InitPage()
    {
        Dictionary<string, string> branchList = new Dictionary<string, string>();
        vanList = new List<VanDetails>();
        salArea = new List<SaleArea>();

        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("proc_MonthlySalesSumReport_GetBranch", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                //branchList.Add("-1", "---All---");
                while (reader.Read())
                {
                    branchList.Add(reader["BranchID"].ToString(), reader["BranchName"].ToString());
                }

                cn.Close();

                cn.Open();
                cmd = new SqlCommand("proc_MonthlySalesSumReport_GetVANDetails", cn);
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
                cmd = new SqlCommand("proc_MonthlySalesSumReport_GetSaleArea", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SaleArea s = new SaleArea();
                    s.BranchID = reader["BranchID"].ToString();
                    s.SalAreaID = reader["SalAreaID"].ToString();
                    s.SalAreaName = reader["SalAreaName"].ToString();
                    s.VAN_ID = reader["VAN_ID"].ToString();

                    salArea.Add(s);
                }
                cn.Close();
            }

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

                ddlSaleArea.Items.Clear();
                List<SaleArea> _salArea = new List<SaleArea>();
                _salArea.Add(new SaleArea { BranchID = "-1", SalAreaID = "-1", SalAreaName = "---All---" });
                _salArea.AddRange(salArea.Where(x => x.BranchID == _branchID).OrderBy(y => y.SalAreaID).ToList());

                ddlSaleArea.DataSource = _salArea;
                ddlSaleArea.DataTextField = "SalAreaName";
                ddlSaleArea.DataValueField = "SalAreaID";
                ddlSaleArea.DataBind();
            }

            Dictionary<string, string> repportTypeList = new Dictionary<string, string>();
            repportTypeList.Add("PAT1", "MonthlyReport");
            repportTypeList.Add("PAT2", "YearlyReport");

            ddlReportType.Items.Clear();
            ddlReportType.DataSource = repportTypeList;
            ddlReportType.DataTextField = "Value";
            ddlReportType.DataValueField = "Key";
            ddlReportType.DataBind();

        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "PAT1")
        {
            BindGridView();

            DataTable dt = (DataTable)ViewState["MonthlySalesSumReport"];
            if (dt != null)
            {
                List<CustInfo> custInfoList = new List<CustInfo>();
                foreach (DataRow row in dt.Rows)
                {
                    CustInfo custInfo = new CustInfo();
                    custInfo.custtomerID = row["CustomerID"].ToString();
                    custInfo.name = "ร้าน : " + row["CustName"].ToString();
                    custInfo.address = row["AddressNo"].ToString() + " ต." + row["District"].ToString() + " อ." + row["Area"].ToString() + " จ." + row["Province"].ToString();
                    custInfo.tel = "โทร. " + row["Telephone"].ToString();
                    custInfo.lat = row["Latitude"].ToString();
                    custInfo.lng = row["Longitude"].ToString();
                    custInfo.custImg = row["CustomerImage"].ToString();
                    custInfo.rowCount = dt.Rows.Count;

                    custInfoList.Add(custInfo);
                }

                Session["custInfoList"] = custInfoList;

                linkExportReport.Visible = true;
                linkAllLocation.Visible = true;
            }

            //InitDropdownList();
        }
        else if (ddlReportType.SelectedValue == "PAT2")
        {
            SetReportFilter();

            string pageurl = "SalesPeriodReport.aspx";
            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
        }
    }

    private void BindGridView()
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                cn.Open();

                string dateFrom = "";
                string dateTo = "";
                List<string> _dateFrom = new List<string>();
                List<string> _dateTo = new List<string>();

                string reportType = ddlReportType.SelectedValue;
                if (reportType == "PAT1")
                {
                    _dateFrom = txtStartDate.Text.Split('/').ToList();
                    dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

                    _dateTo = txtEndDate.Text.Split('/').ToList();
                    dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);
                }

                string branch = ddlBranch.SelectedValue.ToString() == "-1" ? "" : ddlBranch.SelectedValue.ToString();
                string van = ddlVan.SelectedValue.ToString() == "-1" ? "" : ddlVan.SelectedValue.ToString();
                string saleArea = ddlSaleArea.SelectedValue.ToString() == "-1" ? "" : ddlSaleArea.SelectedValue.ToString();

                SqlCommand cmd = null;

                cmd = new SqlCommand("proc_MonthlySalesSumReport_GetData_MapDetails", cn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BranchID", branch));
                cmd.Parameters.Add(new SqlParameter("@VAN_ID", van));
                cmd.Parameters.Add(new SqlParameter("@SalAreaID", saleArea));
                cmd.Parameters.Add(new SqlParameter("@PostingDateFrom", dateFrom));
                cmd.Parameters.Add(new SqlParameter("@PostingDateTo", dateTo));
                cmd.Parameters.Add(new SqlParameter("@PatternFlag", reportType));


                cmd.CommandTimeout = 0;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "MonthlySalesSumReport");

                var dt = new DataTable();
                dt = ds.Tables["MonthlySalesSumReport"];
                ViewState["grdCustomerList"] = dt;
                grdCustomerList.DataSource = dt;
                grdCustomerList.DataBind();

                cn.Close();

                ViewState["MonthlySalesSumReport"] = ds.Tables["MonthlySalesSumReport"];

                SetReportFilter();

                Session["MonthlySalesSumReport_Data"] = ds;
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SetReportFilter()
    {
        string dateFrom = "";
        string dateTo = "";
        List<string> _dateFrom = new List<string>();
        List<string> _dateTo = new List<string>();

        string reportType = ddlReportType.SelectedValue;
        //if (reportType == "PAT1")
        {
            _dateFrom = txtStartDate.Text.Split('/').ToList();
            dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

            _dateTo = txtEndDate.Text.Split('/').ToList();
            dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);
        }

        string branch = ddlBranch.SelectedValue.ToString() == "-1" ? "" : ddlBranch.SelectedValue.ToString();
        string van = ddlVan.SelectedValue.ToString() == "-1" ? "" : ddlVan.SelectedValue.ToString();
        string saleArea = ddlSaleArea.SelectedValue.ToString() == "-1" ? "" : ddlSaleArea.SelectedValue.ToString();

        ReportFilter rft = new ReportFilter();
        rft.branch = branch;
        rft.van = van;
        rft.saleArea = saleArea;
        rft.dateFrom = dateFrom;
        rft.dateTo = dateTo;
        rft.reportType = reportType;
        rft.customerStatus = "";

        Session["ReportFilter"] = rft;
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitddlBranch();
    }

    protected void ddlVan_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitddlVan();
    }

    private void InitDropdownList()
    {
        InitddlBranch();
        InitddlVan();
    }

    private void InitddlBranch()
    {
        var _vanList = new List<VanDetails>();
        var _salArea = new List<SaleArea>();
        if (ddlBranch.SelectedValue != "-1")
        {
            _vanList.Add(new VanDetails { BranchID = "-1", VAN_ID = "-1", VanCode = "---All---" });
            _vanList.AddRange(vanList.Where(x => x.BranchID == ddlBranch.SelectedItem.Value).OrderBy(y => y.VAN_ID).ToList());

            _salArea.Add(new SaleArea { BranchID = "-1", SalAreaID = "-1", SalAreaName = "---All---" });
            _salArea.AddRange(salArea.OrderBy(y => y.SalAreaID).ToList());
        }
        else
        {
            _vanList.Add(new VanDetails { BranchID = "-1", VAN_ID = "-1", VanCode = "---All---" });
            _vanList.AddRange(vanList.OrderBy(y => y.VAN_ID).ToList());

            _salArea.Add(new SaleArea { BranchID = "-1", SalAreaID = "-1", SalAreaName = "---All---" });
            _salArea.AddRange(salArea.OrderBy(y => y.SalAreaID).ToList());

        }

        ddlVan.Items.Clear();
        ddlVan.DataSource = _vanList;
        ddlVan.DataTextField = "VanCode";
        ddlVan.DataValueField = "VAN_ID";
        ddlVan.DataBind();

        ddlSaleArea.Items.Clear();
        ddlSaleArea.DataSource = _salArea;
        ddlSaleArea.DataTextField = "SalAreaName";
        ddlSaleArea.DataValueField = "SalAreaID";
        ddlSaleArea.DataBind();
    }

    private void InitddlVan()
    {
        var _salArea = new List<SaleArea>();

        if (ddlVan.SelectedValue != "-1")
        {
            _salArea.Add(new SaleArea { BranchID = "-1", SalAreaID = "-1", SalAreaName = "---All---" });
            _salArea.AddRange(salArea.Where(x => x.BranchID == ddlBranch.SelectedItem.Value && x.VAN_ID == ddlVan.SelectedValue).OrderBy(y => y.SalAreaID).ToList());
        }
        else
        {
            _salArea.Add(new SaleArea { BranchID = "-1", SalAreaID = "-1", SalAreaName = "---All---" });
            _salArea.AddRange(salArea.OrderBy(y => y.SalAreaID).ToList());
        }

        ddlSaleArea.Items.Clear();
        ddlSaleArea.DataSource = _salArea;
        ddlSaleArea.DataTextField = "SalAreaName";
        ddlSaleArea.DataValueField = "SalAreaID";
        ddlSaleArea.DataBind();
    }

    protected void btnDetails_Click(object sender, EventArgs e)
    {
        if (Session["ReportFilter"] != null && Session["MonthlySalesSumReport_Data"] != null)
        {
            string pageurl = "MonthlySalesSumReport.aspx";

            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        ResponseToAllLocation();
    }

    private void ResponseToAllLocation()
    {
        if (Session["custInfoList"] != null)
        {
            string pageurl = "SalesSumLocation.aspx";

            Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
        }
    }

    protected void ddlSaleArea_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void ValidateInputDate()
    {

    }

    protected void grdCustomerList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "imgLocation")
        {
            if (Session["custInfoList"] != null)
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _custtomerID = "";
                _custtomerID = grdCustomerList.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_custtomerID))
                {
                    string pageurl = "CustomerPosition.aspx";

                    List<CustInfo> custInfoList = new List<CustInfo>();
                    custInfoList = (List<CustInfo>)Session["custInfoList"];

                    if (custInfoList.Count > 0 && !string.IsNullOrEmpty(_custtomerID))
                    {
                        Session["custInfoItem"] = custInfoList.Where(x => x.custtomerID == _custtomerID).ToList();

                        Response.Write("<script> window.open('" + pageurl + "', '_blank', 'toolbar = yes, scrollbars = yes, resizable = yes, top = 500, left = 500, width = 400, height = 400') </script>");
                    }
                }
            }
        }
    }

    protected void grdCustomerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdCustomerList"] != null)
        {
            grdCustomerList.PageIndex = e.NewPageIndex;
            grdCustomerList.DataSource = (DataTable)ViewState["grdCustomerList"];
            grdCustomerList.DataBind();
        }
    }
}