using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;

public partial class QuestionnaireReport : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string conString_qa = ConfigurationManager.ConnectionStrings["myConnectionString_qa"].ConnectionString;
    Dictionary<string, string> permissionList = new Dictionary<string, string>();

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
        }

        var requestTarget = this.Request["__EVENTTARGET"];

        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "upload")
                ImportExcel();
        }
    }

    private void InitPage()
    {
        try
        {
            Dictionary<string, string> QATypeList = new Dictionary<string, string>();
            QATypeList.Add("1", "WS");
            QATypeList.Add("2", "ALLC");
            ddlQAType_T.BindDropdownList(QATypeList);
            ddlQAType_T_QA.BindDropdownList(QATypeList);

            Dictionary<string, object> b_prmt = new Dictionary<string, object>();
            b_prmt.Add("BranchID", "-1");

            DataTable _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);
            DataRow sRow = _dt.NewRow();
            sRow["BranchID"] = "-1";
            sRow["BranchName"] = "---ทั้งหมด---";
            _dt.Rows.InsertAt(sRow, 0);
            ddlBranch.BindDropdownList(_dt, "BranchName", "BranchID");

            BindCustomerWS();

            PrepareGridData();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void PrepareGridData()
    {
        Dictionary<string, object> _prmt = new Dictionary<string, object>();
        _prmt.Add("QAType", ddlQAType_T.SelectedItem.Text);

        DataTable _dt = Helper.ExecuteProcedureToTable(conString_qa, "proc_qa_get_questionnaire", _prmt);
        if (_dt != null && _dt.Rows.Count > 0)
        {
            ViewState["grdQA"] = _dt;
            BindGridView(grdQA, _dt);
        }

        Dictionary<string, object> _prmtQA = new Dictionary<string, object>();
        _prmtQA.Add("BranchID", ddlBranch.SelectedValue);

        DataTable _dtQA = Helper.ExecuteProcedureToTable(conString_qa, "proc_qa_get_qa_list", _prmtQA);
        if (_dtQA != null && _dtQA.Rows.Count > 0)
        {
            ViewState["grdQAList"] = _dtQA;
            BindGridView(grdQAList, _dtQA);
        }
    }

    private void BindGridView(GridView grd, DataTable data)
    {
        try
        {
            grd.DataSource = data;
            grd.DataBind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void BindVanID(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlBranch.SelectedValue);
        _dt = Helper.ExecuteProcedureToTable(conString, "proc_qa_van_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["van_id"] = "-1";
        _sRow["VanName"] = "---เลือกแวน---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlVanID.BindDropdownList(_dt, "VanName", "van_id");
        }
    }

    private void BindSaleArea(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlBranch.SelectedValue);
        p.Add("van_id", ddlVanID.SelectedValue);
        _dt = Helper.ExecuteProcedureToTable(conString, "proc_qa_salearea_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["SalAreaID"] = "-1";
        _sRow["SalAreaName"] = "---เลือกตลาด---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlSalAreaID.BindDropdownList(_dt, "SalAreaName", "SalAreaID");
        }
    }

    private void BindCustomer(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlBranch.SelectedValue);
        p.Add("van_id", ddlVanID.SelectedValue);
        p.Add("SalAreaID", ddlSalAreaID.SelectedValue);
        _dt = Helper.ExecuteProcedureToTable(conString, "proc_qa_customer_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["CustomerID"] = "-1";
        _sRow["CustName"] = "---เลือกร้านค้า---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlCustomer.BindDropdownList(_dt, "CustName", "CustomerID");
        }
    }

    private void BindCustomerWS()
    {
        DataTable _dt = new DataTable();
        _dt = Helper.ExecuteProcedureToTable(conString, "proc_qa_customer_get_ws", CommandType.StoredProcedure, null);

        DataRow _sRow = _dt.NewRow();
        _sRow["CustomerID"] = "-1";
        _sRow["CustName"] = "---เลือกร้านค้า---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlWSCustomer.BindDropdownList(_dt, "CustName", "CustomerID");
        }
    }

    private void ImportExcel()
    {
        List<bool> results = new List<bool>();

        try
        {
            if (string.IsNullOrEmpty(FileUpload1.FileName))
            {
                lblUploadResult.Text = "กรุณาเลือก excel!";
                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);

                return;
            }

            if (FileUpload1.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(FileUpload1.FileName))
                {
                    results.Add(UploadExcel(FileUpload1, "Topic", "tbl_Questionnaire"));

                    results.Add(UploadExcel(FileUpload1, "QA_Details", "tbl_QuestionnaireDetails"));
                }
            }

            if (results.Count > 0 && results.All(x => x))
            {
                PrepareGridData();

                lblUploadResult.Text = "อัพโหลด excel เรียบร้อยแล้ว!";
                lblUploadResult.ForeColor = System.Drawing.Color.Green;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true); //SendResult
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
        }
    }

    private bool UploadExcel(FileUpload fileUP, string sheetName, string tableName)
    {
        bool result = false;

        try
        {
            if (fileUP.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(fileUP.FileName))
                {
                    string sqlCmd = "TRUNCATE TABLE " + tableName;
                    Helper.ExecuteProcedureToTable(conString_qa, sqlCmd, CommandType.Text);

                    bool _result = AddExcelToDB(fileUP, sheetName, tableName);
                    //Helper.ExecuteProcedure(conString, storeName, _prmt);

                    if (_result)
                        result = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);

            result = false;
        }

        return result;
    }

    private bool AddExcelToDB(FileUpload fileUP, string sheetName, string tableName)
    {
        bool result = false;
        try
        {
            if (fileUP.HasFile)
            {
                DateTime cDate = DateTime.Now;

                string path = string.Concat(Server.MapPath("~/App_Data/" + fileUP.FileName));
                fileUP.SaveAs(path);

                var dataTable = new DataTable();
                dataTable = Helper.ReadExcelToDataTable(conString_qa, fileUP.FileName, sheetName);

                DataTable readDT = new DataTable(tableName);

                if (tableName == "tbl_Questionnaire")
                {
                    readDT.Columns.Add("PK", typeof(int));
                    readDT.Columns.Add("QuestionnaireID", typeof(int));
                    readDT.Columns.Add("Topic", typeof(string));
                    readDT.Columns.Add("Pattern", typeof(string));
                    readDT.Columns.Add("QAType", typeof(string));
                    readDT.Columns.Add("CreateDate", typeof(DateTime));
                    readDT.Columns.Add("UpdateDate", typeof(DateTime));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string _pkStr = "";
                        _pkStr = row[0].ToString();
                        int _pk = 0;

                        string _questionnaireIDStr = "";
                        _questionnaireIDStr = row[1].ToString();
                        int _questionnaireID = 0;

                        if (int.TryParse(_pkStr, out _pk) &&
                            int.TryParse(_questionnaireIDStr, out _questionnaireID))
                        {
                            readDT.Rows.Add(_pk, _questionnaireID, row[2].ToString(), row[3].ToString(), row[4].ToString(), cDate, DBNull.Value);
                        }
                    }
                }
                else if (tableName == "tbl_QuestionnaireDetails")
                {
                    readDT.Columns.Add("QuestionnaireDetailsID", typeof(int));
                    readDT.Columns.Add("QuestionnaireID", typeof(int));
                    readDT.Columns.Add("Seq", typeof(int));
                    readDT.Columns.Add("Question", typeof(string));
                    readDT.Columns.Add("CreateDate", typeof(DateTime));
                    readDT.Columns.Add("UpdateDate", typeof(DateTime));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string _questionnaireDetailsStr = "";
                        _questionnaireDetailsStr = row[0].ToString();
                        int _questionnaireDetailsID = 0;

                        string _questionnaireIDStr = "";
                        _questionnaireIDStr = row[1].ToString();
                        int _questionnaireID = 0;

                        string _seqStr = "";
                        _seqStr = row[2].ToString();
                        int _seq = 0;

                        if (int.TryParse(_questionnaireDetailsStr, out _questionnaireDetailsID) &&
                            int.TryParse(_questionnaireIDStr, out _questionnaireID) &&
                            int.TryParse(_seqStr, out _seq))
                        {
                            readDT.Rows.Add(_questionnaireDetailsID, _questionnaireID, _seq, row[3].ToString(), cDate, DBNull.Value);
                        }
                    }
                }

                if (readDT != null && readDT.Rows.Count > 0)
                {
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(conString_qa);
                    bulkInsert.DestinationTableName = tableName;
                    bulkInsert.WriteToServer(readDT);
                    result = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
        }

        return result;
    }

    protected void ddlQAType_T_SelectedIndexChanged(object sender, EventArgs e)
    {
        PrepareGridData();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void grdQA_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdQA.PageIndex;
        grdQA.DataSource = SortDataTable((DataTable)ViewState["grdQA"], false);
        grdQA.DataBind();
        for (int i = 1; i < grdQA.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdQA.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdQA.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdQA.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    private string GridViewSortDirection
    {
        get { return ViewState["SortDirection"] as string ?? "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return ViewState["SortExpression"] as string ?? string.Empty; }
        set { ViewState["SortExpression"] = value; }
    }

    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }

    protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
    {
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (GridViewSortExpression != string.Empty)
            {
                if (isPageIndexChanging)
                {
                    dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                }
                else
                {
                    dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                }
            }
            return dataView;
        }
        else
        {
            return new DataView();
        }
    }

    protected void grdQA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdQA"] != null)
        {
            grdQA.PageIndex = e.NewPageIndex;
            BindGridView(grdQA, (DataTable)ViewState["grdQA"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
    }

    protected void grdQA_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grdQAList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdQAList_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void grdQAList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void grdQAList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnSearchQABranch_Click(object sender, EventArgs e)
    {

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue != "-1")
            BindVanID();
        else
        {
            BindVanID(true);
            BindSaleArea(true);
            BindCustomer(true);
        }
    }

    protected void ddlVanID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue != "-1" && ddlVanID.SelectedValue != "-1")
            BindSaleArea();
        else
        {
            BindSaleArea(true);
            BindCustomer(true);
        }
    }

    protected void ddlSalAreaID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue != "-1" && ddlVanID.SelectedValue != "-1" && ddlSalAreaID.SelectedValue != "-1")
            BindCustomer();
        else
            BindCustomer(true);
    }

    protected void ddlQAType_T_QA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (true)
        {

        }
    }
}