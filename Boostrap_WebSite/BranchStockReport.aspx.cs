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

public partial class BranchStockReport : System.Web.UI.Page
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
        string dataList = "";

        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "upload")
                ImportExcel();
            if (requestTarget == "savePO")
                if (this.Request["__EVENTARGUMENT"] != null)
                {
                    dataList = this.Request["__EVENTARGUMENT"].ToString();

                    SavePO(dataList);
                }

            if (requestTarget == "saveWH")
            {

                if (this.Request["__EVENTARGUMENT"] != null)
                {
                    dataList = this.Request["__EVENTARGUMENT"].ToString();

                    SaveWH(dataList);
                }
            }

            if (requestTarget == "removePO")
            {
                if (this.Request["__EVENTARGUMENT"] != null)
                {
                    string pk = this.Request["__EVENTARGUMENT"].ToString();

                    if (!string.IsNullOrEmpty(pk) && pk != "undefined")
                    {
                        int _pk = Convert.ToInt32(pk);
                        RemovePO(_pk);
                    }
                    else
                    {
                        lblUploadResult.Text = "ไม่พบข้อมูลที่ต้องการลบ!!!";
                        lblUploadResult.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload(); ActiveTab(2);", true);
                        return;
                    }
                }
            }
        }

        rdoc1 = new ReportDocument();
    }

    #region Private Methods
    private void InitPage()
    {
        Dictionary<string, string> SCDList = new Dictionary<string, string>();
        for (int i = 1; i <= 31; i++)
        {
            SCDList.Add(i.ToString(), i.ToString() + " วัน");
        }
        ddlStockCD.BindDropdownList(SCDList);
        ddlStockCD.SelectedValue = "7";

        Dictionary<string, string> docStatusList = new Dictionary<string, string>();
        docStatusList.Add("-1", "---ทั้งหมด---");
        docStatusList.Add("1", "ศูนย์ส่งมาแล้ว");
        docStatusList.Add("0", "ยังไม่ส่งข้อมูล");
        ddlDocStatus.BindDropdownList(docStatusList);
        ddlDocStatus.SelectedValue = "-1";

        DataTable _docstatus_dt = new DataTable();

        _docstatus_dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_doc_status", null);

        DataRow _sRow = _docstatus_dt.NewRow();
        _sRow["DocStatusCode"] = "-1";
        _sRow["DocStatus"] = "---เลือกสถานะ---";
        _docstatus_dt.Rows.InsertAt(_sRow, 0);

        if (_docstatus_dt != null && _docstatus_dt.Rows.Count > 0)
        {
            ddlPODocStatus.BindDropdownList(_docstatus_dt, "DocStatus", "DocStatusCode");
            ddlWHDocStatus.BindDropdownList(_docstatus_dt, "DocStatus", "DocStatusCode");
            Session["BranchDocStatus"] = _docstatus_dt;
        }

        DataTable _per_dt = new DataTable();
        _per_dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_branch_permission", null);

        if (_per_dt != null && _per_dt.Rows.Count > 0)
        {
            foreach (DataRow item in _per_dt.Rows)
            {
                permissionList.Add(item["UserName"].ToString(), item["RoleName"].ToString());
            }
        }

        if (permissionList.Count > 0)
        {
            var user = permissionList.FirstOrDefault(x => x.Key == UserList.First().Key);
            if (user.Value != null)
            {
                DataTable _dt = new DataTable();
                DataTable _dt2 = new DataTable();
                Dictionary<string, object> b_prmt = new Dictionary<string, object>();

                if (user.Value == "adminbranch")
                {
                    btnSavePO.Visible = false;
                    string _branchID = user.Key.Substring(1, user.Key.Length - 1);

                    b_prmt.Add("BranchID", _branchID);

                    _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);
                    _dt2 = _dt.Copy();
                }
                else
                {
                    btnSavePO.Visible = true;
                    b_prmt = GetAllBranchFromDB(ref _dt, ref _dt2);
                }

                BindDDLSalesArea(ref _dt, ref _dt2, b_prmt);
            }
            else
            {
                DataTable _dt = new DataTable();
                DataTable _dt2 = new DataTable();

                Dictionary<string, object> b_prmt = GetAllBranchFromDB(ref _dt, ref _dt2);

                BindDDLSalesArea(ref _dt, ref _dt2, b_prmt);
            }
        }

        string userName = UserList.First().Key;
        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
        t_prmt.Add("UserName", userName);

        string roleName = "";

        DataTable _userDt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_user", t_prmt);
        if (_userDt != null && _userDt.Rows.Count > 0)
            roleName = _userDt.Rows[0]["RoleName"].ToString();

        txtStatusDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtDocDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtPONo.Text = DateTime.Now.ToString("yyMM") + "001";
        if (!string.IsNullOrEmpty(roleName) && roleName != "adminbranch")
        {
            SearchStatus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
    }

    private Dictionary<string, object> GetAllBranchFromDB(ref DataTable _dt, ref DataTable _dt2)
    {
        Dictionary<string, object> b_prmt = new Dictionary<string, object>();
        b_prmt.Add("BranchID", "-1");

        _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);
        _dt2 = _dt.Copy();

        DataRow sRow = _dt2.NewRow();
        sRow["BranchID"] = "-1";
        sRow["BranchName"] = "---ทั้งหมด---";
        _dt2.Rows.InsertAt(sRow, 0);

        return b_prmt;
    }

    private void BindDDLSalesArea(ref DataTable _dt, ref DataTable _dt2, Dictionary<string, object> b_prmt)
    {
        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlSalesArea.BindDropdownList(_dt, "BranchName", "BranchID");
            ddlSalesAreaDT.BindDropdownList(_dt2, "BranchName", "BranchID");
            ddlSalesAreaS.BindDropdownList(_dt2, "BranchName", "BranchID");

            DataTable __dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);
            DataTable __dt2 = __dt.Copy();

            DataRow _sRow1 = __dt2.NewRow();
            _sRow1["BranchID"] = "-1";
            _sRow1["BranchName"] = "---สาขาทั้งหมด---";
            __dt2.Rows.InsertAt(_sRow1, 0);

            DataRow _sRow2 = __dt2.NewRow();
            _sRow2["BranchID"] = "-2";
            _sRow2["BranchName"] = "---ผลรวมทั้งหมด---";
            __dt2.Rows.InsertAt(_sRow2, 1);
            ddlSalesArea_BSR.BindDropdownList(__dt2, "BranchName", "BranchID");
        }
    }

    private bool AddExcelToDB(FileUpload fileUP, string sheetName, string storeName, string tableName, string tempStoreName)
    {
        bool result = false;
        if (fileUP.HasFile)
        {
            string path = string.Concat(Server.MapPath("~/App_Data/" + fileUP.FileName));
            fileUP.SaveAs(path);

            var dataTable = new DataTable();
            dataTable = Helper.ReadExcelToDataTable(conString, fileUP.FileName, sheetName);

            //fileUP.SaveAs(path);

            //string serverExcelPath = excelPath + fileUP.FileName;

            //string cmd = " SELECT * ";
            //cmd += " FROM OPENROWSET ( ";
            //cmd += " 'Microsoft.ACE.OLEDB.12.0', ";
            //cmd += string.Format(" 'Excel 8.0;Database={0};', ", serverExcelPath);
            //cmd += string.Format(" 'SELECT * FROM [{0}$]' ", sheetName);
            //cmd += " )";

            //var dataTable = new DataTable();
            //dataTable = Helper.ExecuteProcedureToTable(conString, cmd, System.Data.CommandType.Text, null);

            DataTable readDT = new DataTable(tableName);

            if (tableName == "tbl_balance_qty_by_wh_temp")
            {
                readDT.Columns.Add("BranchID", typeof(string));
                readDT.Columns.Add("RowNo", typeof(int));
                readDT.Columns.Add("ProductCode", typeof(string));
                readDT.Columns.Add("ProductName", typeof(string));
                readDT.Columns.Add("ChestQty", typeof(int));
                readDT.Columns.Add("ChestUom", typeof(string));
                readDT.Columns.Add("PackQty", typeof(int));
                readDT.Columns.Add("PackUom", typeof(string));
                readDT.Columns.Add("SalesPrice", typeof(decimal));
                readDT.Columns.Add("DocDate", typeof(DateTime));
                readDT.Columns.Add("Update_Date", typeof(DateTime));

                foreach (DataRow row in dataTable.Rows)
                {
                    string _rowNo = "";
                    _rowNo = row[0].ToString();

                    int rowNo = 0;
                    if (int.TryParse(_rowNo, out rowNo))
                    {
                        string BranchID = ddlSalesArea.SelectedValue.ToString();
                        string ProductCode = row[1].ToString();
                        string ProductName = row[2].ToString();
                        int ChestQty = Convert.ToInt32(row[3]);
                        string ChestUom = row[4].ToString();
                        int PackQty = Convert.ToInt32(row[5]);
                        string PackUom = row[6].ToString();
                        decimal SalesPrice = Convert.ToDecimal(row[7]);

                        string _DocDate = "";
                        var _dDate = txtDocDate.Text.Split('/').ToList();
                        _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
                        DateTime DocDate = Convert.ToDateTime(_DocDate);

                        readDT.Rows.Add(BranchID, rowNo, ProductCode, ProductName, ChestQty, ChestUom, PackQty, PackUom, SalesPrice, DocDate, DateTime.Now);
                    }
                }
            }
            else if (tableName == "tbl_branch_stock_od_temp")
            {
                readDT.Columns.Add("BranchID", typeof(string));
                readDT.Columns.Add("ProductCode", typeof(string));
                readDT.Columns.Add("ProductName", typeof(string));
                readDT.Columns.Add("SaleUom", typeof(string));
                readDT.Columns.Add("SalesPrice", typeof(decimal));
                readDT.Columns.Add("OrderQty", typeof(int));
                readDT.Columns.Add("TotalSalesPrice", typeof(decimal));
                readDT.Columns.Add("DocDate", typeof(DateTime));
                readDT.Columns.Add("PONo", typeof(string));
                readDT.Columns.Add("Update_Date", typeof(DateTime));
                readDT.Columns.Add("Seq", typeof(int));
                readDT.Columns.Add("Remark", typeof(string));

                int rowIndex = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    if (rowIndex >= 5)
                    {
                        if (row[2] != null && row[2] != DBNull.Value && !string.IsNullOrEmpty(row[2].ToString()))
                        {
                            string BranchID = ddlSalesArea.SelectedValue.ToString();
                            string ProductCode = row[2].ToString();
                            string ProductName = row[3].ToString();
                            string SaleUom = "หีบ"; //row[4].ToString();
                            decimal SalesPrice = 0;

                            var cell1 = row[1];
                            var cell4 = row[4];
                            var cell5 = row[5];
                            var cell6 = row[6];
                            var cell7 = row[7];

                            if (cell4 != null && cell4 != DBNull.Value && !string.IsNullOrEmpty(cell4.ToString()) && cell4.ToString() != "-")
                                SalesPrice = Convert.ToDecimal(cell4);

                            decimal OrderQty = 0;
                            if (cell5 != null && cell5 != DBNull.Value && !string.IsNullOrEmpty(cell5.ToString()) && cell5.ToString() != "-")
                                OrderQty = Convert.ToDecimal(cell5);

                            string _DocDate = "";
                            var _dDate = txtDocDate.Text.Split('/').ToList();
                            _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
                            DateTime DocDate = Convert.ToDateTime(_DocDate);

                            string poNo = txtPONo.Text;

                            decimal TotalSalesPrice = 0;

                            string refSAPPO = "";

                            int seq = -1;
                            if (cell1 != null && cell1 != DBNull.Value && !string.IsNullOrEmpty(cell1.ToString()))
                                seq = Convert.ToInt32(cell1);

                            if (cell6 != null && cell6 != DBNull.Value && !string.IsNullOrEmpty(cell6.ToString()) && cell6.ToString() != "-")
                                TotalSalesPrice = Convert.ToDecimal(cell6);

                            if (cell7 != null && cell7 != DBNull.Value && !string.IsNullOrEmpty(cell7.ToString()))
                                refSAPPO = cell7.ToString();

                            readDT.Rows.Add(BranchID, ProductCode, ProductName, SaleUom, SalesPrice, OrderQty, TotalSalesPrice, DocDate, poNo, DateTime.Now, seq, refSAPPO);
                        }
                        else
                            break;
                    }

                    rowIndex++;
                }
            }

            if (readDT != null && readDT.Rows.Count > 0)
            {
                // Bulk Copy to SQL Server 
                SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                bulkInsert.DestinationTableName = tableName;
                bulkInsert.WriteToServer(readDT);
                result = true;
            }
        }

        return result;
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
            if (string.IsNullOrEmpty(FileUpload1.FileName) && string.IsNullOrEmpty(FileUpload2.FileName))
            {
                lblUploadResult.Text = "กรุณาเลือก excel!";
                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);

                return;
            }

            if (FileUpload1.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(FileUpload1.FileName))
                    results.Add(UploadExcel(FileUpload1, "RptBalanceQty_ByWH", "proc_branch_stock_report_update_qty_by_wh", "tbl_balance_qty_by_wh_temp", "proc_branch_stock_report_clear_wh_temp"));
            }
            if (FileUpload2.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(FileUpload2.FileName))
                    results.Add(UploadExcel(FileUpload2, "PO1", "proc_branch_stock_report_update_stock_od", "tbl_branch_stock_od_temp", "proc_branch_stock_report_clear_od_temp"));

                //results.Add(UploadExcel(FileUpload2, "PO1", "proc_branch_stock_report_update_stock_od", "tbl_branch_stock_od_temp", "proc_branch_stock_report_clear_od_temp"));
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

    private string GetDocStatus(string _status)
    {
        string status = "";
        if (Session["BranchDocStatus"] != null)
        {
            DataTable dt = Session["BranchDocStatus"] as DataTable;
            foreach (DataRow row in dt.Rows)
            {
                if (row["DocStatusCode"].ToString() == _status)
                {
                    status = row["DocStatus"].ToString();
                    break;
                }
            }
        }

        return status;
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

    private void GenExcel(string sessionName, string storeName, string topicExcel, bool isSendSAP = false)
    {
        if (Session[sessionName] != null)
        {
            try
            {
                Dictionary<string, DataTable> _data = (Dictionary<string, DataTable>)Session[sessionName];

                DataTable _dt = new DataTable();

                string reportName = ""; // "PO_05.06.2020_หน่วยอุดรธานี";
                int pk = Convert.ToInt32(_data.First().Key.Split('|')[0]);

                if (sessionName == "PO_DATA")
                {
                    if (!CheckExportExcel(pk))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "VerifyExportExcel(2)", true);
                        return;
                    }

                    if (isSendSAP)
                    {
                        UpdateDocSendToSAP(pk);

                        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSavePO(2);", true); //ShowEditPOPopup(_pk); 

                        //SearchStatus(false);

                        //RefreshPO();
                    }    
                }

                reportName = GetExcelName(topicExcel, pk, storeName);
                string excelName = "";

                if (isSendSAP)
                {
                    Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                    t_prmt.Add("PK", pk);

                    var _dt_temp = new DataTable();
                    _dt_temp = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_stock_od_to_SAP", t_prmt);

                    excelName = reportName + "_to_SAP.xlsx";

                    if (_dt_temp != null && _dt_temp.Rows.Count > 0)
                        _dt = _dt_temp;
                }
                else
                {
                    var _tempDT = _data.First().Value;
                    if (_tempDT != null && _tempDT.Rows.Count > 0 && _tempDT.Rows[1]["รหัสสาขา"].ToString() == "103")
                    {
                        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                        t_prmt.Add("PK", pk);

                        _dt = new DataTable();
                        _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_stock_od_103", t_prmt);
                    }
                    else
                        _dt = _data.First().Value;

                    excelName = reportName + ".xlsx";
                }
                    
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + excelName);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }

                //SearchStatus();
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.Message);
                throw ex;
            }
        }
    }

    private string GetExcelName(string topicExcel, int pk, string storeName)
    {
        string result = "";
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("PK", pk);

            DataTable _dtBranch = Helper.ExecuteProcedureToTable(conString, storeName, t_prmt);
            if (_dtBranch != null && _dtBranch.Rows.Count > 0)
            {
                result = string.Join("_", topicExcel, Convert.ToDateTime(_dtBranch.Rows[0]["DocDate"]).ToString("dd.MM.yyyy"), _dtBranch.Rows[0]["BranchName"].ToString());
            }
        }
        catch (Exception ex)
        {
            result = "";
            throw ex;
        }

        return result;
    }

    private string GetDocStatusCode(int pk)
    {
        string ret = "";
        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
        t_prmt.Add("PK", pk);

        DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_doc_status_by_pk", t_prmt);
        if (t_DT != null && t_DT.Rows.Count > 0)
        {
            ret = t_DT.Rows[0]["DocStatusCode"].ToString();
        }

        return ret;
    }

    private void GenPattern1Report()
    {
        try
        {
            if (Session["PO_DATA"] != null)
            {
                Dictionary<string, DataTable> _data = (Dictionary<string, DataTable>)Session["PO_DATA"];

                string pk = _data.First().Key.Split('|')[0];

                Dictionary<string, object> _p = new Dictionary<string, object>();
                _p.Add("PK", Convert.ToInt32(pk));

                DataTable dt = new DataTable();
                dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_print_po", _p);

                rdoc1.Load(Server.MapPath("proc_Branch_PO.rpt"));

                rdoc1.SetDataSource(dt);

                //rdoc1.SetParameterValue("?PK", docDate);

                CrystalReportViewer1.RefreshReport();

                CrystalReportViewer1.ReportSource = rdoc1;

                CrystalReportViewer1.DataBind();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void RefreshPO()
    {
        Dictionary<string, DataTable> _data = (Dictionary<string, DataTable>)Session["PO_DATA"];

        int pk = Convert.ToInt32(_data.First().Key.Split('|')[0]);
        SearchPO(pk);

        string userName = UserList.First().Key;
        SetDefaultStatus(ddlPODocStatus, userName);
    }

    private void Search()
    {
        try
        {
            string StockCD = ddlStockCD.SelectedValue.ToString();
            string BranchID = ddlSalesAreaDT.SelectedValue.ToString();
            //string from = "";
            //string to = "";
            string docDate = "";

            //var _from = txtTranDate1.Text.Split('/').ToList();
            //from = string.Join("/", _from[1], _from[0], _from[2]);

            //var _to = txtTranDate2.Text.Split('/').ToList();
            //to = string.Join("/", _to[1], _to[0], _to[2]);

            var _dd = txtDocDateS.Text.Split('/').ToList();
            docDate = string.Join("/", _dd[1], _dd[0], _dd[2]);

            string poNo = txtPONoR.Text;

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("StockCoverDay", StockCD);
            //t_prmt.Add("DateFrom", from);
            //t_prmt.Add("DateTo", to);
            t_prmt.Add("BranchID", BranchID);
            t_prmt.Add("DocDate", docDate);
            t_prmt.Add("PONo", poNo);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_suggest_order_allocate_get", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdSOA, t_DT);
                ViewState["grdSOA"] = t_DT;

                btnExportExcel.Visible = true;
            }
            else
            {
                BindGridView(grdSOA, null);
                ViewState["grdSOA"] = null;

                btnExportExcel.Visible = false;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SearchBSR()
    {
        try
        {
            string BranchID = ddlSalesArea_BSR.SelectedValue.ToString();

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("BranchID", BranchID);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_branch_stock", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdBSR, t_DT);
                ViewState["grdBSR"] = t_DT;

                btnExportExcelBSR.Visible = true;
            }
            else
            {
                BindGridView(grdBSR, null);
                ViewState["grdBSR"] = null;

                btnExportExcelBSR.Visible = false;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private DataTable SearchEditData(GridView grd, int pk, string storeName, string funcName)
    {
        DataTable _dt = new DataTable();
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("PK", pk);

            _dt = Helper.ExecuteProcedureToTable(conString, storeName, t_prmt);
            string refSAPPO = "";
            string docStatus = "";

            if (_dt != null && _dt.Rows.Count > 0)
            {
                if (storeName == "proc_branch_stock_report_get_stock_od")
                {
                    refSAPPO = _dt.Rows[1]["RefSAPPO"].ToString();
                    docStatus = _dt.Rows[1]["DocStatus"].ToString();
                }

                BindGridView(grd, _dt);
                ViewState[grd.ID] = _dt;
            }
            else
            {
                BindGridView(grd, null);
                ViewState[grd.ID] = null;
            }

            if (storeName == "proc_branch_stock_report_get_stock_od")
            {
                txtRefSAPPO.Text = refSAPPO;
                txtCurDocStatus.Text = docStatus;

                string userName = UserList.First().Key;
                Dictionary<string, object> u_prmt = new Dictionary<string, object>();
                u_prmt.Add("UserName", userName);

                string roleName = "";

                DataTable role_dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_user", u_prmt);
                if (role_dt != null && role_dt.Rows.Count > 0)
                {
                    roleName = role_dt.Rows[0]["RoleName"].ToString();

                    bool checkSendSAP = false;
                    bool checkPrint = false;
                    bool checkRefSAP = false;
                    if (roleName == "admincenter")
                    {
                        if (docStatus == "ส่งเข้า sap แล้ว")
                        {
                            checkRefSAP = true;
                            checkSendSAP = true;
                            if (!string.IsNullOrEmpty(txtRefSAPPO.Text))
                                checkPrint = true;
                        }
                        else if (docStatus == "ผู้จัดการตรวจสอบแล้ว")
                        {
                            checkSendSAP = true;
                        }
                    }

                    lblRefSAPPO.Visible = checkRefSAP;
                    txtRefSAPPO.Visible = checkRefSAP;

                    EXCrytalReport.Visible = checkPrint;

                    ExportToSAP.Visible = checkSendSAP;
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", funcName, true);

            return _dt;
        }
        catch (Exception ex)
        {
            return null;
            throw ex;
        }
    }

    private void SearchPO(int pk)
    {
        Dictionary<string, DataTable> sesData = new Dictionary<string, DataTable>();

        //string docStatusCode = GetDocStatusCode(pk);
        //ddlPODocStatus.SelectedValue = docStatusCode;
        //string docStatusCode = ddlPODocStatus.SelectedValue;

        string docStatusCode = "-1";

        DataTable dt = SearchEditData(grdEditPO, pk, "proc_branch_stock_report_get_stock_od", "ShowPO(2)");
        sesData.Add(pk + "|" + docStatusCode, dt);

        Session["PO_DATA"] = sesData;
    }

    private void SavePO(string poData)
    {
        try
        {
            string _pk = "";
            string whStatus = GetDocStatus(poData.Split('!')[1].ToString());
            string refSAPPO = "";
            string temp_SAPPO = poData.Split('!')[2].ToString();

            if (!string.IsNullOrEmpty(temp_SAPPO) && temp_SAPPO != "undefined")
                refSAPPO = temp_SAPPO;

            Dictionary<string, string> whDataList = new Dictionary<string, string>();

            try
            {
                List<string> grdData = poData.Split('!')[0].Split(',').ToList();
                foreach (string item in grdData)
                {
                    var _item = item.Split('|').ToList();
                    if (!string.IsNullOrEmpty(_item[0]))
                        whDataList.Add(_item[0], _item[1]);
                }
            }
            catch (Exception ex)
            {
                lblUploadResult.Text = "ข้อมูลสินค้าใน Excel ไม่ถูกต้อง กรุณาตรวจสอบอีกครั้ง!!! : " + ex.Message;
                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowErrSave(2)", true);

                return;
            }
           
            foreach (KeyValuePair<string, string> item in whDataList)
            {
                int pk = Convert.ToInt32(item.Key);
                _pk = item.Key;

                decimal orderQty = 0;
                string docStatus = whStatus; // ddlPODocStatus.SelectedItem.Text;
                string updateBy = UserList.First().Key;

                if (!string.IsNullOrEmpty(item.Value))
                    orderQty = Convert.ToDecimal(item.Value);

                Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                t_prmt.Add("PK", pk);
                t_prmt.Add("OrderQty", orderQty);
                t_prmt.Add("UpdateBy", updateBy);
                t_prmt.Add("DocStatus", docStatus);
                t_prmt.Add("RefSAPPO", refSAPPO);

                Helper.ExecuteProcedureOBJ(conString, "proc_branch_stock_report_save_od", t_prmt);
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSavePO(2);", true); //ShowEditPOPopup(_pk); 

            SearchStatus(false);

            RefreshPO();
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "เกิดข้อผิดพลาดระหว่างบันทึกข้อมูล กรุณาตรวจสอบอีกครั้ง!!!:" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowErrSave(2)", true);
        }
    }

    private void RemovePO(int pk)
    {
        try
        {
            bool delFalg = false;
            //check is admin center-----------------------------------------
            string userName = UserList.First().Key;
            Dictionary<string, object> u_prmt = new Dictionary<string, object>();
            u_prmt.Add("UserName", userName);

            string roleName = "";

            DataTable _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_user", u_prmt);
            if (_dt != null && _dt.Rows.Count > 0)
                roleName = _dt.Rows[0]["RoleName"].ToString();

            if (roleName == "admincenter")
                delFalg = true;

            //check is admin center-----------------------------------------

            if (!delFalg)
            {
                lblUploadResult.Text = "ผู้ใช้งานนี้ ไม่มีสิทธิ์ในการลบข้อมูล!!!";
                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload(); ActiveTab(2);", true);
                return;
            }
            else
            {
                Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                t_prmt.Add("PK", pk);

                Helper.ExecuteProcedureOBJ(conString, "proc_branch_stock_report_remove_od", t_prmt);

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "RemoveResult(2);", true); //ShowEditPOPopup(_pk); 

                SearchStatus(true);
            }
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "เกิดข้อผิดพลาดระหว่างลบข้อมูล กรุณาตรวจสอบอีกครั้ง!!!:" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload(); ActiveTab(2);", true);
        }
    }

    private void SearchWH(int pk)
    {
        Dictionary<string, DataTable> sesData = new Dictionary<string, DataTable>();

        string docStatusCode = GetDocStatusCode(pk);
        ddlWHDocStatus.SelectedValue = docStatusCode;

        DataTable dt = SearchEditData(grdEditWH, pk, "proc_branch_stock_report_get_qty_by_wh", "ShowWH(2)");
        sesData.Add(pk + "|" + docStatusCode, dt);

        Session["WH_DATA"] = sesData;
    }

    private void SaveWH(string whData)
    {
        try
        {
            Dictionary<string, string> whDataList = new Dictionary<string, string>();

            string whStatus = GetDocStatus(whData.Split('!')[1].ToString());

            List<string> grdData = whData.Split('!')[0].Split(',').ToList();
            foreach (string item in grdData)
            {
                var _item = item.Split('|').ToList();
                if (!string.IsNullOrEmpty(_item[0]))
                    whDataList.Add(_item[0], _item[1]);
            }

            foreach (KeyValuePair<string, string> item in whDataList)
            {
                int pk = Convert.ToInt32(item.Key); // Convert.ToInt32(grd.DataKeys[row.RowIndex].Values[0]);

                decimal chestQty = 0;
                string docStatus = whStatus; // ddlWHDocStatus.SelectedItem.Text;
                string updateBy = UserList.First().Key;

                if (!string.IsNullOrEmpty(item.Value))
                    chestQty = Convert.ToDecimal(item.Value);

                Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                t_prmt.Add("PK", pk);
                t_prmt.Add("ChestQty", chestQty);
                t_prmt.Add("UpdateBy", updateBy);
                t_prmt.Add("DocStatus", docStatus);

                Helper.ExecuteProcedureOBJ(conString, "proc_branch_stock_report_save_wh", t_prmt);
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveResult(2);", true);
            SearchStatus();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void SearchStatus(bool isActive = true)
    {
        try
        {
            string BranchID = ddlSalesAreaS.SelectedValue.ToString();
            string docDate = "";
            string sendStatus = ddlDocStatus.SelectedValue.ToString();

            var _dd = txtStatusDate.Text.Split('/').ToList();
            docDate = string.Join("/", _dd[1], _dd[0], _dd[2]);

            string poNo = txtPOS.Text;
            string user = UserList.First().Key;

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("BranchID", BranchID);
            t_prmt.Add("DocDate", docDate);
            t_prmt.Add("PONo", poNo);
            t_prmt.Add("UpdateBy", user);
            t_prmt.Add("SendStatus", Convert.ToInt32(sendStatus));

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_status_report", t_prmt);
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

            if (isActive)
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void SetDefaultStatus(DropDownList ddl, string userName)
    {
        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
        t_prmt.Add("UserName", userName);

        string roleName = "";

        DataTable _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_user", t_prmt);
        if (_dt != null && _dt.Rows.Count > 0)
        {
            roleName = _dt.Rows[0]["RoleName"].ToString();
        }

        string selectedValue = "";
        switch (roleName)
        {
            case "admincenter":
                {
                    selectedValue = "2";
                    chkAdminApprove.Visible = true;
                }
                break;
            case "mktleader": { selectedValue = "3"; } break;

            default:
                { selectedValue = "1"; }
                break;
        }

        ddl.SelectedValue = selectedValue;
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

    private void ShowEditPOPopup(string _pk)
    {
        if (!string.IsNullOrEmpty(_pk))
        {
            int _PK = Convert.ToInt32(_pk);
            if (_PK != 0)
            {
                SearchPO(_PK);

                string userName = UserList.First().Key;
                SetDefaultStatus(ddlPODocStatus, userName);
            }
        }
    }

    private bool UploadExcel(FileUpload fileUP, string sheetName, string storeName, string tableName, string tempStoreName)
    {
        bool result = false;

        try
        {
            if (fileUP.PostedFile != null)
            {
                if (!string.IsNullOrEmpty(fileUP.FileName))
                {
                    Dictionary<string, string> _prmt = new Dictionary<string, string>();
                    _prmt.Add("BranchID", ddlSalesArea.SelectedValue.ToString());

                    Dictionary<string, string> _prmt_u = new Dictionary<string, string>();
                    _prmt_u.Add("BranchID", ddlSalesArea.SelectedValue.ToString());

                    string _DocDate = "";
                    var _dDate = txtDocDate.Text.Split('/').ToList();
                    _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);

                    string poNo = txtPONo.Text.Trim();
                    string user = UserList.First().Key;

                    _prmt_u.Add("DocDate", _DocDate);
                    _prmt_u.Add("PONo", poNo);
                    _prmt_u.Add("UpdateBy", user);

                    Helper.ExecuteProcedure(conString, tempStoreName, _prmt);
                    bool _result = AddExcelToDB(fileUP, sheetName, storeName, tableName, tempStoreName);
                    Helper.ExecuteProcedure(conString, storeName, _prmt_u);

                    //bool _result = AddExcelToDB(con, sheetName, tableName);

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

    private void UpdateDocSendToSAP(int pk)
    {
        string docStatus = "ส่งเข้า sap แล้ว";
        string updateBy = UserList.First().Key;

        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
        t_prmt.Add("PK", pk);
        t_prmt.Add("UpdateBy", updateBy);
        t_prmt.Add("DocStatus", docStatus);

        Helper.ExecuteProcedureOBJ(conString, "proc_branch_stock_report_update_doc_status", t_prmt);
    }


    #endregion

    #region Event Methods

    protected void ExportPO_Click(object sender, EventArgs e)
    {
        if (Session["PO_DATA"] != null)
        {
            GenExcel("PO_DATA", "proc_branch_stock_report_get_doc_status_by_pk", "PO");
        }
    }

    protected void ExportToSAP_Click(object sender, EventArgs e)
    {
        if (Session["PO_DATA"] != null)
        {
            GenExcel("PO_DATA", "proc_branch_stock_report_get_doc_status_by_pk", "PO", true);
        }
    }

    protected void btnRefreshPO_Click(object sender, EventArgs e)
    {
        RefreshPO();
    }

    protected void btnRefreshWH_Click(object sender, EventArgs e)
    {
        if (Session["WH_DATA"] != null)
        {
            Dictionary<string, DataTable> _data = (Dictionary<string, DataTable>)Session["WH_DATA"];

            int pk = Convert.ToInt32(_data.First().Key.Split('|')[0]);
            SearchWH(pk);
        }
    }

    private bool CheckExportExcel(int pk)
    {
        bool ret = false;
        Dictionary<string, object> t_prmt = new Dictionary<string, object>();
        t_prmt.Add("PK", pk);

        string whStatus = "";
        DataTable _dtBranch = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_doc_status_by_pk", t_prmt);
        if (_dtBranch != null && _dtBranch.Rows.Count > 0)
            whStatus = _dtBranch.Rows[0]["DocStatusCode"].ToString();

        //check is admin center-----------------------------------------
        string userName = UserList.First().Key;
        Dictionary<string, object> u_prmt = new Dictionary<string, object>();
        u_prmt.Add("UserName", userName);

        string roleName = "";

        DataTable _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_user", u_prmt);
        if (_dt != null && _dt.Rows.Count > 0)
            roleName = _dt.Rows[0]["RoleName"].ToString();

        bool exportFlag = false;
        if (roleName == "admincenter" || whStatus == "4")
            exportFlag = true;

        //check is admin center-----------------------------------------

        //if (whStatus == "3" || whStatus == "4" || exportFlag)
        if (exportFlag)
            ret = true;

        return ret;
    }

    protected void ExportWH_Click(object sender, EventArgs e)
    {
        if (Session["WH_DATA"] != null)
        {
            GenExcel("WH_DATA", "proc_branch_stock_report_get_doc_status_by_pk", "RptBalanceQty_ByWH");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdSOA"] != null)
            {
                DataTable _dt = (DataTable)ViewState["grdSOA"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Suggest Order Allocate Report.xlsx");
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

    protected void btnSearchStatus_Click(object sender, EventArgs e)
    {
        SearchStatus();
    }

    protected void btnSearchBSR_Click(object sender, EventArgs e)
    {
        SearchBSR();
    }

    protected void ddlSalesArea_BSR_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchBSR();
    }

    protected void btnExportExcelBSR_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdBSR"] != null)
            {
                DataTable _dt = (DataTable)ViewState["grdBSR"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Branch Stock Report_"+ ddlSalesArea_BSR.SelectedItem.Text +".xlsx");
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


    protected void EXCrytalReport_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowPO(2);ShowPrint(2);", true);
    }

    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        GenPattern1Report();
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {

    }

    protected void grdSOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdSOA.PageIndex;
        grdSOA.DataSource = SortDataTable((DataTable)ViewState["grdSOA"], false);
        grdSOA.DataBind();
        for (int i = 1; i < grdSOA.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdSOA.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdSOA.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdSOA.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void grdSOA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdSOA"] != null)
        {
            grdSOA.PageIndex = e.NewPageIndex;
            BindGridView(grdSOA, (DataTable)ViewState["grdSOA"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
    }

    protected void grdStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "poEdit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdStatus.DataKeys[index].Value.ToString();

                ShowEditPOPopup(_pk);
            }
            else if (e.CommandName == "whEdit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdStatus.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    int _PK = Convert.ToInt32(_pk);
                    if (_PK != 0)
                    {
                        SearchWH(_PK);

                        string userName = UserList.First().Key;
                        SetDefaultStatus(ddlWHDocStatus, userName);

                        if (ddlWHDocStatus.SelectedValue == "5")
                            chkAdminApprove.Enabled = false;
                        else
                            chkAdminApprove.Enabled = true;
                    }
                }
            }
            else if (e.CommandName == "report")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdStatus.DataKeys[index].Value.ToString();

                ddlStockCD.SelectedValue = "7";

                Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                t_prmt.Add("PK", _pk);

                DataTable _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_doc_status_by_pk", t_prmt);
                if (_dt != null && _dt.Rows.Count > 0)
                    ddlSalesAreaDT.SelectedValue = _dt.Rows[0]["BranchID"].ToString();

                txtDocDateS.Text = txtStatusDate.Text;
                txtPONoR.Text = txtPOS.Text;

                Search();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
            }
        }
        catch (Exception ex)
        {

            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex > -1)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                if (j == 5)
                {
                    if (cell.Text == "ศูนย์ส่งมาแล้ว")
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#92D050");
                        cell.Font.Bold = true;

                        var cell7 = e.Row.Cells[7];
                        var cell8 = e.Row.Cells[8];
                        var cell5 = e.Row.Cells[5];

                        if (cell7.Text == "&nbsp;" || cell8.Text == "&nbsp;")
                        {
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00");

                            cell5.Text = "excel ไม่ครบถ้วน";
                        }
                    }
                    else if (cell.Text == "ยังไม่ส่งข้อมูล")
                    {
                        var cell7 = e.Row.Cells[7];
                        var cell8 = e.Row.Cells[8];
                        var cell5 = e.Row.Cells[5];

                        if (cell7.Text == "&nbsp;" && cell8.Text == "&nbsp;")
                        {
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE699");
                            cell.Font.Bold = true;
                        }
                        else
                        {
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00");

                            cell5.Text = "excel ไม่ครบถ้วน";
                            cell.Font.Bold = true;
                        }
                    }
                    else
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFE699");
                        cell.Font.Bold = true;
                    }
                }
                if (j == 6)
                {
                    cell.Font.Bold = true;
                    System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    switch (cell.Text)
                    {
                        case "ส่งเอกสารจากศูนย์แล้ว": { _color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); } break;
                        case "แอดมินกำลังแก้ไข": { _color = System.Drawing.ColorTranslator.FromHtml("#FFD966"); } break;
                        case "ผู้จัดการตรวจสอบแล้ว": { _color = System.Drawing.ColorTranslator.FromHtml("#9BC2E6"); } break;
                        case "ส่งเข้า sap แล้ว": { _color = System.Drawing.ColorTranslator.FromHtml("#92D050"); } break;
                        case "แอดมินตรวจสอบแล้ว": { _color = System.Drawing.ColorTranslator.FromHtml("#FFFF00"); } break;
                        default:
                            break;
                    }
                    cell.BackColor = _color;

                }
                if (j == 9)
                {
                    cell.Font.Bold = true;
                }
            }
        }
    }

    protected void grdEditPO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex == 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                cell.Font.Bold = true;
                cell.Enabled = false;
            }
        }
    }

    protected void grdEditWH_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex == 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                cell.Font.Bold = true;
                cell.Enabled = false;
            }
        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            rdoc1 = new ReportDocument();

            rdoc1.Close();
            rdoc1.Dispose();

            GC.Collect();
        }
        catch
        {

        }
    }

    protected void grdBSR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdBSR"] != null)
        {
            grdBSR.PageIndex = e.NewPageIndex;
            BindGridView(grdBSR, (DataTable)ViewState["grdBSR"]);

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
        }
    }

    protected void grdBSR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex == 0 && grdBSR.PageIndex == 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                cell.Font.Bold = true;
                cell.Enabled = false;
            }
        }
    }

    #endregion









}









//public bool AddExcelToDB(string path, string tableName)
//{
//    bool checkImportFalg = false;
//    try
//    {
//        Microsoft.Office.Interop.Excel.Application xlApp;
//        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
//        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
//        Microsoft.Office.Interop.Excel.Range range;

//        int rCnt;
//        int rw = 0;
//        int cl = 0;

//        xlApp = new Microsoft.Office.Interop.Excel.Application();
//        xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
//        xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

//        range = xlWorkSheet.UsedRange;
//        rw = range.Rows.Count;
//        cl = range.Columns.Count;

//        DataTable readDT = new DataTable(tableName);

//        if (tableName == "tbl_balance_qty_by_wh_temp")
//        {
//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("RowNo", typeof(int));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("ChestQty", typeof(int));
//            readDT.Columns.Add("ChestUom", typeof(string));
//            readDT.Columns.Add("PackQty", typeof(int));
//            readDT.Columns.Add("PackUom", typeof(string));
//            readDT.Columns.Add("SalesPrice", typeof(decimal));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));

//            for (rCnt = 1; rCnt <= rw; rCnt++)
//            {
//                var cell1 = (range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell2 = (range.Cells[rCnt, 2] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell3 = (range.Cells[rCnt, 3] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell4 = (range.Cells[rCnt, 4] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell5 = (range.Cells[rCnt, 5] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell6 = (range.Cells[rCnt, 6] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell7 = (range.Cells[rCnt, 7] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell8 = (range.Cells[rCnt, 8] as Microsoft.Office.Interop.Excel.Range).Value2;

//                if (cell1 != null)
//                {
//                    string _rowNo = cell1.ToString();

//                    int rowNo = 0;
//                    if (int.TryParse(_rowNo, out rowNo))
//                    {
//                        string BranchID = ddlSalesArea.SelectedValue.ToString();
//                        string ProductCode = cell2.ToString();
//                        string ProductName = cell3.ToString();
//                        int ChestQty = Convert.ToInt32(cell4);
//                        string ChestUom = cell5.ToString();
//                        int PackQty = Convert.ToInt32(cell6);
//                        string PackUom = cell7.ToString();
//                        decimal SalesPrice = Convert.ToDecimal(cell8);

//                        string _DocDate = "";
//                        var _dDate = txtDocDate.Text.Split('/').ToList();
//                        _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                        DateTime DocDate = Convert.ToDateTime(_DocDate);

//                        readDT.Rows.Add(BranchID, rowNo, ProductCode, ProductName, ChestQty, ChestUom, PackQty, PackUom, SalesPrice, DocDate, DateTime.Now);
//                    }
//                }
//            }
//        }
//        else if (tableName == "tbl_branch_stock_od_temp")
//        {
//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("SaleUom", typeof(string));
//            readDT.Columns.Add("OrderQty", typeof(int));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));

//            for (rCnt = 2; rCnt <= rw; rCnt++)
//            {
//                var cell1 = (range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell2 = (range.Cells[rCnt, 2] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell3 = (range.Cells[rCnt, 3] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell4 = (range.Cells[rCnt, 4] as Microsoft.Office.Interop.Excel.Range).Value2;
//                var cell5 = (range.Cells[rCnt, 5] as Microsoft.Office.Interop.Excel.Range).Value2;

//                if (cell1 != null)
//                {
//                    string _rowNo = cell1.ToString();

//                    int rowNo = 0;
//                    if (int.TryParse(_rowNo, out rowNo))
//                    {
//                        string BranchID = ddlSalesArea.SelectedValue.ToString();
//                        string ProductCode = cell2.ToString();
//                        string ProductName = cell3.ToString();
//                        string SaleUom = cell5.ToString();
//                        decimal OrderQty = Convert.ToDecimal(cell4);

//                        string _DocDate = "";
//                        var _dDate = txtDocDate.Text.Split('/').ToList();
//                        _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                        DateTime DocDate = Convert.ToDateTime(_DocDate);

//                        readDT.Rows.Add(BranchID, ProductCode, ProductName, SaleUom, OrderQty, DocDate, DateTime.Now);
//                    }
//                }
//            }
//        }

//        if (readDT != null && readDT.Rows.Count > 0)
//        {
//            // Bulk Copy to SQL Server 
//            SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
//            bulkInsert.DestinationTableName = tableName;
//            bulkInsert.WriteToServer(readDT);

//            checkImportFalg = true;
//        }

//        xlWorkBook.Close(true, null, null);
//        xlApp.Quit();

//        Marshal.ReleaseComObject(xlWorkSheet);
//        Marshal.ReleaseComObject(xlWorkBook);
//        Marshal.ReleaseComObject(xlApp);
//    }
//    catch (Exception ex)
//    {
//        lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
//        lblUploadResult.ForeColor = System.Drawing.Color.Red;
//        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);

//        checkImportFalg = false;
//    }

//    return checkImportFalg;
//}


//private bool AddExcelToDB(OleDbConnection con, string sheetName, string tableName)
//{
//    bool result = false;
//    try
//    {
//        OleDbCommand cmd = new OleDbCommand("select * from [" + sheetName + "$]", con);
//        con.Open();
//        // Create DbDataReader to Data Worksheet
//        DbDataReader dr = cmd.ExecuteReader();
//        var dataTable = new DataTable();
//        dataTable.Load(dr);

//        DataTable readDT = new DataTable(tableName);

//        if (tableName == "tbl_balance_qty_by_wh_temp")
//        {

//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("RowNo", typeof(int));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("ChestQty", typeof(int));
//            readDT.Columns.Add("ChestUom", typeof(string));
//            readDT.Columns.Add("PackQty", typeof(int));
//            readDT.Columns.Add("PackUom", typeof(string));
//            readDT.Columns.Add("SalesPrice", typeof(decimal));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));

//            foreach (DataRow row in dataTable.Rows)
//            {
//                string _rowNo = "";
//                _rowNo = row[0].ToString();

//                int rowNo = 0;
//                if (int.TryParse(_rowNo, out rowNo))
//                {
//                    string BranchID = ddlSalesArea.SelectedValue.ToString();
//                    string ProductCode = row[1].ToString();
//                    string ProductName = row[2].ToString();
//                    int ChestQty = Convert.ToInt32(row[3]);
//                    string ChestUom = row[4].ToString();
//                    int PackQty = Convert.ToInt32(row[5]);
//                    string PackUom = row[6].ToString();
//                    decimal SalesPrice = Convert.ToDecimal(row[7]);

//                    string _DocDate = "";
//                    var _dDate = txtDocDate.Text.Split('/').ToList();
//                    _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                    DateTime DocDate = Convert.ToDateTime(_DocDate);

//                    readDT.Rows.Add(BranchID, rowNo, ProductCode, ProductName, ChestQty, ChestUom, PackQty, PackUom, SalesPrice, DocDate, DateTime.Now);
//                }
//            }
//        }
//        else if (tableName == "tbl_branch_stock_od_temp")
//        {
//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("SaleUom", typeof(string));
//            readDT.Columns.Add("SalesPrice", typeof(decimal));
//            readDT.Columns.Add("OrderQty", typeof(int));
//            readDT.Columns.Add("TotalSalesPrice", typeof(decimal));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("PONo", typeof(string));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));
//            readDT.Columns.Add("Seq", typeof(int));

//            int rowIndex = 0;
//            foreach (DataRow row in dataTable.Rows)
//            {
//                if (rowIndex >= 88)
//                {

//                }
//                if (rowIndex >= 5)
//                {
//                    //string _rowNo = "";
//                    //_rowNo = row[1].ToString();

//                    //int rowNo = 0;
//                    //if (int.TryParse(_rowNo, out rowNo))
//                    if (row[2] != null && row[2] != DBNull.Value && !string.IsNullOrEmpty(row[2].ToString()))
//                    {
//                        string BranchID = ddlSalesArea.SelectedValue.ToString();
//                        string ProductCode = row[2].ToString();
//                        string ProductName = row[3].ToString();
//                        string SaleUom = "หีบ"; //row[4].ToString();
//                        decimal SalesPrice = 0;

//                        if (row[4] != null && row[4] != DBNull.Value && !string.IsNullOrEmpty(row[4].ToString()))
//                            SalesPrice = Convert.ToDecimal(row[4]);

//                        decimal OrderQty = 0;
//                        if (row[5] != null && row[5] != DBNull.Value && !string.IsNullOrEmpty(row[5].ToString()))
//                            OrderQty = Convert.ToDecimal(row[5]);

//                        string _DocDate = "";
//                        var _dDate = txtDocDate.Text.Split('/').ToList();
//                        _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                        DateTime DocDate = Convert.ToDateTime(_DocDate);

//                        string poNo = txtPONo.Text;

//                        decimal TotalSalesPrice = 0;

//                        int seq = -1;
//                        if (row[1] != null && row[1] != DBNull.Value && !string.IsNullOrEmpty(row[1].ToString()))
//                            seq = Convert.ToInt32(row[1]);

//                        if (row[6] != null && row[6] != DBNull.Value && !string.IsNullOrEmpty(row[6].ToString()) && row[6].ToString() != "-")
//                            TotalSalesPrice = Convert.ToDecimal(row[6]);

//                        readDT.Rows.Add(BranchID, ProductCode, ProductName, SaleUom, SalesPrice, OrderQty, TotalSalesPrice, DocDate, poNo, DateTime.Now, seq);
//                    }
//                    else
//                        break;
//                }

//                rowIndex++;
//            }
//        }

//        if (readDT != null && readDT.Rows.Count > 0)
//        {
//            // Bulk Copy to SQL Server 
//            SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
//            bulkInsert.DestinationTableName = tableName;
//            bulkInsert.WriteToServer(readDT);
//            result = true;
//        }

//        //BindGridview();
//        con.Close();
//    }
//    catch (Exception ex)
//    {
//        con.Close();

//        lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
//        lblUploadResult.ForeColor = System.Drawing.Color.Red;

//        result = false;
//        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
//    }

//    return result;
//}



//private bool AddExcelToDB(DataTable dt, string sheetName, string tableName)
//{
//    bool result = false;
//    try
//    {
//        var dataTable = new DataTable();
//        dataTable = dt;

//        DataTable readDT = new DataTable(tableName);

//        if (tableName == "tbl_balance_qty_by_wh_temp")
//        {

//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("RowNo", typeof(int));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("ChestQty", typeof(int));
//            readDT.Columns.Add("ChestUom", typeof(string));
//            readDT.Columns.Add("PackQty", typeof(int));
//            readDT.Columns.Add("PackUom", typeof(string));
//            readDT.Columns.Add("SalesPrice", typeof(decimal));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));

//            foreach (DataRow row in dataTable.Rows)
//            {
//                string _rowNo = "";
//                _rowNo = row[0].ToString();

//                int rowNo = 0;
//                if (int.TryParse(_rowNo, out rowNo))
//                {
//                    string BranchID = ddlSalesArea.SelectedValue.ToString();
//                    string ProductCode = row[1].ToString();
//                    string ProductName = row[2].ToString();
//                    int ChestQty = Convert.ToInt32(row[3]);
//                    string ChestUom = row[4].ToString();
//                    int PackQty = Convert.ToInt32(row[5]);
//                    string PackUom = row[6].ToString();
//                    decimal SalesPrice = Convert.ToDecimal(row[7]);

//                    string _DocDate = "";
//                    var _dDate = txtDocDate.Text.Split('/').ToList();
//                    _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                    DateTime DocDate = Convert.ToDateTime(_DocDate);

//                    readDT.Rows.Add(BranchID, rowNo, ProductCode, ProductName, ChestQty, ChestUom, PackQty, PackUom, SalesPrice, DocDate, DateTime.Now);
//                }
//            }
//        }
//        else if (tableName == "tbl_branch_stock_od_temp")
//        {
//            readDT.Columns.Add("BranchID", typeof(string));
//            readDT.Columns.Add("ProductCode", typeof(string));
//            readDT.Columns.Add("ProductName", typeof(string));
//            readDT.Columns.Add("SaleUom", typeof(string));
//            readDT.Columns.Add("SalesPrice", typeof(decimal));
//            readDT.Columns.Add("OrderQty", typeof(int));
//            readDT.Columns.Add("TotalSalesPrice", typeof(decimal));
//            readDT.Columns.Add("DocDate", typeof(DateTime));
//            readDT.Columns.Add("PONo", typeof(string));
//            readDT.Columns.Add("Update_Date", typeof(DateTime));
//            readDT.Columns.Add("Seq", typeof(int));

//            int rowIndex = 0;
//            foreach (DataRow row in dataTable.Rows)
//            {
//                if (rowIndex >= 88)
//                {

//                }
//                if (rowIndex >= 5)
//                {
//                    //string _rowNo = "";
//                    //_rowNo = row[1].ToString();

//                    //int rowNo = 0;
//                    //if (int.TryParse(_rowNo, out rowNo))
//                    if (row[2] != null && row[2] != DBNull.Value && !string.IsNullOrEmpty(row[2].ToString()))
//                    {
//                        string BranchID = ddlSalesArea.SelectedValue.ToString();
//                        string ProductCode = row[2].ToString();
//                        string ProductName = row[3].ToString();
//                        string SaleUom = "หีบ"; //row[4].ToString();
//                        decimal SalesPrice = 0;

//                        if (row[4] != null && row[4] != DBNull.Value && !string.IsNullOrEmpty(row[4].ToString()))
//                            SalesPrice = Convert.ToDecimal(row[4]);

//                        decimal OrderQty = 0;
//                        if (row[5] != null && row[5] != DBNull.Value && !string.IsNullOrEmpty(row[5].ToString()))
//                            OrderQty = Convert.ToDecimal(row[5]);

//                        string _DocDate = "";
//                        var _dDate = txtDocDate.Text.Split('/').ToList();
//                        _DocDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
//                        DateTime DocDate = Convert.ToDateTime(_DocDate);

//                        string poNo = txtPONo.Text;

//                        decimal TotalSalesPrice = 0;

//                        int seq = -1;
//                        if (row[1] != null && row[1] != DBNull.Value && !string.IsNullOrEmpty(row[1].ToString()))
//                            seq = Convert.ToInt32(row[1]);

//                        if (row[6] != null && row[6] != DBNull.Value && !string.IsNullOrEmpty(row[6].ToString()) && row[6].ToString() != "-")
//                            TotalSalesPrice = Convert.ToDecimal(row[6]);

//                        readDT.Rows.Add(BranchID, ProductCode, ProductName, SaleUom, SalesPrice, OrderQty, TotalSalesPrice, DocDate, poNo, DateTime.Now, seq);
//                    }
//                    else
//                        break;
//                }

//                rowIndex++;
//            }
//        }

//        if (readDT != null && readDT.Rows.Count > 0)
//        {
//            // Bulk Copy to SQL Server 
//            SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
//            bulkInsert.DestinationTableName = tableName;
//            bulkInsert.WriteToServer(readDT);
//            result = true;
//        }


//    }
//    catch (Exception ex)
//    {

//        lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
//        lblUploadResult.ForeColor = System.Drawing.Color.Red;

//        result = false;
//        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
//    }

//    return result;
//}
