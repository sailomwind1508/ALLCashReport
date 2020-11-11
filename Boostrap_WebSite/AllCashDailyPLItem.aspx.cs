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

public partial class AllCashDailyPLItem : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
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

    #region private methods

    private bool AddExcelToDB(FileUpload fileUP, string tableName)
    {
        bool ret = false;
        string errMsg = "";
        List<string> result = new List<string>();
        List<int> exAcc = new List<int>() { 1, 2, 3, 13 };
        try
        {
            if (fileUP.HasFile)
            {
                DateTime cDate = DateTime.Now;
                string _transDate = "";
                var _dDate = txtTransferDate.Text.Split('/').ToList();
                _transDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);

                DateTime _uDate = Convert.ToDateTime(_transDate + " " + cDate.ToShortTimeString());

                string path = string.Concat(Server.MapPath("~/App_Data/" + fileUP.FileName));
                fileUP.SaveAs(path);

                if (tableName == "tbl_allc_daily_pl_item_import")
                {
                    var branchDT = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_branch", null);
                    if (branchDT != null && branchDT.Rows.Count > 0)
                    {
                        var brancList = branchDT.AsEnumerable();

                        var accMstDT = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_acc_master", null);
                        if (accMstDT != null && accMstDT.Rows.Count > 0)
                        {
                            var accMstList = accMstDT.AsEnumerable();

                            DataTable readDT = new DataTable(tableName);
                            readDT.Columns.Add("ImpDate", typeof(string));
                            readDT.Columns.Add("BranchID", typeof(string));
                            readDT.Columns.Add("VersionID", typeof(string));
                            readDT.Columns.Add("AccountingCode", typeof(string));
                            readDT.Columns.Add("Value", typeof(decimal));
                            readDT.Columns.Add("UpdateBy", typeof(string));
                            readDT.Columns.Add("UpdateDate", typeof(DateTime));

                            string ImpDateStr = "";

                            foreach (var _sheetName in (List<string>)Session["SheetNameList"])
                            {
                                var dataTable = new DataTable();
                                try
                                {
                                    dataTable = Helper.ReadExcelToDataTable2(conString, fileUP.FileName, _sheetName);
                                }
                                catch(Exception ex)
                                {
                                    dataTable = null;
                                }

                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    List<bool> allCheckValid = new List<bool>();

                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        DateTime _Impdate = new DateTime();
                                        string _ImpDateStr = row[0].ToString();
                                        if (!string.IsNullOrEmpty(_ImpDateStr) && DateTime.TryParse(_ImpDateStr, out _Impdate))
                                        {
                                            string _BranchID = brancList.FirstOrDefault(x => x.Field<string>("SAPPlantID").ToString() == row[1].ToString()).Field<string>("BranchID");
                                            string _VersionID = row[2].ToString();
                                            if (exAcc.All(x => x.ToString() != row[3].ToString()))
                                            {
                                                string _AccountingCode = accMstList.FirstOrDefault(x => x.Field<int>("MappingExcelID").ToString() == row[3].ToString()).Field<string>("AccountingCode");

                                                decimal _Value = 0;
                                                decimal _tmpValue = 0;
                                                if (decimal.TryParse(row[4].ToString(), out _tmpValue))
                                                    _Value = _tmpValue;

                                                if (_Value > 0)
                                                    _Value = _Value * -1;

                                                string userName = UserList.First().Key;
                                                ImpDateStr = _Impdate.ToString("yyyyMM");

                                                readDT.Rows.Add(ImpDateStr, _BranchID, _VersionID, _AccountingCode, _Value, userName, _uDate);
                                            }
                                        }
                                    }
                                }
                            }

                            if (readDT != null && readDT.Rows.Count > 0)
                            {
                                Dictionary<string, object> _prmt_p = new Dictionary<string, object>();
                                _prmt_p.Add("ImpDate", ImpDateStr);
                                _prmt_p.Add("Version", ddlVersion.SelectedValue);
                                Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_prepare_data", _prmt_p);

                                SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                                bulkInsert.DestinationTableName = tableName;
                                bulkInsert.WriteToServer(readDT);
                                ret = true;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(errMsg))
                                    lblUploadResult.Text = "เกิดข้อผิดพลาด กรุณาตรวจสอบ excel อีกครั้ง!!!";
                                else
                                    lblUploadResult.Text = errMsg;

                                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                                ret = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (string.IsNullOrEmpty(errMsg))
                lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
            else
                lblUploadResult.Text = errMsg;

            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            ret = false;
        }

        return ret;
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
                    results.Add(UploadExcel(FileUpload1, "tbl_allc_daily_pl_item_import"));
                }
            }

            if (results.Count > 0 && results.All(x => x))
            {
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

    private void InitPage()
    {
        try
        {
            List<string> sheetNameList = new List<string>();

            for (int i = 1; i <= 12; i++)
            {
                sheetNameList.Add("M" + i.ToString() + " Actual");
                sheetNameList.Add("M" + i.ToString() + " Assumption");
            }
            Session["SheetNameList"] = sheetNameList;

            //Dictionary<string, string> docStatusList = new Dictionary<string, string>();
            //docStatusList.Add("-1", "---ทั้งหมด---");
            //docStatusList.Add("1", "ศูนย์ส่งมาแล้ว");
            //docStatusList.Add("0", "ยังไม่ส่งข้อมูล");
            //ddlDocStatus.BindDropdownList(docStatusList);
            //ddlDocStatus.SelectedValue = "-1";

            Dictionary<string, string> versionList = new Dictionary<string, string>();
            versionList.Add("Assumption", "Assumption");
            versionList.Add("Actual", "Actual");
            ddlVersion.BindDropdownList(versionList);

            Dictionary<string, string> versionListStatus = new Dictionary<string, string>();
            versionListStatus.Add("-1", "---ทั้งหมด---");
            versionListStatus.Add("Assumption", "Assumption");
            versionListStatus.Add("Actual", "Actual");
            ddlVersionStatus.BindDropdownList(versionListStatus);
            ddlVersionDT.BindDropdownList(versionListStatus);

            var _dt = new DataTable();
            _dt = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_branch", null);

            DataRow sRow = _dt.NewRow();
            sRow["BranchID"] = "-1";
            sRow["BranchName2"] = "---ทั้งหมด---";
            _dt.Rows.InsertAt(sRow, 0);

            ddlBranch.BindDropdownList(_dt, "BranchName2", "BranchID");
            ddlBranch.SelectedIndex = 0;

        }
        catch (Exception ex)
        {

            throw ex;
        }
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

    private void Search()
    {
        try
        {
            string _version = ddlVersionStatus.SelectedValue.ToString();

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("Version", _version);


            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_status", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdStatus, t_DT);
                ViewState["grdStatus"] = t_DT;

                btnExportExcel.Visible = true;
            }
            else
            {
                BindGridView(grdStatus, null);
                ViewState["grdStatus"] = null;

                btnExportExcel.Visible = false;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SearchDT(GridView grd, string pk = null)
    {
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            string _impdate = "";
            string _version = "";
            string _branchID = "";

            _branchID = ddlBranch.SelectedValue;

            if (pk != null && !string.IsNullOrEmpty(pk))
            {
                _impdate = pk.Substring(0, 6);
                _version = pk.Substring(6, pk.Length - 6);
            }
            else
            {
                string _transferDate = "";
                string tfDate = "";
                if (!string.IsNullOrEmpty(txtTransferDateS.Text))
                {
                    var _td = txtTransferDateS.Text.Split('/').ToList();
                    _transferDate = string.Join("/", _td[1], _td[0], _td[2]);

                    tfDate = Convert.ToDateTime(_transferDate).ToString("yyyyMM");
                }
                else
                    tfDate = "-1";

                _impdate = tfDate;
                _version = ddlVersionDT.SelectedValue;
            }

            t_prmt.Add("ImpDate", _impdate);
            t_prmt.Add("Version", _version);
            t_prmt.Add("BranchID", _branchID);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_details", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grd, t_DT);
                ViewState[grd.ID] = t_DT;

                btnExcel_DT.Visible = true;
            }
            else
            {
                BindGridView(grd, null);
                ViewState[grd.ID] = null;

                btnExcel_DT.Visible = false;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void SearchPLDT(GridView grd, string pk)
    {
        try
        {
            if (!string.IsNullOrEmpty(pk))
            {
                Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                string _impdate = "";
                string _version = "";
                string _branchID = "";
                string _accCode = "";

                _impdate = pk.Substring(0, 6);
                _branchID = pk.Substring(6, 3);
                _accCode = pk.Substring(9, 3);
                _version = pk.Substring(12, pk.Length - 12);

                t_prmt.Add("ImpDate", _impdate);
                t_prmt.Add("Version", _version);
                t_prmt.Add("BranchID", _branchID);
                t_prmt.Add("AccCode", _accCode);

                DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_allcash_daily_pl_get_pl_details", t_prmt);
                if (t_DT != null && t_DT.Rows.Count > 0)
                {
                    BindGridView(grd, t_DT);
                    ViewState[grd.ID] = t_DT;

                    btnExportPLDT.Visible = true;
                }
                else
                {
                    BindGridView(grd, null);
                    ViewState[grd.ID] = null;

                    btnExportPLDT.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
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


    private bool UploadExcel(FileUpload fileUP, string tableName)
    {
        bool result = false;

        try
        {
            if (fileUP.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(fileUP.FileName))
                {
                    result = AddExcelToDB(fileUP, tableName);

                    //foreach (var _vanID in _result.Distinct().ToList())
                    //{                      
                    //    string user = UserList.First().Key;

                    //    Dictionary<string, object> _prmt = new Dictionary<string, object>();
                    //    _prmt.Add("BranchID", "");// ddlBranch.SelectedValue);
                    //    _prmt.Add("VanID", _vanID);
                    //    _prmt.Add("TransferDate", _transDate);
                    //    _prmt.Add("UpdateBy", user);
                    //    Helper.ExecuteProcedureOBJ(conString, "proc_branch_expenses_update_data", _prmt);
                    //}
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


    #endregion

    #region event methods

    protected void btnSearchBE_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdStatus"] != null)
            {
                DataTable _dt = (DataTable)ViewState["grdStatus"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานการส่งข้อมูลและคำนวน P&L.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void ddlBranch_R_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdStatus.PageIndex;
        var dv = SortDataTable((DataTable)ViewState["grdStatus"], false); ;
        grdStatus.DataSource = dv.ToTable();
        grdStatus.DataBind();
        for (int i = 1; i < grdStatus.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdStatus.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdStatus.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdStatus.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void grdStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "report")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string _pk = "";
                _pk = grdStatus.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    SearchDT(grdDetails, _pk);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grdStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            if (j == 2)
            {
                e.Row.Cells[j].Visible = false;
            }
            if (j == 3 || j == 4 || j == 7 || j == 8)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }


            if (e.Row.RowIndex > -1)
            {
                var cell = e.Row.Cells[j];
                if (j == 5)
                {
                    if (cell.Text == "ส่งข้อมูลเรียบร้อยแล้ว")
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#92D050");
                        cell.Font.Bold = true;
                    }
                    else
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE699");
                        cell.Font.Bold = true;
                    }
                }
                if (j == 6)
                {
                    if (cell.Text == "คำนวนเรียบร้อยแล้ว")
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#92D050");
                        cell.Font.Bold = true;
                    }
                    else
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE699");
                        cell.Font.Bold = true;
                    }
                }
            }
        }
    }

    protected void grdStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void grdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "report")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string _pk = "";
                _pk = grdDetails.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    SearchPLDT(grdPLDT, _pk);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowDT(3)", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            if (j == 1)
            {
                e.Row.Cells[j].Visible = false;
            }
            if (j == 3 || j == 4 || j == 8 || j == 9)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            if (j == 7)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
            }
        }
    }

    protected void grdDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdDetails.PageIndex;
        var dv = SortDataTable((DataTable)ViewState["grdDetails"], false); ;
        grdDetails.DataSource = dv.ToTable();
        grdDetails.DataBind();
        for (int i = 1; i < grdDetails.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdDetails.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdDetails.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdDetails.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void ddlBranch_DT_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSearchDT_Click(object sender, EventArgs e)
    {
        SearchDT(grdDetails, null);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void btnExcel_DT_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdDetails"] != null)
            {
                DataTable _dt = (DataTable)ViewState["grdDetails"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานการ Import P&L.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnExportPLDT_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdPLDT"] != null)
            {
                DataTable _dt = (DataTable)ViewState["grdPLDT"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานผลการคำนวน P&L.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdPLDT_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdPLDT.PageIndex;
        var dv = SortDataTable((DataTable)ViewState["grdPLDT"], false); ;
        grdPLDT.DataSource = dv.ToTable();
        grdPLDT.DataBind();
        for (int i = 1; i < grdPLDT.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdPLDT.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdPLDT.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdPLDT.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowDT(3)", true);
    }

    protected void grdPLDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            if (j == 1)
            {
                e.Row.Cells[j].Visible = false;
            }
            if (j == 3 || j == 5 || j == 7)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            if (j == 6)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
            }
        }
    }

    protected void grdDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdDetails"] != null)
        {
            grdDetails.PageIndex = e.NewPageIndex;
            BindGridView(grdDetails, (DataTable)ViewState["grdDetails"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
    }

    #endregion

}