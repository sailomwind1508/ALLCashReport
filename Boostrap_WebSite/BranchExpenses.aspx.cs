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

public partial class BranchExpenses : System.Web.UI.Page
{
    ReportDocument rdoc1 = new ReportDocument();

    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    Dictionary<string, string> permissionList = new Dictionary<string, string>();
    List<string> sheetList = new List<string> {"VAN1", "VAN2", "VAN3", "VAN4", "VAN5", "VAN6", "VAN7", "VAN8", "VAN9", "VAN10", "VAN11", "VAN12", "VAN13",
                    "VAN21", "VAN22", "VAN23", "VAN24",
                    "VAN90", "VAN91", "VAN92", "VAN93", "VAN94", "VAN95", "VAN96", "VAN97", "VAN98", "VAN99" };

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
            rankingList.Add("1", "Top 10 ค่าน้ำมันต่อวัน");
            rankingList.Add("2", "Top 10 ค่าเฉลี่ยน้ำมันต่อวัน");
            rankingList.Add("3", "Top 10 ระยะทางต่อวัน");
            rankingList.Add("4", "Top 10 ค่าเฉลี่ยระยะทางต่อวัน");
            rankingList.Add("5", "เรียงลำดับเอง");
            //rankingList.Add("4", "Top 10 ค่าเฉลี่ยนการใช้น้ำมัน");
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

