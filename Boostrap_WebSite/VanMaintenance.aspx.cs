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

public partial class VanMaintenance : System.Web.UI.Page
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

    #region Private Methods

    private void InitPage()
    {
        try
        {
            Dictionary<string, string> docStatusList = new Dictionary<string, string>();
            docStatusList.Add("-1", "---ทั้งหมด---");
            docStatusList.Add("1", "ศูนย์ส่งมาแล้ว");
            docStatusList.Add("0", "ยังไม่ส่งข้อมูล");
            ddlDocStatus.BindDropdownList(docStatusList);
            ddlDocStatus.SelectedValue = "-1";

            Dictionary<string, string> rankingList = new Dictionary<string, string>();
            rankingList.Add("1", "Top 10 ค่าซ่อมบำรุง");
            rankingList.Add("2", "Top 10 ค่าเฉลี่ยนการซ่อมบำรุง");
            rankingList.Add("3", "เรียงลำดับเอง");

            ddlRankingType.BindDropdownList(rankingList);
            ddlRankingType.SelectedValue = "-1";

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
                DataTable _dt1 = new DataTable();
                DataTable _dt2 = new DataTable();
                Dictionary<string, object> b_prmt = new Dictionary<string, object>();

                bool flagUser = false;
                var user = permissionList.FirstOrDefault(x => x.Key == UserList.First().Key);

                if (user.Value != null)
                {
                    if (user.Value == "adminbranch")
                        flagUser = true;
                }

                if (!flagUser)
                    b_prmt = GetAllBranchFromDB(ref _dt1, ref _dt2);
                else
                {
                    string _branchID = user.Key.Substring(1, user.Key.Length - 1);

                    b_prmt.Add("BranchID", _branchID);

                    _dt1 = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", b_prmt);
                    _dt2 = _dt1.Copy();
                }

                BindDDLSalesArea(ref _dt1, ref _dt2, ddlBranch, b_prmt);
                BindDDLSalesArea(ref _dt1, ref _dt2, ddlBranch_R, b_prmt, true);
                BindDDLSalesArea(ref _dt1, ref _dt2, ddlBranch_DT, b_prmt, true);
                BindDDLSalesArea(ref _dt1, ref _dt2, ddlBranch_Rank, b_prmt, true);
            }

            //BindBranchData(ddlBranch);

            //BindBranchData(ddlBranch_R, true);

            //BindBranchData(ddlBranch_DT, true);

            //BindBranchData(ddlBranch_Rank, true);

            BindVanData(ddlBranch_DT, ddlVan_DT, true);

            PrepareGridData();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private List<string> AddExcelToDB(FileUpload fileUP, string sheetName, string tableName)
    {
        bool validateEx = true;
        string errMsg = "";
        List<string> result = new List<string>();
        try
        {
            if (fileUP.HasFile)
            {
                DateTime cDate = DateTime.Now;
                string _transDate = "";
                var _dDate = txtTransferDate.Text.Split('/').ToList();
                _transDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
                string _branchID = ddlBranch.SelectedValue;
                string _vanID = "";
                //string _vanID = ddlVanID.SelectedValue;

                string path = string.Concat(Server.MapPath("~/App_Data/" + fileUP.FileName));
                fileUP.SaveAs(path);

                if (tableName == "tbl_branch_van_mt_temp")
                {
                    var sheetList = new List<string> {"VAN1", "VAN2", "VAN3", "VAN4", "VAN5", "VAN6", "VAN7", "VAN8", "VAN9", "VAN10", "VAN11", "VAN12", "VAN13",
                    "VAN21", "VAN22", "VAN23", "VAN24",
                    "VAN90", "VAN91", "VAN92", "VAN93", "VAN94", "VAN95", "VAN96", "VAN97", "VAN98", "VAN99" };

                    DataTable readDT = new DataTable(tableName);

                    readDT.Columns.Add("TransferDate", typeof(DateTime));
                    readDT.Columns.Add("DocDate", typeof(DateTime));
                    readDT.Columns.Add("BranchID", typeof(string));
                    readDT.Columns.Add("VanID", typeof(string));
                    readDT.Columns.Add("Licences", typeof(string));
                    readDT.Columns.Add("Supplier", typeof(string));
                    readDT.Columns.Add("CheckDetailsType", typeof(int));
                    readDT.Columns.Add("Remarks", typeof(string));
                    readDT.Columns.Add("ExcVat", typeof(decimal));
                    readDT.Columns.Add("VatAmt", typeof(decimal));
                    readDT.Columns.Add("TotalDue", typeof(decimal));
                    readDT.Columns.Add("StartMile", typeof(int));
                    readDT.Columns.Add("EndMile", typeof(int));
                    readDT.Columns.Add("MileUsed", typeof(int));
                    readDT.Columns.Add("UpdateBy", typeof(string));
                    readDT.Columns.Add("UpdateDate", typeof(DateTime));


                    foreach (var _sheetName in sheetList)
                    {
                        var dataTable = new DataTable();
                        try
                        {
                            dataTable = Helper.ReadExcelToDataTable2(conString, fileUP.FileName, _sheetName);
                        }
                        catch (Exception ex)
                        {
                            lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
                            lblUploadResult.ForeColor = System.Drawing.Color.Red;
                            dataTable = null;
                        }

                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            _vanID = GetVanID(_sheetName); //GetVanID(dataTable.Rows[1][2].ToString());

                            bool isValid = true;
                            List<bool> allCheckValid = new List<bool>();
                            string exMsg = "";

                            int index = 0;
                            foreach (DataRow row in dataTable.Rows)
                            {
                                DateTime _date = new DateTime();

                                //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------

                                if (DateTime.TryParse(row[0].ToString(), out _date))
                                {
                                    //if (_date < cDate)
                                    DateTime validateDate = Convert.ToDateTime(cDate.ToShortDateString() + " 08:00:00");
                                    var chkD1 = Convert.ToDateTime(cDate.ToShortDateString());
                                    var chkD2 = Convert.ToDateTime(_date.ToShortDateString());

                                    if (chkD1 > chkD2 ||
                                       (chkD1 == chkD2 && cDate >= validateDate))
                                    {
                                        var row1 = dataTable.Rows[1];

                                        string _licences = row1[3].ToString();
                                        string _supplier = row[4].ToString();

                                        int _checkDetailsType = 0;
                                        string col5Data = "";
                                        col5Data = row[5].ToString();
                                        if (!string.IsNullOrEmpty(col5Data))
                                        {
                                            col5Data = row[5].ToString();
                                            _checkDetailsType = GetCDTNameValue(col5Data);
                                        }

                                        if (_checkDetailsType == 0 && string.IsNullOrEmpty(col5Data))
                                        {
                                            DataRow _row = dataTable.Rows[index - 1];
                                            if (_row != null)
                                            {
                                                col5Data = _row[5].ToString();
                                                _checkDetailsType = GetCDTNameValue(col5Data);
                                            }
                                        }

                                        string _remarks = "";
                                        if (!string.IsNullOrEmpty(row[6].ToString()))
                                            _remarks = row[6].ToString();

                                        decimal _excVat = Convert.ToDecimal(string.Format("{0:0.00}", Helper.ConvertColToDec(row, 7)));
                                        decimal _vatAmt = Convert.ToDecimal(string.Format("{0:0.00}", Helper.ConvertColToDec(row, 8)));
                                        decimal _totalDue = Helper.ConvertColToDec(row, 9);

                                        int _startMile = Helper.ConvertColToInt(row, 10);
                                        int _endMile = Helper.ConvertColToInt(row, 11);
                                        int _mileUsed = Helper.ConvertColToInt(row, 12);

                                        string userName = UserList.First().Key;

                                        if (_startMile > _endMile)
                                        {
                                            exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : เลขไมค์เริ่มต้นมากกว่าเลขไมค์สิ้นสุด!" + "\n";
                                            allCheckValid.Add(false);
                                        }
                                        if ((_excVat + _vatAmt) != _totalDue)
                                        {
                                            exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : จำนวนเงินรวม ไม่ถูกต้อง!" + "\n";
                                            allCheckValid.Add(false);
                                        }
                                        if (Convert.ToDateTime(_transDate).Month != _date.Month)
                                        {
                                            exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : วันที่เติมน้ำมัน ไม่ถูกต้อง!" + "\n";
                                            allCheckValid.Add(false);
                                        }

                                        if (!string.IsNullOrEmpty(_supplier))
                                        {
                                            readDT.Rows.Add(_transDate, _date, _branchID, _vanID, _licences, _supplier, _checkDetailsType, _remarks,
                                                _excVat, _vatAmt, _totalDue, _startMile, _endMile, _mileUsed, userName, cDate);

                                            result.Add(_vanID);
                                        }
                                    }
                                }

                                index++;
                                //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------
                            }

                            //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------

                            if (allCheckValid.All(x => x))
                                isValid = true;
                            else
                            {
                                exMsg += " กรุณาตรวจสอบ excel อีกครั้ง!!!" + "\n";
                                isValid = false;
                            }

                            if (!isValid)
                            {
                                errMsg = exMsg;

                                throw new Exception();
                            }
                            //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------
                        }

                    }

                    if (readDT != null && readDT.Rows.Count > 0)
                    {
                        SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                        bulkInsert.DestinationTableName = tableName;
                        bulkInsert.WriteToServer(readDT);
                    }
                    else
                    {
                        lblUploadResult.Text = "ไม่พบข้อมูลการซ่อมใน excel!!!";
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
            result = new List<string>();
        }

        return result;
    }

    private int GetCDTNameValue(string cdtName)
    {
        int ret = 0;
        var van_dt_mast = Helper.ExecuteProcedureToTable(conString, "proc_branch_van_mt_get_dt_master", null);
        if (van_dt_mast != null && van_dt_mast.Rows.Count > 0)
        {
            var van_dt_mastList = van_dt_mast.AsEnumerable();

            var temp = van_dt_mastList.FirstOrDefault(x => x.Field<string>("CheckDetailsName").ToString() == cdtName);
            if (temp != null)
                ret = temp.Field<int>("CheckDetailsType");
        }

        return ret;
    }

    private void BindDDLSalesArea(ref DataTable _dt, ref DataTable _dt2, DropDownList ddlB, Dictionary<string, object> b_prmt, bool allFlag = false)
    {
        if (_dt != null && _dt.Rows.Count > 0)
        {
            if (allFlag)
                ddlB.BindDropdownList(_dt2, "BranchName", "BranchID");
            else
                ddlB.BindDropdownList(_dt, "BranchName", "BranchID");
        }
    }

    private void BindBranchData(DropDownList ddlB, bool allFlag = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> _prmt = new Dictionary<string, object>();
        _prmt.Add("BranchID", "-1");
        _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_stock_report_get_all_branch", _prmt);

        if (allFlag)
        {
            DataTable _dt2 = _dt.Copy();

            DataRow _sRow1 = _dt2.NewRow();
            _sRow1["BranchID"] = "-1";
            _sRow1["BranchName"] = "---สาขาทั้งหมด---";
            _dt2.Rows.InsertAt(_sRow1, 0);

            if (_dt2 != null && _dt2.Rows.Count > 0)
            {
                ddlB.BindDropdownList(_dt2, "BranchName", "BranchID");
            }
        }
        else
        {
            if (_dt != null && _dt.Rows.Count > 0)
            {
                ddlB.BindDropdownList(_dt, "BranchName", "BranchID");
            }
        }
    }

    private void BindVanData(DropDownList ddlB, DropDownList ddlVan, bool allFlag = false)
    {
        try
        {
            DataTable _dt = new DataTable();
            Dictionary<string, object> _prmt = new Dictionary<string, object>();
            _prmt.Add("BranchID", ddlB.SelectedValue);
            _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_get_van", _prmt);

            if (allFlag)
            {
                DataTable _dt2 = _dt.Copy();

                DataRow _sRow1 = _dt2.NewRow();
                _sRow1["van_id"] = "-1";
                _sRow1["VanName"] = "---แวนทั้งหมด---";
                _dt2.Rows.InsertAt(_sRow1, 0);

                ddlVan.BindDropdownList(_dt2, "VanName", "van_id");
            }
            else
            {
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    ddlVan.BindDropdownList(_dt, "VanName", "van_id");
                }
            }

            //else
            //{
            //    DataTable _dt2 = new DataTable();
            //    _dt2.Columns.Add("van_id");
            //    _dt2.Columns.Add("VanName");
            //    DataRow _sRow1 = _dt2.NewRow();
            //    _sRow1["van_id"] = "-1";
            //    _sRow1["VanName"] = "---แวนทั้งหมด---";
            //    _dt2.Rows.InsertAt(_sRow1, 0);

            //    if (_dt2 != null && _dt2.Rows.Count > 0)
            //    {
            //        ddlVan.BindDropdownList(_dt2, "VanName", "van_id");
            //    }
            //}
        }
        catch (Exception ex)
        {

            throw ex;
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
                    results.Add(UploadExcel(FileUpload1, "", "tbl_branch_van_mt_temp"));
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

    private string GetVanID(string sheetName)
    {
        string ret = "";
        if (sheetName.Contains("VAN"))
        {
            var tmp = sheetName.Substring(3, sheetName.Length - 3);

            var tmpInt = Convert.ToInt32(tmp);
            if (tmpInt < 10)
            {
                ret = "V0" + tmpInt;
            }
            else
            {
                ret = "V" + tmpInt;
            }
        }

        return ret;
    }

    private string GridViewSortDirection
    {
        get { return Session["SortDirection"] as string ?? "ASC"; }
        set { Session["SortDirection"] = value; }
    }

    private string GridViewSortExpression
    {
        get { return Session["SortExpression"] as string ?? string.Empty; }
        set { Session["SortExpression"] = value; }
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

    private void PrepareGridData()
    {
        //Dictionary<string, object> _prmt = new Dictionary<string, object>();
        //_prmt.Add("QAType", ddlQAType_T.SelectedItem.Text);

        //DataTable _dt = Helper.ExecuteProcedureToTable(conString_qa, "proc_qa_get_questionnaire", _prmt);
        //if (_dt != null && _dt.Rows.Count > 0)
        //{
        //    Session["grdQA"] = _dt;
        //    BindGridView(grdQA, _dt);
        //}
    }

    private void Search()
    {
        try
        {
            string _branchID = ddlBranch_R.SelectedValue.ToString();
            //string _vanID = ddlVanID_R.SelectedValue.ToString();
            int _sendStatus = Convert.ToInt32(ddlDocStatus.SelectedValue.ToString());

            string _transferDate = "";
            if (!string.IsNullOrEmpty(txtTransferDate_R.Text))
            {
                var _td = txtTransferDate_R.Text.Split('/').ToList();
                _transferDate = string.Join("/", _td[1], _td[0], _td[2]);
            }

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("BranchID", _branchID);
            t_prmt.Add("VanID", ""); // _vanID);
            t_prmt.Add("TransferDate", _transferDate);
            t_prmt.Add("SendStatus", _sendStatus);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_van_mt_search_status", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdStatus, t_DT);
                Session["grdStatus"] = t_DT;

                btnExportExcel.Visible = true;
            }
            else
            {
                BindGridView(grdStatus, null);
                Session["grdStatus"] = null;

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

    private void SearchDT(GridView grd, int? pk = null)
    {
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            bool isViewDT = false;

            if (pk != null)
            {
                isViewDT = true;
                t_prmt.Add("PK", pk);
                t_prmt.Add("BranchID", "-1");
                t_prmt.Add("VanID", "-1");
                t_prmt.Add("TransferDateF", "");
                t_prmt.Add("TransferDateT", "");
            }
            else
            {
                string _branchID = ddlBranch_DT.SelectedValue.ToString();
                string _vanID = ddlVan_DT.SelectedValue.ToString();

                string _transferDateF = "";
                if (!string.IsNullOrEmpty(txtTransferDateF_DT.Text))
                {
                    var _td = txtTransferDateF_DT.Text.Split('/').ToList();
                    _transferDateF = string.Join("/", _td[1], _td[0], _td[2]);
                }

                string _transferDateT = "";
                if (!string.IsNullOrEmpty(txtTransferDateT_DT.Text))
                {
                    var _td = txtTransferDateT_DT.Text.Split('/').ToList();
                    _transferDateT = string.Join("/", _td[1], _td[0], _td[2]);
                }

                t_prmt.Add("PK", -1);
                t_prmt.Add("BranchID", _branchID);
                t_prmt.Add("VanID", _vanID);
                //t_prmt.Add("TransferDateF", _transferDateF);
                //t_prmt.Add("TransferDateT", _transferDateT);
                if (chkHistory.Checked)
                {
                    t_prmt.Add("TransferDateF", _transferDateF);
                    t_prmt.Add("TransferDateT", _transferDateT);
                }
                else
                {
                    t_prmt.Add("TransferDateF", "");
                    t_prmt.Add("TransferDateT", "");
                }
            }

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_van_mt_search", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                if (isViewDT)
                    ddlBranch_DT.SelectedValue = t_DT.Rows[1][4].ToString();

                BindGridView(grd, t_DT);
                Session[grd.ID] = t_DT;

                btnExcel_DT.Visible = true;
            }
            else
            {
                BindGridView(grd, null);
                Session[grd.ID] = null;

                btnExcel_DT.Visible = false;
            }

            hidHistory.Value = chkHistory.Checked ? "1" : "0";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void SearchDT_Popup(GridView grd, int pk)
    {
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();

            t_prmt.Add("PK", pk);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_van_mt_search_dt", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grd, t_DT);
                Session[grd.ID] = t_DT;

                btnExportSubDT.Visible = true;
            }
            else
            {
                BindGridView(grd, null);
                Session[grd.ID] = null;

                btnExportSubDT.Visible = false;
            }

            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void SearchRank()
    {
        try
        {
            Dictionary<string, object> t_prmt = new Dictionary<string, object>();

            string _branchID = ddlBranch_Rank.SelectedValue.ToString();
            int _rankingType = Convert.ToInt32(ddlRankingType.SelectedValue);

            string _fromDate = "";
            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                var _td = txtDateFrom.Text.Split('/').ToList();
                _fromDate = string.Join("/", _td[1], _td[0], _td[2]);
            }

            string _toDate = "";
            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                var _td = txtDateTo.Text.Split('/').ToList();
                _toDate = string.Join("/", _td[1], _td[0], _td[2]);
            }

            t_prmt.Add("BranchID", _branchID);
            t_prmt.Add("RankingType", _rankingType);
            t_prmt.Add("DateFrom", _fromDate);
            t_prmt.Add("DateTo", _toDate);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_van_mt_search_rank", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdRanking, t_DT);
                Session["grdRanking"] = t_DT;

                btnExport_Rank.Visible = true;
            }
            else
            {
                BindGridView(grdRanking, null);
                Session["grdRanking"] = null;

                btnExport_Rank.Visible = false;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
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
                    Helper.ExecuteProcedureToTable(conString, sqlCmd, CommandType.Text);

                    List<string> _result = new List<string>();
                    _result = AddExcelToDB(fileUP, sheetName, tableName);

                    foreach (var _vanID in _result.Distinct().ToList())
                    {
                        string _transDate = "";
                        var _dDate = txtTransferDate.Text.Split('/').ToList();
                        _transDate = string.Join("/", _dDate[1], _dDate[0], _dDate[2]);
                        string user = UserList.First().Key;

                        Dictionary<string, object> _prmt = new Dictionary<string, object>();
                        _prmt.Add("BranchID", ddlBranch.SelectedValue);
                        _prmt.Add("VanID", _vanID);
                        _prmt.Add("TransferDate", _transDate);
                        _prmt.Add("UpdateBy", user);
                        Helper.ExecuteProcedureOBJ(conString, "proc_branch_van_mt_update_data", _prmt);
                    }

                    if (_result.Count > 0)
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


    #endregion

    #region Event Methods

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindVanData(ddlBranch, ddlVanID);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void ddlBranch_R_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindVanData(ddlBranch_R, ddlVanID_R, true);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void ddlBranch_DT_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVanData(ddlBranch_DT, ddlVan_DT, true);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdStatus"] != null)
            {
                DataTable _dt = (DataTable)Session["grdStatus"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานสถานะการส่งค่าซ่อมบำรุงรถ.xlsx");
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

    protected void btnSearchDT_Click(object sender, EventArgs e)
    {
        SearchDT(grdDetails);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void btnExcel_DT_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdDetails"] != null)
            {
                DataTable _dt = (DataTable)Session["grdDetails"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานรายละเอียดค่าซ่อมบำรุงรถ.xlsx");
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

    protected void btnSearch_Rank_Click(object sender, EventArgs e)
    {
        SearchRank();
    }

    protected void btnExport_Rank_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdRanking"] != null)
            {
                DataTable _dt = (DataTable)Session["grdRanking"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานอันกับค่าซ่อมบำรุงรถ.xlsx");
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

    protected void grdStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "report")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdStatus.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    SearchDT(grdDetails, Convert.ToInt32(_pk));
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grdStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["grdStatus"] != null)
        {
            grdStatus.PageIndex = e.NewPageIndex;
            BindGridView(grdStatus, (DataTable)Session["grdStatus"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
    }

    protected void grdStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<int> centerAlign = new List<int>() { 5, 6, 7, 8, 9 };
        List<int> rightAlign = new List<int>();

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            if (j == 1)
            {
                e.Row.Cells[j].Visible = false;
            }
            if (centerAlign.Contains(j))
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            if (rightAlign.Contains(j))
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
            }

            if (e.Row.RowIndex > -1)
            {
                var cell = e.Row.Cells[j];
                if (j == 7)
                {
                    if (cell.Text == "ส่งเอกสารจากศูนย์แล้ว")
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

    protected void grdStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView grd = grdStatus;
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grd.PageIndex;
        grd.DataSource = SortDataTable((DataTable)Session["grdStatus"], false);
        grd.DataBind();
        for (int i = 1; i < grd.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grd.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grd.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grd.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void grdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            List<int> centerAlign = new List<int>() { 2, 3, 4, 6, 7, 17, 18 };
            List<int> rightAlign = new List<int>() { 11, 12, 13, 14, 15, 16 };
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                //if (j == 0)
                //{
                //    e.Row.Cells[j].Visible = false;
                //}
                if (centerAlign.Contains(j))
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
                if (rightAlign.Contains(j))
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
                }

                if (e.Row.RowIndex == 0 && grdDetails.PageIndex == 0)
                {
                    var cell = e.Row.Cells[j];
                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                    cell.Font.Bold = true;
                    cell.Enabled = false;

                }
            }
        }
    }

    protected void grdDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView grd = grdDetails;
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grd.PageIndex;
        grd.DataSource = SortDataTable((DataTable)Session["grdDetails"], false);
        grd.DataBind();
        for (int i = 1; i < grd.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grd.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grd.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grd.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void grdDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["grdDetails"] != null)
        {
            grdDetails.PageIndex = e.NewPageIndex;
            BindGridView(grdDetails, (DataTable)Session["grdDetails"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
    }

    protected void btnSearchStatus_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnExportSubDT_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdSubDT"] != null)
            {
                DataTable _dt = (DataTable)Session["grdSubDT"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานรายละเอียดอันดับค่าบำรุงรักษารถ.xlsx");
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

    protected void grdRanking_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdRanking.PageIndex;
        var dv = SortDataTable((DataTable)Session["grdRanking"], false); ;
        grdRanking.DataSource = dv.ToTable();
        grdRanking.DataBind();
        for (int i = 1; i < grdRanking.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdRanking.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdRanking.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdRanking.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
    }

    protected void grdRanking_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["grdRanking"] != null)
        {
            grdRanking.PageIndex = e.NewPageIndex;
            var dv = SortDataTable((DataTable)Session["grdRanking"], false);
            BindGridView(grdRanking, dv.ToTable());
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
        }
    }

    protected void grdRanking_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            List<int> centerAlign = new List<int>() { 3, 6, 7 };
            List<int> rightAlign = new List<int>() { 8 };

            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                if (j == 1)
                {
                    e.Row.Cells[j].Visible = false;
                }
                if (centerAlign.Contains(j))
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
                if (rightAlign.Contains(j))
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
                }
            }

            if (e.Row.RowIndex >= 0)
            {
                var cell14 = e.Row.Cells[8];
                cell14.BackColor = System.Drawing.ColorTranslator.FromHtml("#FCECC4");
                cell14.Font.Bold = true;
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected void grdRanking_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "details")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdRanking.DataKeys[index].Value.ToString();

                SearchDT_Popup(grdSubDT, Convert.ToInt32(_pk));

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowDT(4)", true);
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }

    }

    protected void grdSubDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<int> centerAlign = new List<int>() { 2, 3, 4, 6, 7, 17, 18 };
        List<int> rightAlign = new List<int>() { 11, 12, 13, 14, 15, 16 };
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {

            if (j == 0)
            {
                e.Row.Cells[j].Visible = false;
            }
            if (centerAlign.Contains(j))
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            if (rightAlign.Contains(j))
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
            }
        }
    }

    protected void grdSubDT_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdSubDT.PageIndex;
        grdSubDT.DataSource = SortDataTable((DataTable)Session["grdSubDT"], false);
        grdSubDT.DataBind();
        for (int i = 1; i < grdSubDT.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdSubDT.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdSubDT.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdSubDT.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowDT(4)", true);
    }

    protected void ddlRankingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchRank();
    }


    #endregion

}