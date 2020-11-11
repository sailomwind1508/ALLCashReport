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

public partial class TransportationCost : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

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

            ClearForm();

            //Search();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
        }

        var requestTarget = this.Request["__EVENTTARGET"];
        string pk = "";
        if (this.Request["__EVENTARGUMENT"] != null)
            pk = this.Request["__EVENTARGUMENT"].ToString();

        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "save")
            {
                Save();
            }
            else if (requestTarget == "remove")
            {
                if (!string.IsNullOrEmpty(pk))
                {
                    Remove(pk);
                }
            }
            else if (requestTarget == "import_excel")
            {
                string excelPath = Request["__EVENTARGUMENT"]; // parameter
            }
        }
    }

    #region Private Methods

    private void InitPage()
    {
        Dictionary<string, string> reportList = new Dictionary<string, string>();
        reportList.Add("1", "รายงานรายศูนย์");
        reportList.Add("2", "รายงานรายสินค้า");
        reportList.Add("3", "รายงานตรวจสอบค่าขนส่ง");
        ddlReportType.BindDropdownList(reportList);

        DataTable saleareaDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_salearea", null);
        Dictionary<string, string> saleareaList = new Dictionary<string, string>();
        Dictionary<string, string> saleareaList_E = new Dictionary<string, string>();
        saleareaList.Add("-1", "---All---");

        foreach (DataRow item in saleareaDT.Rows)
        {
            saleareaList.Add(item["SalesAreaCode"].ToString(), item["SalesAreaCode"] + ":" + item["SalesArea"]);
            saleareaList_E.Add(item["SalesAreaCode"].ToString(), item["SalesAreaCode"] + ":" + item["SalesArea"]);
        }

        if (saleareaDT != null && saleareaDT.Rows.Count > 0)
        {
            ddlSalesAreaR.BindDropdownList(saleareaList);
            ddlSalesArea.BindDropdownList(saleareaList);
            ddlSalesAreaCode_E.BindDropdownList(saleareaList_E);
            ddlSalesAreaDT.BindDropdownList(saleareaList);
        }

        DataTable cartypeDT_E = new DataTable();
        DataTable cartypeDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_cartype", null);
        cartypeDT_E = cartypeDT.Copy();

        DataRow cRow = cartypeDT.NewRow();
        cRow["CarType"] = "-1";
        cRow["CarTypeName"] = "---All---";
        cartypeDT.Rows.InsertAt(cRow, 0);

        if (cartypeDT != null && cartypeDT.Rows.Count > 0)
        {
            ddlCarType.BindDropdownList(cartypeDT, "CarTypeName", "CarType");
            ddlCarType_E.BindDropdownList(cartypeDT_E, "CarTypeName", "CarType");
            ddlCarTypeDT.BindDropdownList(cartypeDT, "CarTypeName", "CarType");
        }

        DataTable supplierDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_supplier", null);
        DataTable supplierDT_E = new DataTable();
        supplierDT_E = supplierDT.Copy();

        DataRow sRow = supplierDT.NewRow();
        sRow["Supplier"] = "-1";
        sRow["SupplierName"] = "---All---";
        supplierDT.Rows.InsertAt(sRow, 0);

        if (supplierDT != null && supplierDT.Rows.Count > 0)
        {
            ddlSuppiler.BindDropdownList(supplierDT, "SupplierName", "Supplier");
            ddlSupplier_E.BindDropdownList(supplierDT_E, "SupplierName", "Supplier");
        }

        var _date = DateTime.Now;

        Dictionary<string, string> monthList = new Dictionary<string, string>();
        monthList.Add("-1", "--All--");
        for (int i = 1; i <= 12; i++)
        {
            var _month = new DateTime(_date.Year, i, 1);
            monthList.Add(i.ToString(), _month.ToString("MMMM"));
        }
        ddlMonth.BindDropdownList(monthList);
        ddlMonth.SelectedValue = _date.Month.ToString();

        Dictionary<string, string> yearList = new Dictionary<string, string>();
        for (int i = 2010; i <= _date.Year; i++)
        {
            yearList.Add(i.ToString(), i.ToString());
        }
        ddlYear.BindDropdownList(yearList);
        ddlYear.SelectedValue = _date.Year.ToString();

        Dictionary<string, object> d_prmt = new Dictionary<string, object>();
        d_prmt.Add("_SalesAreaCode", "-1");
        d_prmt.Add("_DocType", "-1");
        DataTable docnoDT = new DataTable();
        docnoDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_po_tr_master_get_docno", d_prmt);
        if (docnoDT != null && docnoDT.Rows.Count > 0)
        {
            Session["docnoDT"] = docnoDT;

            ddlDocNo.BindDropdownList(docnoDT, "DocNumDT", "DocNum");
            BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);

            ddlDocNoDT.BindDropdownList(docnoDT, "DocNumDT", "DocNum");
            BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
        }
    }

    private void Search()
    {
        try
        {
            string _SalesAreaCode = ddlSalesArea.SelectedValue.ToString();
            int _Month = Convert.ToInt32(ddlMonth.SelectedValue);
            int _Year = Convert.ToInt32(ddlYear.SelectedValue);
            string _CarType = ddlCarType.SelectedValue.ToString();
            string _Supplier = ddlSuppiler.SelectedValue.ToString();

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("_SalesAreaCode", _SalesAreaCode);
            t_prmt.Add("_Month", _Month);
            t_prmt.Add("_Year", _Year);
            t_prmt.Add("_CarType", _CarType);
            t_prmt.Add("_Supplier", _Supplier);

            DataTable t_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_transportation_get", t_prmt);
            if (t_DT != null && t_DT.Rows.Count > 0)
            {
                BindGridView(grdTransportation, t_DT);
                ViewState["grdTransportation"] = t_DT;
            }
            else
            {
                BindGridView(grdTransportation, null);
                ViewState["grdTransportation"] = null;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void SearchDT()
    {
        try
        {
            string _SalesAreaCode = ddlSalesAreaDT.SelectedValue.ToString();
            string _CarType = ddlCarTypeDT.SelectedValue.ToString();
            string _Licence = txtLicenceDT.Text;
            string _DocNo = ddlDocNoDT.SelectedValue.ToString();
            string _DocType = rdoPO.Checked ? "PO" : "TR";

            Dictionary<string, object> tdt_prmt = new Dictionary<string, object>();
            tdt_prmt.Add("_SalesAreaCode", _SalesAreaCode);
            tdt_prmt.Add("_CarType", _CarType);

            if (!string.IsNullOrEmpty(txtTranDateDT_F.Text) && !string.IsNullOrEmpty(txtTranDateDT_T.Text))
            {
                string _TranDate_F = "";

                var _tDate_F = txtTranDateDT_F.Text.Split('/').ToList();
                _TranDate_F = string.Join("/", _tDate_F[1], _tDate_F[0], _tDate_F[2]);

                string _TranDate_T = "";

                var _tDate_T = txtTranDateDT_T.Text.Split('/').ToList();
                _TranDate_T = string.Join("/", _tDate_T[1], _tDate_T[0], _tDate_T[2]);

                tdt_prmt.Add("_TranDate_F", _TranDate_F);
                tdt_prmt.Add("_TranDate_T", _TranDate_T);
            }
            else
                tdt_prmt.Add("_TranDate", "");

            tdt_prmt.Add("_Licence", _Licence);
            tdt_prmt.Add("_DocNo", _DocNo);

            string docType = "";

            if (rdoPOTR.Checked)
                docType = "-1";
            else
                docType = _DocType;

            tdt_prmt.Add("_DocType", docType);

            DataTable tdt_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_transportation_dt_get", tdt_prmt);
            if (tdt_DT != null && tdt_DT.Rows.Count > 0)
            {
                BindGridView(grdTransportationDT, tdt_DT);
                ViewState["grdTransportation_dt"] = tdt_DT;
                btnExportDTExcel.Visible = true;
            }
            else
            {
                BindGridView(grdTransportationDT, null);
                ViewState["grdTransportation_dt"] = null;
                btnExportDTExcel.Visible = false;
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void SearchReport1()
    {
        try
        {
            Dictionary<string, object> tdt_prmt = new Dictionary<string, object>();
            string _TranDateF = "";
            string _TranDateT = "";

            var _tDateF = txtTranDate1.Text.Split('/').ToList();
            _TranDateF = string.Join("/", _tDateF[1], _tDateF[0], _tDateF[2]);

            var _tDateT = txtTranDate2.Text.Split('/').ToList();
            _TranDateT = string.Join("/", _tDateT[1], _tDateT[0], _tDateT[2]);

            tdt_prmt.Add("DateFrom", _TranDateF);
            tdt_prmt.Add("DateTo", _TranDateT);
            tdt_prmt.Add("SalesAreaCode", ddlSalesAreaR.SelectedValue.ToString());

            DataTable r1_DT = new DataTable("ReportData");
            if (ddlReportType.SelectedValue.ToString() == "1")
            {
                grdTransReport1.Visible = true;
                grdTransReport2.Visible = false;
                grdTransReport3.Visible = false;

                r1_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_salesarea_report", tdt_prmt);
                if (r1_DT != null && r1_DT.Rows.Count > 0)
                {
                    BindGridView(grdTransReport1, r1_DT);
                    ViewState["grdTransReport1"] = r1_DT;
                    btnExportExcel.Visible = true;
                }
                else
                {
                    BindGridView(grdTransReport1, null);
                    ViewState["grdTransReport1"] = null;
                    btnExportExcel.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString() == "2")
            {
                grdTransReport1.Visible = false;
                grdTransReport2.Visible = true;
                grdTransReport3.Visible = false;

                r1_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_salesarea_by_sku_report", tdt_prmt);
                if (r1_DT != null && r1_DT.Rows.Count > 0)
                {
                    BindGridView(grdTransReport2, r1_DT);
                    ViewState["grdTransReport2"] = r1_DT;
                    btnExportExcel.Visible = true;
                }
                else
                {
                    BindGridView(grdTransReport2, null);
                    ViewState["grdTransReport2"] = null;
                    btnExportExcel.Visible = false;
                }
            }
            else if (ddlReportType.SelectedValue.ToString() == "3")
            {
                grdTransReport1.Visible = false;
                grdTransReport2.Visible = false;
                grdTransReport3.Visible = true;

                r1_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_salesarea_diff_report", tdt_prmt);
                if (r1_DT != null && r1_DT.Rows.Count > 0)
                {
                    BindGridView(grdTransReport3, r1_DT);
                    ViewState["grdTransReport3"] = r1_DT;
                    btnExportExcel.Visible = true;
                }
                else
                {
                    BindGridView(grdTransReport3, null);
                    ViewState["grdTransReport3"] = null;
                    btnExportExcel.Visible = false;
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void Save()
    {
        try
        {
            string _SalesAreaCode = ddlSalesAreaCode_E.SelectedValue.ToString();
            string _SalesArea = ddlSalesAreaCode_E.SelectedItem.Text.Split(':')[1];

            string date = "";
            var _date = txtTranDate.Text.Split('/').ToList();
            date = string.Join("/", _date[1], _date[0], _date[2]);
            DateTime _TranDate = Convert.ToDateTime(date);

            string _Licence = txtLicence.Text;

            string _DocNo = "";
            if (chkDocNoCon.Checked)
                _DocNo = txtDocNo_E.Text;
            else
                _DocNo = ddlDocNo.SelectedValue.ToString();

            string _CarType = ddlCarType_E.SelectedValue.ToString();
            string _Supplier = ddlSupplier_E.SelectedItem.Text; //txtSupplier.Text;
            decimal _Quantity = Convert.ToInt32(txtQuantity.Text);
            decimal _TotalQuantity = Convert.ToInt32(txtTotalQuantity.Text);
            decimal _Amount = Convert.ToDecimal(txtAmount.Text);
            string _Remark = txtRemark.Text;

            int _PK = -1;
            if (!string.IsNullOrEmpty(hidPK.Value))
                _PK = Convert.ToInt32(hidPK.Value);

            Dictionary<string, object> t_prmt = new Dictionary<string, object>();
            t_prmt.Add("SalesAreaCode", _SalesAreaCode);
            t_prmt.Add("SalesArea", _SalesArea);
            t_prmt.Add("TranDate", _TranDate);
            t_prmt.Add("Licence", _Licence);
            t_prmt.Add("DocNo", _DocNo);
            t_prmt.Add("CarType", _CarType);
            t_prmt.Add("Supplier", _Supplier);
            t_prmt.Add("Quantity", _Quantity);
            t_prmt.Add("TotalQuantity", _TotalQuantity);
            t_prmt.Add("Amount", _Amount);
            t_prmt.Add("Remark", _Remark);
            t_prmt.Add("PK", _PK);

            Helper.ExecuteProcedureOBJ(conString, "proc_transportation_transportation_dt_edit_r2", t_prmt);

            SearchDT();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveResult(2);", true);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void Remove(string pk)
    {
        Dictionary<string, object> gpk_prmt = new Dictionary<string, object>();
        int _PK = Convert.ToInt32(pk);
        gpk_prmt.Add("_PK", _PK);

        Helper.ExecuteProcedureOBJ(conString, "proc_transportation_transportation_dt_delete", gpk_prmt);

        SearchDT();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveResult(2)", true);
    }

    private void ClearForm()
    {
        ddlSalesAreaCode_E.SelectedIndex = 0;
        txtTranDate.Text = string.Empty;
        txtLicence.Text = string.Empty;

        txtDocNo_E.Text = string.Empty;
        chkDocNoCon.Checked = false;
        ddlDocNo.SelectedIndex = 0;

        ddlCarType_E.SelectedIndex = 0;
        ddlSupplier_E.SelectedIndex = 0;
        //txtSupplier.Text = string.Empty;
        txtQuantity.Text = string.Empty;
        txtTotalQuantity.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtRemark.Text = string.Empty;
        txtFilterDate.Text = string.Empty;
        hidPK.Value = "";

        Dictionary<string, object> d_prmt = new Dictionary<string, object>();
        d_prmt.Add("_SalesAreaCode", "-1");
        d_prmt.Add("_DocType", "-1");
        DataTable docnoDT = new DataTable();
        docnoDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_po_tr_master_get_docno", d_prmt);
        if (docnoDT != null && docnoDT.Rows.Count > 0)
        {
            ddlDocNo.BindDropdownList(docnoDT, "DocNumDT", "DocNum");
            BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
        }
    }

    private string AddExcelToDB(FileUpload fileUp, string sheetName, string tableName)
    {
        string result = "";
        try
        {
            //OleDbCommand cmd = new OleDbCommand("select * from [" + sheetName + "$]", con);
            //con.Open();
            //// Create DbDataReader to Data Worksheet
            //DbDataReader dr = cmd.ExecuteReader();
            //var dataTable = new DataTable();
            //dataTable.Load(dr);

            string path = string.Concat(Server.MapPath("~/App_Data/" + fileUp.FileName));
            fileUp.SaveAs(path);

            var dataTable = new DataTable();
            dataTable = Helper.ReadExcelToDataTable(conString, fileUp.FileName, sheetName);

            if (tableName == "tbl_Transportation_Details_Temp")
            {
                DataTable _filterDTTemp = new DataTable();
                _filterDTTemp = dataTable.Select("SalesAreaCode <> ''").CopyToDataTable();
                DataTable _filterDT = _filterDTTemp.Clone();
                _filterDT.Clear();
                //_filterDT.Columns.Add("SalesAreaCode", typeof(string));

                DataTable saleareaDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_get_salearea", null);
                if (saleareaDT != null && saleareaDT.Rows.Count > 0)
                {
                    foreach (DataRow row in _filterDTTemp.Rows)
                    {
                        var salesAreaObj = saleareaDT.AsEnumerable().FirstOrDefault(x => x.Field<string>("SalesArea") == row["SalesArea"].ToString());
                        if (salesAreaObj != null)
                        {
                            string saleAreaCode = salesAreaObj.Field<string>("SalesAreaCode");
                            object supplier = "";
                            supplier = (row["Supplier"] == null || string.IsNullOrEmpty(row["Supplier"].ToString())) ? "" : row["Supplier"];
                            _filterDT.Rows.Add(saleAreaCode, row["SalesArea"], row["TranDate"], row["Licence"],
                                row["Supplier"], row["DocNo"], row["CarType"], row["Quantity"], row["Amount"], row["TotalQuantity"]);
                        }
                    }

                    // Bulk Copy to SQL Server 
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                    bulkInsert.DestinationTableName = tableName;
                    bulkInsert.WriteToServer(_filterDT);
                }
            }
            else
            {
                SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                bulkInsert.DestinationTableName = tableName;
                bulkInsert.WriteToServer(dataTable);
            }
            //BindGridview();
            //con.Close();
        }
        catch (Exception ex)
        {
            //con.Close();

            result = ex.Message;
        }

        return result;
    }

    private void BindGridView(GridView grd, DataTable data)
    {
        try
        {
            //if (grd.ID == "grdTransportation" && data.Rows.Count > 0)
            //{
            //    DataRow totalRow = data.NewRow();
            //    totalRow["ROW_NO"] = 0;
            //    totalRow["PK"] = 0;
            //    totalRow["SalesAreaCode"] = "ยอดรวม";

            //    foreach (DataColumn column in data.Columns)
            //    {
            //        if (column.ColumnName != "ROW_NO" && column.ColumnName != "PK" && column.ColumnName != "SalesAreaCode")
            //        {
            //            decimal _col_total = 0;
            //            foreach (var x in data.AsEnumerable())
            //            {
            //                object _temp_data = null;

            //                if (column.ColumnName == "ShippingCost")
            //                {
            //                    _temp_data = x.Field<decimal>(column.ColumnName);
            //                    var _data = !string.IsNullOrEmpty(_temp_data.ToString()) ? Convert.ToDecimal(_temp_data.ToString()) : 0;
            //                    _col_total += _data;
            //                }
            //            }

            //            if (column.ColumnName == "ShippingCost")
            //                totalRow[column.ColumnName] = _col_total.ToString("#,##0.00");
            //            //else if (column.ColumnName == "เดือน" || column.ColumnName == "ปี")
            //            //    totalRow[column.ColumnName] = 0;
            //            else
            //                totalRow[column.ColumnName] = "";
            //        }
            //    }

            //    data.Rows.InsertAt(totalRow, 0);
            //}

            grd.DataSource = data;
            grd.DataBind();


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void BindDocNo(DropDownList ddl, DropDownList ddlSAC, RadioButton rdo, bool hasDefault = false)
    {
        if ((DataTable)Session["docnoDT"] != null)
        {
            ddl.DataSource = null;

            string saleACode = ddlSAC.SelectedValue.ToString();
            string docType = (rdo.Checked ? "PO" : "TR");

            DataTable _docnoDT = (DataTable)Session["docnoDT"]; //Helper.ExecuteProcedureToTable(conString, "proc_transportation_po_tr_master_get_docno", d_prmt);

            DataTable ddlDT = new DataTable();
            ddlDT = _docnoDT.Clone();
            ddlDT.Clear();

            List<DataRow> res = new List<DataRow>();
            if (ddlSAC.ID == "ddlSalesAreaCode_E")
                res = FilterDataTable_E(_docnoDT, saleACode, docType, txtFilterDate.Text);
            else
                res = FilterDataTable_N(_docnoDT, saleACode, docType); //, txtTranDateDT.Text);

            foreach (DataRow dr in res)
            {
                ddlDT.Rows.Add(dr["DocNum"], dr["DocNumDT"], dr["SalesAreaCode"]);
            }

            if (hasDefault)
            {
                DataRow sRow = ddlDT.NewRow();
                sRow["DocNum"] = "-1";
                sRow["DocNumDT"] = "---All---";
                sRow["SalesAreaCode"] = "-1";
                sRow["DocDate"] = DateTime.Now;
                ddlDT.Rows.InsertAt(sRow, 0);
            }

            ddl.Items.Clear();
            if (ddlDT != null && ddlDT.Rows.Count > 0)
                ddl.BindDropdownList(ddlDT, "DocNumDT", "DocNum");
            else
            {
                DataRow sRow = ddlDT.NewRow();
                sRow["DocNum"] = "-1";
                sRow["DocNumDT"] = "---All---";
                sRow["DocDate"] = DateTime.Now;
                ddlDT.Rows.InsertAt(sRow, 0);
                ddl.BindDropdownList(ddlDT, "DocNumDT", "DocNum");
            }
        }
    }

    private List<DataRow> FilterDataTable_E(DataTable _docnoDT, string saleACode, string docType, string tranDate)
    {
        List<DataRow> res = new List<DataRow>();
        if (!string.IsNullOrEmpty(tranDate))
        {
            res = (from row in _docnoDT.AsEnumerable()
                   where row.Field<string>("SalesAreaCode") == saleACode && row.Field<DateTime>("DocDate").ToString("dd/MM/yyyy") == tranDate && (row.Field<string>("DocNum").Contains(docType))
                   select row).ToList();
        }
        else
        {
            res = (from row in _docnoDT.AsEnumerable()
                   where row.Field<string>("SalesAreaCode") == saleACode && (row.Field<string>("DocNum").Contains(docType))
                   select row).ToList();
        }

        return res;
    }

    private List<DataRow> FilterDataTable_N(DataTable _docnoDT, string saleACode, string docType) //, string tranDate)
    {
        List<DataRow> res = new List<DataRow>();
        if (saleACode != "-1")
        {
            if (rdoPOTR.Checked)
            {
                //if (!string.IsNullOrEmpty(tranDate))
                //    res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("SalesAreaCode") == saleACode && row.Field<DateTime>("DocDate").ToString("dd/MM/yyyy") == tranDate select row).ToList();
                //else
                    res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("SalesAreaCode") == saleACode select row).ToList();
            }
            else
            {
                //if (!string.IsNullOrEmpty(tranDate))
                //    res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("DocNum").Contains(docType) && row.Field<string>("SalesAreaCode") == saleACode && row.Field<DateTime>("DocDate").ToString("dd/MM/yyyy") == tranDate select row).ToList();
                //else
                    res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("DocNum").Contains(docType) && row.Field<string>("SalesAreaCode") == saleACode select row).ToList();
            }
        }
        else
        {
            if (rdoPOTR.Checked)
            {
                //if (!string.IsNullOrEmpty(tranDate))
                //    res = (from row in _docnoDT.AsEnumerable() where row.Field<DateTime>("DocDate").ToString("dd/MM/yyyy") == tranDate select row).ToList();
                //else
                    res = (from row in _docnoDT.AsEnumerable() select row).ToList();
            }
            else
            {
                //    if (!string.IsNullOrEmpty(tranDate))
                //        res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("DocNum").Contains(docType) && row.Field<DateTime>("DocDate").ToString("dd/MM/yyyy") == tranDate select row).ToList();
                //    else
                res = (from row in _docnoDT.AsEnumerable() where row.Field<string>("DocNum").Contains(docType) select row).ToList();
            }
        }
        return res;
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

    private void RowCreated(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell tc in e.Row.Cells)
            {
                if (tc.HasControls())
                {
                    // search for the header link
                    LinkButton lnk = (LinkButton)tc.Controls[0];

                    string sortDir = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";
                    string sortBy = ViewState["SortExpression"] != null ? ViewState["SortExpression"].ToString() : "---";

                    if (lnk != null && sortBy == lnk.CommandArgument)
                    {
                        string sortArrow = sortDir == "ASC" ? " &#9650;" : " &#9660;";
                        lnk.Text += sortArrow;
                    }
                }
            }
        }
    }

    #endregion

    #region Event Methods

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable _dt = new DataTable();
            string reportName = "";

            if (ddlReportType.SelectedValue.ToString() == "1" && ViewState["grdTransReport1"] != null)
            {
                _dt = (DataTable)ViewState["grdTransReport1"];
                reportName = "รายงานค่าขนส่งรายศูนย์";
            }
            else if (ddlReportType.SelectedValue.ToString() == "2" && ViewState["grdTransReport2"] != null)
            {
                _dt = (DataTable)ViewState["grdTransReport2"];
                reportName = "รายงานค่าขนส่งรายสินค้า";
            }
            else if (ddlReportType.SelectedValue.ToString() == "3" && ViewState["grdTransReport3"] != null)
            {
                _dt = (DataTable)ViewState["grdTransReport3"];
                reportName = "รายงานตรวจสอบค่าขนส่ง";
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_dt, "Report");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");
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
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnExportDTExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["grdTransportation_dt"] != null)
            {
                DataTable _dt = new DataTable();
                _dt = (DataTable)ViewState["grdTransportation_dt"];
                if (_dt.Rows.Count > 0)
                {
                   

                    string reportName = "รายละเอียดการขนส่ง";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(_dt, "Report");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");
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

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
        
    }

    protected void btnSR1_Click(object sender, EventArgs e)
    {
        SearchReport1();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Save();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        List<string> results = new List<string>();
        bool checkImportFalg = false;

        if (FileUpload1.PostedFile != null)
        {
            try
            {
                if (string.IsNullOrEmpty(FileUpload1.FileName))
                {
                    lblUploadResult.Text = "กรุณาเลือก excel ก่อนกดปุ่ม!";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
                    return;
                }

                //FileUpload1.SaveAs(path);

                //string fullPath = "";
                //OleDbConnectionStringBuilder Builder = new OleDbConnectionStringBuilder();
                //if (Path.GetExtension(FileUpload1.FileName).ToUpper() == ".XLS")
                //{
                //    fullPath = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;HDR=YES;", path);
                //}
                //else
                //{
                //    fullPath = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0;", path);
                //}

                //// Connection String to Excel Workbook
                //string excelCS = string.Format(fullPath);
                //using (OleDbConnection con = new OleDbConnection(excelCS))
                {
                    if (chkTransportation.Checked)
                    {
                        Dictionary<string, string> procedureParam1 = new Dictionary<string, string>();
                        procedureParam1.Add("tableName", "tbl_Transportation_Temp");
                        Helper.ExecuteProcedure(conString, "proc_transportation_clear_master_data", procedureParam1);
                        results.Add(AddExcelToDB(FileUpload1, "Transportation", "tbl_Transportation_Temp"));
                        Helper.ExecuteProcedure(conString, "proc_transportation_update_transportation", null);

                        checkImportFalg = true;
                    }
                    if (chkTransportationDT.Checked)
                    {
                        Dictionary<string, string> procedureParam2 = new Dictionary<string, string>();
                        procedureParam2.Add("tableName", "tbl_Transportation_Details_Temp");
                        Helper.ExecuteProcedure(conString, "proc_transportation_clear_master_data", procedureParam2);
                        results.Add(AddExcelToDB(FileUpload1, "Transportation_Details", "tbl_Transportation_Details_Temp"));
                        Helper.ExecuteProcedure(conString, "proc_transportation_update_transportation_dt", null);

                        checkImportFalg = true;
                    }
                }

                if (!checkImportFalg)
                {
                    lblUploadResult.Text = "กรุณาเลือกข้อมูลที่ต้องนำเข้าระบบ!";
                    lblUploadResult.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
                }
                else
                {
                    if (results.All(x => x == ""))
                    {
                        lblUploadResult.Text = "อัพโหลด excel เรียบร้อยแล้ว!";
                        lblUploadResult.ForeColor = System.Drawing.Color.Green;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
                    }
                    else
                    {
                        List<string> errMsgList = results.Where(x => !string.IsNullOrEmpty(x)).ToList();
                        for (int i = 0; i < errMsgList.Count; i++)
                        {
                            errMsgList[i] = (i + 1).ToString() + ") " + errMsgList[i];
                        }
                        string errMsg = "";
                        errMsg = string.Join("<br />", errMsgList);
                        lblUploadResult.Text = errMsg;
                        lblUploadResult.ForeColor = System.Drawing.Color.Red;

                        InitPage();

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
                    }
                }
            }
            catch (Exception ex)
            {
                lblUploadResult.Text = "2) เกิดข้อผิดพลาดในการอัพโหลด excel!" + ex.Message;
                lblUploadResult.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
            }
        }
        else
        {
            lblUploadResult.Text = "กรุณาเลือก excel!";
            lblUploadResult.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnSearchDT_Click(object sender, EventArgs e)
    {
        SearchDT();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void ddlSalesAreaDT_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void rdoPO_CheckedChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void rdoTR_CheckedChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void rdoPOTR_CheckedChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void ddlSalesAreaCode_E_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSalesAreaCode_E.SelectedValue != "9999")
        {
            chkDocNoCon.Checked = false;
            BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
        }
        else
        {
            chkDocNoCon.Checked = true;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void txtTranDate_TextChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    //protected void txtTranDateDT_TextChanged(object sender, EventArgs e)
    //{
    //    BindDocNo(ddlDocNoDT, ddlSalesAreaDT, rdoPO, true);
    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    //}

    protected void rdoPO_E_CheckedChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    protected void rdoTR_E_CheckedChanged(object sender, EventArgs e)
    {
        BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
    }

    #region Gridview Event

    protected void grdTransReport1_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdTransReport1.PageIndex;
        grdTransReport1.DataSource = SortDataTable((DataTable)ViewState["grdTransReport1"], false);
        grdTransReport1.DataBind();
        for (int i = 1; i < grdTransReport1.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdTransReport1.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                //TableCell tableCell = grdTransReport1.HeaderRow.Cells[i];
                //Image img = new Image();
                //img.ImageUrl = "~/img/ico_" + (grdTransReport1.SortDirection == SortDirection.Ascending ? "asc" : "desc") + ".gif";
                //// adding a space and the image to the header link

                ////img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                //tableCell.Controls.Add(img);
            }
        }

        grdTransReport1.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void grdTransReport1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdTransReport1"] != null)
        {
            grdTransReport1.PageIndex = e.NewPageIndex;
            BindGridView(grdTransReport1, (DataTable)ViewState["grdTransReport1"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
        }
    }

    protected void grdTransportationDT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "imgEdit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdTransportationDT.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    Dictionary<string, object> gpk_prmt = new Dictionary<string, object>();
                    int _PK = Convert.ToInt32(_pk);
                    gpk_prmt.Add("_PK", _PK);

                    DataTable tdt_DT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_transportation_dt_get_by_pk", gpk_prmt);
                    if (tdt_DT != null && tdt_DT.Rows.Count > 0)
                    {
                        ddlSalesAreaCode_E.SelectedValue = tdt_DT.Rows[0]["SalesAreaCode"].ToString();
                        txtTranDate.Text = Convert.ToDateTime(tdt_DT.Rows[0]["TranDate"]).ToString("dd/MM/yyyy");
                        txtLicence.Text = tdt_DT.Rows[0]["Licence"].ToString();

                        if (ddlSalesAreaCode_E.SelectedValue == "9999")
                        {
                            txtDocNo_E.Text = tdt_DT.Rows[0]["DocNo"].ToString();
                            chkDocNoCon.Checked = true;
                        }
                        else
                        {
                            try
                            {
                                ddlDocNo.Items.Clear();

                                Dictionary<string, object> d_prmt = new Dictionary<string, object>();
                                d_prmt.Add("_SalesAreaCode", "-1");
                                d_prmt.Add("_DocType", "-1");
                                DataTable docnoDT = new DataTable();
                                docnoDT = Helper.ExecuteProcedureToTable(conString, "proc_transportation_po_tr_master_get_docno", d_prmt);
                                if (docnoDT != null && docnoDT.Rows.Count > 0)
                                {
                                    ddlDocNo.BindDropdownList(docnoDT, "DocNumDT", "DocNum");
                                    BindDocNo(ddlDocNo, ddlSalesAreaCode_E, rdoPO_E);
                                }

                                ddlDocNo.SelectedValue = tdt_DT.Rows[0]["DocNo"].ToString();
                                chkDocNoCon.Checked = false;
                            }
                            catch
                            {
                                txtDocNo_E.Text = tdt_DT.Rows[0]["DocNo"].ToString();
                                chkDocNoCon.Checked = true;
                            }
                        }

                        ddlCarType_E.SelectedValue = tdt_DT.Rows[0]["CarType"].ToString();
                        //txtSupplier.Text = tdt_DT.Rows[0]["Supplier"].ToString();
                        ddlSupplier_E.SelectedItem.Text = tdt_DT.Rows[0]["Supplier"].ToString();
                        txtQuantity.Text = tdt_DT.Rows[0]["Quantity"].ToString();
                        txtTotalQuantity.Text = tdt_DT.Rows[0]["TotalQuantity"].ToString();
                        txtAmount.Text = Convert.ToDecimal(tdt_DT.Rows[0]["Amount"]).ToString("#,##0.00");
                        txtRemark.Text = tdt_DT.Rows[0]["Remark"].ToString();
                        hidPK.Value = _pk;

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(3)", true);
                    }
                }
            }
            else if (e.CommandName == "imgRemove")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string _pk = "";
                _pk = grdTransportationDT.DataKeys[index].Value.ToString();

                if (!string.IsNullOrEmpty(_pk))
                {
                    Remove(_pk);
                }
            }
        }
        catch (Exception ex)
        {

            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdTransportation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdTransportation"] != null)
        {
            grdTransportation.PageIndex = e.NewPageIndex;
            BindGridView(grdTransportation, (DataTable)ViewState["grdTransportation"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(4)", true);
        }
    }

    protected void grdTransportation_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grdTransportation_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdTransportation.PageIndex;
        grdTransportation.DataSource = SortDataTable((DataTable)ViewState["grdTransportation"], false);
        grdTransportation.DataBind();
        for (int i = 1; i < grdTransportation.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdTransportation.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdTransportation.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdTransportation.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
    }

    protected void grdTransportation_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grdTransportationDT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdTransportation_dt"] != null)
        {
            grdTransportationDT.PageIndex = e.NewPageIndex;
            BindGridView(grdTransportationDT, (DataTable)ViewState["grdTransportation_dt"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
    }

    protected void grdTransportationDT_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j == 8)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDD7EE");
            }
            if (j == 9)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD966");
            }
        }
    }

    protected void grdTransportationDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //        e.Row.BackColor = System.Drawing.Color.FromName("#C2D69B");
            //}
            //else
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //        e.Row.BackColor = System.Drawing.Color.FromName("white");
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");

            //    e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
            //}


            //List<int> centerCols = new List<int>() { 1 };

            //for (int j = 0; j < e.Row.Cells.Count; j++)
            //{
            //    var cell = e.Row.Cells[j];
            //    if (centerCols.Any(x => x == j))
            //    {
            //        if (e.Row.RowIndex != -1 && j == 2)
            //            cell.Text = Convert.ToDateTime(cell.Text).ToString("dd/MM/yyyy");
            //    }
            //}
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grdTransportationDT_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void grdTransReport2_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdTransReport2.PageIndex;
        grdTransReport2.DataSource = SortDataTable((DataTable)ViewState["grdTransReport2"], false);
        grdTransReport2.DataBind();
        for (int i = 1; i < grdTransReport2.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdTransReport2.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                //TableCell tableCell = grdTransReport2.HeaderRow.Cells[i];
                //Image img = new Image();
                //img.ImageUrl = "~/img/ico_" + (grdTransReport1.SortDirection == SortDirection.Ascending ? "asc" : "desc") + ".gif";
                ////img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                //tableCell.Controls.Add(img);
            }
        }

        grdTransReport2.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void grdTransReport2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdTransReport2"] != null)
        {
            grdTransReport2.PageIndex = e.NewPageIndex;
            BindGridView(grdTransReport2, (DataTable)ViewState["grdTransReport2"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
        }
    }

    protected void grdTransReport3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["grdTransReport3"] != null)
        {
            grdTransReport3.PageIndex = e.NewPageIndex;
            BindGridView(grdTransReport3, (DataTable)ViewState["grdTransReport3"]);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
        }
    }

    protected void grdTransReport3_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdTransReport3.PageIndex;
        grdTransReport3.DataSource = SortDataTable((DataTable)ViewState["grdTransReport3"], false);
        grdTransReport3.DataBind();
        for (int i = 1; i < grdTransReport3.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdTransReport3.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                //TableCell tableCell = grdTransReport3.HeaderRow.Cells[i];
                //Image img = new Image();
                //img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/img/panel_tool_collapse.gif" : "~/img/panel_tool_expand.gif";
                //tableCell.Controls.Add(img);
            }
        }

        grdTransReport3.PageIndex = pageIndex;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void grdTransReport2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        RowCreated(e);
    }

    protected void grdTransReport1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        RowCreated(e);
    }

    protected void grdTransReport3_RowCreated(object sender, GridViewRowEventArgs e)
    {
        RowCreated(e);
    }

    protected void grdTransReport2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j == 10)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDD7EE");
            }
            if (j == 11)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD966");
            }
        }

        if (e.Row.RowIndex == 0)
        {
            e.Row.Font.Bold = true;
            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC000");
        }
    }

    protected void grdTransReport3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];

            if (e.Row.RowIndex >= 0)
            {
                if (j == 9)
                {
                    if (cell.Text == "ตรง")
                    {
                        cell.BackColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        cell.BackColor = System.Drawing.Color.Orange;
                    }
                }
            }

            if (j == 7)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C6E0B4");

            }
            if (j == 8)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDD7EE");
            }
        }
    }

    protected void grdTransReport1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j == 10)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C6E0B4");
            }
            if (j == 11)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDD7EE");
            }
            if (j == 12)
            {
                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFD966");
            }
        }
    }

    #endregion

    #endregion




}