                if (tableName == "tbl_branch_oil_bill_temp")
                {

                    
                    // Helper.GetExcelSheetNames(fileUP.FileName, path);

                    DataTable readDT = new DataTable(tableName);
                    //readDT.Columns.Add("PK", typeof(int));
                    readDT.Columns.Add("TransferDate", typeof(DateTime));
                    readDT.Columns.Add("DocDate", typeof(DateTime));
                    readDT.Columns.Add("BranchID", typeof(string));
                    readDT.Columns.Add("VanID", typeof(string));
                    readDT.Columns.Add("Licences", typeof(string));
                    readDT.Columns.Add("FleetGardNo", typeof(string));
                    readDT.Columns.Add("DocNo", typeof(string));
                    readDT.Columns.Add("VATNumber", typeof(string));
                    readDT.Columns.Add("CarrierName", typeof(string));
                    readDT.Columns.Add("Establishment", typeof(string));
                    readDT.Columns.Add("ExcVat", typeof(decimal));
                    readDT.Columns.Add("VatAmt", typeof(decimal));
                    readDT.Columns.Add("TotalDue", typeof(decimal));
                    readDT.Columns.Add("PricePerLiter", typeof(decimal));
                    readDT.Columns.Add("LiterAmt", typeof(decimal));
                    readDT.Columns.Add("StartMile", typeof(int));
                    readDT.Columns.Add("EndMile", typeof(int));
                    readDT.Columns.Add("MileUsed", typeof(int));
                    readDT.Columns.Add("AddFuelMileNo", typeof(int));
                    readDT.Columns.Add("Remarks", typeof(string));
                    readDT.Columns.Add("UpdateBy", typeof(string));
                    readDT.Columns.Add("UpdateDate", typeof(DateTime));

                    foreach (var _sheetName in sheetList)
                    {
                        var dataTable = new DataTable();
                        try
                        {
                            dataTable = Helper.ReadExcelToDataTable2(conString, fileUP.FileName, _sheetName);
                            //Helper.ReadSample(fileUP, path, _sheetName);
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

                            foreach (DataRow row in dataTable.Rows)
                            {
                                DateTime _date = new DateTime();

                                //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------

                                if (DateTime.TryParse(row[0].ToString(), out _date))
                                {
                                    DateTime validateDate = Convert.ToDateTime(cDate.ToShortDateString() + " 19:00:00");
                                    var chkD1 = Convert.ToDateTime(cDate.ToShortDateString());
                                    var chkD2 = Convert.ToDateTime(_date.ToShortDateString());

                                    if (chkD1 > chkD2 || 
                                       (chkD1 == chkD2 && cDate >= validateDate))
                                    {
                                        string _licences = row[3].ToString();
                                        string _fleetGardNo = row[4].ToString();
                                        string _docNo = row[5].ToString();
                                        string _vatNumber = row[6].ToString();
                                        string _carrierName = row[7].ToString();
                                        string _establishment = row[8].ToString();

                                        decimal _excVat = ConvertColToDec(row, 9);
                                        decimal _vatAmt = ConvertColToDec(row, 10);
                                        decimal _totalDue = ConvertColToDec(row, 11);
                                        decimal _pricePerLiter = ConvertColToDec(row, 12);
                                        decimal _literAmt = ConvertColToDec(row, 13);
                                        int _startMile = ConvertColToInt(row, 14);
                                        int _endMile = ConvertColToInt(row, 15);
                                        int _mileUsed = ConvertColToInt(row, 16);
                                        int _addFuelMileNo = ConvertColToInt(row, 17);

                                        string _remarks = "";
                                        if (!string.IsNullOrEmpty(row[18].ToString()))
                                            _remarks = row[18].ToString();

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
                                        if (_mileUsed >= 1000)
                                        {
                                            exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : จำนวนไมล์ใช้งาน ไม่ถูกต้อง!" + "\n";
                                            allCheckValid.Add(false);
                                        }
                                        if (Convert.ToDateTime(_transDate).Month != _date.Month || Convert.ToDateTime(chkD2).Year >= 2100)
                                        {
                                            exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : วันที่เติมน้ำมัน ไม่ถูกต้อง!" + "\n";
                                            allCheckValid.Add(false);
                                        }

                                        readDT.Rows.Add(_transDate, _date, _branchID, _vanID, _licences, _fleetGardNo, _docNo, _vatNumber, _carrierName,
                                            _establishment, _excVat, _vatAmt, _totalDue, _pricePerLiter, _literAmt, _startMile, _endMile, _mileUsed, _addFuelMileNo, _remarks, userName, cDate);

                                        result.Add(_vanID);
                                    }

                                    if (Convert.ToDateTime(chkD2).Year >= 2100)
                                    {
                                        exMsg += "ข้อมูลแวน " + _vanID + " ของวันที่ " + _date.ToString("dd/MM/yyyy") + " : วันที่เติมน้ำมัน ไม่ถูกต้อง!" + "\n";
                                        allCheckValid.Add(false);
                                    }
                                }

                                //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------
                            }

                            //validate Excel -------------------------------------------------------------------------------------------------------------------------------------------
                            bool _checkJCols = false;
                            bool _checkRCols = false;
                            if (readDT != null && readDT.Rows.Count > 0)
                            {
                                foreach (DataRow item in readDT.Rows)
                                {
                                    decimal _check_excVat = 0;
                                    if (decimal.TryParse(item[10].ToString(), out _check_excVat))
                                    {
                                        if (_check_excVat > 0)
                                        {
                                            _checkJCols = true;
                                            break;
                                        }
                                    }
                                }

                                foreach (DataRow item in readDT.Rows)
                                {
                                    int _check_addFuelMileNo = 0;
                                    if (int.TryParse(item[18].ToString(), out _check_addFuelMileNo))
                                    {
                                        if (_check_addFuelMileNo > 0)
                                        {
                                            _checkRCols = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (allCheckValid.All(x => x) && _checkJCols && _checkRCols)
                                isValid = true;
                            else
                            {
                                if (readDT.AsEnumerable().All(x => x.Field<decimal>("TotalDue") == 0))
                                {
                                    //Do nothing
                                }
                                else
                                {
                                    exMsg += " กรุณาตรวจสอบ excel อีกครั้ง!!!" + "\n";
                                    isValid = false;
                                }
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

    private decimal ConvertColToDec(DataRow row, int colIndex)
    {
        decimal ret = 0;
        try
        {
            string colStr = "";
            colStr = row[colIndex].ToString();

            if (!string.IsNullOrEmpty(colStr) && colStr != "-")
            {
                decimal _tmp = 0;
                if (decimal.TryParse(colStr, out _tmp))
                    ret = _tmp;
            }
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "ข้อมูลใน excel ไม่ถูกต้อง!" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
        }

        return ret;
    }

    private int ConvertColToInt(DataRow row, int colIndex)
    {
        int ret = 0;
        try
        {
            string colStr = "";
            colStr = row[colIndex].ToString();

            if (!string.IsNullOrEmpty(colStr) && colStr != "-")
            {
                int _tmp = 0;
                if (int.TryParse(colStr, out _tmp))
                    ret = _tmp;
            }
        }
        catch (Exception ex)
        {
            lblUploadResult.Text = "ข้อมูลใน excel ไม่ถูกต้อง!" + ex.Message;
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
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
            //Dictionary<string, object> _prmt = new Dictionary<string, object>();
            //_prmt.Add("BranchID", ddlB.SelectedValue);

            //_dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_get_van", _prmt);

            _dt = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_get_van_2", null);
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
                    results.Add(UploadExcel(FileUpload1, "", "tbl_branch_oil_bill_temp"));
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

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_search_status", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdBExns, t_DT);
                Session["grdBExns"] = t_DT;

                btnExportExcel.Visible = true;
            }
            else
            {
                BindGridView(grdBExns, null);
                Session["grdBExns"] = null;

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

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_search", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 1)
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

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_search_dt", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grd, t_DT);
                Session[grd.ID] = t_DT;

                btnExportOilDT.Visible = true;
            }
            else
            {
                BindGridView(grd, null);
                Session[grd.ID] = null;

                btnExportOilDT.Visible = false;
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

            //DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_search_rank", t_prmt);
            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_branch_expenses_search_rank_r2", t_prmt);
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
                        Helper.ExecuteProcedureOBJ(conString, "proc_branch_expenses_update_data", _prmt);
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

    protected void btnSearchBE_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdBExns"] != null)
            {
                DataTable _dt = (DataTable)Session["grdBExns"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานค่าน้ำมัน.xlsx");
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
        SearchDT(grdBEDT);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void btnExcel_DT_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["grdBEDT"] != null)
            {
                DataTable _dt = (DataTable)Session["grdBEDT"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "Report");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Branch Expenses Report.xlsx");
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

    protected void grdBExns_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "report")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdBExns.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    SearchDT(grdBEDT, Convert.ToInt32(_pk));
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grdBExns_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdBExns.PageIndex;
        grdBExns.DataSource = SortDataTable((DataTable)Session["grdQA"], false);
        grdBExns.DataBind();
        for (int i = 1; i < grdBExns.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdBExns.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdBExns.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdBExns.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void grdBExns_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["grdBExns"] != null)
        {
            grdBExns.PageIndex = e.NewPageIndex;
            BindGridView(grdBExns, (DataTable)Session["grdBExns"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
    }

    protected void grdBExns_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex > -1)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                if (j == 5)
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

                //if (j == 9)
                //{
                //    cell.Font.Bold = true;
                //}
            }
        }
    }

    protected void grdBEDT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["grdBEDT"] != null)
        {
            grdBEDT.PageIndex = e.NewPageIndex;
            BindGridView(grdBEDT, (DataTable)Session["grdBEDT"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
        }
    }

    protected void grdBEDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        if (e.Row.RowIndex == 0 && grdBEDT.PageIndex == 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cell = e.Row.Cells[j];
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                cell.Font.Bold = true;
                cell.Enabled = false;
            }
        }
        else if (e.Row.RowIndex > 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cellMileAmt = e.Row.Cells[19];
                if (!string.IsNullOrEmpty(cellMileAmt.Text))
                {
                    int _cellMileAmt = 0;
                    if (int.TryParse(cellMileAmt.Text, out _cellMileAmt))
                    {
                        if (_cellMileAmt >= 100)
                        {
                            cellMileAmt.Font.Bold = true;
                            cellMileAmt.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF1700");
                        }
                    }
                }

                var cellFuel = e.Row.Cells[20];
                if (!string.IsNullOrEmpty(cellFuel.Text))
                {
                    decimal _cellFuel = 0;
                    if (decimal.TryParse(cellFuel.Text, out _cellFuel))
                    {
                        if (_cellFuel > 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDFFC2");
                            e.Row.Font.Bold = true;
                        }
                    }
                }

            }
        }
    }

    protected void grdBEDT_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdBEDT.PageIndex;
        grdBEDT.DataSource = SortDataTable((DataTable)Session["grdBEDT"], false);
        grdBEDT.DataBind();
        for (int i = 1; i < grdBEDT.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdBEDT.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdBEDT.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdBEDT.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void grdBEDT_RowCommand(object sender, GridViewCommandEventArgs e)
    {

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
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานการใช้ค่าน้ำมัน.xlsx");
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
            DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                if (j == 1)
                {
                    e.Row.Cells[j].Visible = false;
                }
                if (j == 3)
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
                if (j == 9)
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Right;
                }
            }

            if (e.Row.RowIndex >= 0)
            {
                var cell14 = e.Row.Cells[9];
                cell14.BackColor = System.Drawing.ColorTranslator.FromHtml("#FCECC4");
                cell14.Font.Bold = true;

                //var cell15 = e.Row.Cells[15];
                //cell15.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4B084");
                //cell15.Font.Bold = true; 

                //var cell20 = e.Row.Cells[20];
                //cell20.BackColor = System.Drawing.ColorTranslator.FromHtml("#FAFE9B");
                //cell20.Font.Bold = true;
            }

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    var _text = ddlRankingType.SelectedItem.Text;
            //    string htext = "";

            //    if (_text.Length > 13)
            //        htext = _text.Substring(7, _text.Length - 7);
            //    else
            //        htext = _text;

            //    e.Row.Cells[9].Text = htext;
            //}
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
        if (e.Row.RowIndex >= 0)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                var cellMileAmt = e.Row.Cells[19];
                if (!string.IsNullOrEmpty(cellMileAmt.Text))
                {
                    int _cellMileAmt = 0;
                    if (int.TryParse(cellMileAmt.Text, out _cellMileAmt))
                    {
                        if (_cellMileAmt >= 100)
                        {
                            cellMileAmt.Font.Bold = true;
                            cellMileAmt.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF1700");
                        }
                    }
                }

                var cellFuel = e.Row.Cells[20];
                if (!string.IsNullOrEmpty(cellFuel.Text))
                {
                    decimal _cellFuel = 0;
                    if (decimal.TryParse(cellFuel.Text, out _cellFuel))
                    {
                        if (_cellFuel > 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FDFFC2");
                            e.Row.Font.Bold = true;
                        }
                    }
                }

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

    protected void btnExportOilDT_Click(object sender, EventArgs e)
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
                    Response.AddHeader("content-disposition", "attachment;filename=รายงานอันดับค่าน้ำมัน.xlsx");
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

    protected void ddlRankingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchRank();
    }


    #endregion

}