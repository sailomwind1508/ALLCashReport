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
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data.Common;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading;

public partial class SaleTargetReport : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString_saletarget"].ConnectionString;
    Dictionary<string, string> yearList = new Dictionary<string, string>();
    Dictionary<string, string> monthList = new Dictionary<string, string>();
    Dictionary<string, string> productList = new Dictionary<string, string>();
    Dictionary<string, string> customerList = new Dictionary<string, string>();
    Dictionary<string, string> empList = new Dictionary<string, string>();
    Dictionary<string, string> edEmpList = new Dictionary<string, string>();
    Dictionary<string, string> excel_yearList = new Dictionary<string, string>();
    Dictionary<string, string> excel_monthList = new Dictionary<string, string>();

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

            SearchReport();
        }

        var requestTarget = this.Request["__EVENTTARGET"];
        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "save_target")
            {
                SaveTarget();
            }
            else if (requestTarget == "import_target")
            {
                string excelPath = Request["__EVENTARGUMENT"]; // parameter
            }
        }
    }

    private void InitPage()
    {
        try
        {
            //tab1
            yearList.StoredProceduresToList(conString, "proc_sale_target_cust_report_getYearMonth_2", "TARGET_YEAR", "TARGET_YEAR");

            monthList.StoredProceduresToList(conString, "proc_sale_target_cust_report_getYearMonth", "TARGET_MONTH", "TARGET_MONTH");
            //monthList.StoredProceduresToListWith2Key(conString, "proc_sale_target_cust_report_getYearMonth", "TARGET_MONTH", "TARGET_YEAR", "TARGET_MONTH");

            ddlSTGYear.BindDropdownList(yearList);
            ddlSTGMonth.BindDropdownList(monthList);

            productList.Add("-1", "---All---");
            productList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getProduct", "SKU_ID", "SKU_ID", "SKU_NAME");
            ddlProduct.BindDropdownList(productList);

            empList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getEmployee", "SALE_ID", "SALE_ID", "SALE_NAME");
            ddlEmployee.BindDropdownList(empList);

            customerList.Add("-1", "---All---");
            ddlCustomer.BindDropdownList(customerList);

            //tab2

            edEmpList.Add("-1", "---ทั้งหมด---");
            edEmpList.Add("-2", "---กรุงเทพฯ---");
            edEmpList.Add("-3", "---ต่างจังหวัด---");
            edEmpList.Add("-4", "---ภาคเหนือ---");
            edEmpList.Add("-5", "---ภาคอีสาน---");
            edEmpList.Add("-6", "---ภาคกลาง---");
            edEmpList.Add("-7", "---ภาคใต้---");
            foreach (KeyValuePair<string, string> item in empList)
            {
                edEmpList.Add(item.Key, item.Value);
            }
            ddlED_Emp.BindDropdownList(edEmpList);
            ddlED_Month.BindDropdownList(monthList);
            ddlED_Year.BindDropdownList(yearList);

            for (int i = 1; i <= 12; i++)
            {
                excel_monthList.Add(i.ToString(), i.ToString());
            }

            for (int i = 2019; i <= 2050; i++)
            {
                excel_yearList.Add(i.ToString(), i.ToString());
            }
            ddlExcelMonth.BindDropdownList(excel_monthList);
            ddlExcelYear.BindDropdownList(excel_yearList);

            DateTime nextMonth = DateTime.Now.AddMonths(1);
            string currentM = nextMonth.Month.ToString();
            string currentY = nextMonth.Year.ToString();

            ddlExcelMonth.SelectedValue = currentM;
            ddlExcelYear.SelectedValue = currentY;

            ddlSTGMonth.SelectedValue = currentM;//currentM + "|" + currentY;
            ddlSTGYear.SelectedValue = currentY;

            ddlED_Month.SelectedValue = currentM;//currentM + "|" + currentY;
            ddlED_Year.SelectedValue = currentY;

            Dictionary<string, string> reportTypeList = new Dictionary<string, string>();
            reportTypeList.Add("cust", "---รายงานตามสินค้ากับลูกค้า---");
            reportTypeList.Add("visit", "---รายงานตามสินค้ากับวันเยี่ยม---");
            reportTypeList.Add("cust_visit", "---รายงานตามแบบลูกค้ากับวันเยี่ยม---");
            reportTypeList.Add("visit_cust", "---รายงานตามแบบวันเยี่ยมกับลูกค้า---");
            ddlReportType.BindDropdownList(reportTypeList);

            BindVisitDate();

            BindCustomer();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SearchReport()
    {
        try
        {
            List<DataTable> dtList = new List<DataTable>();
            string sale_id = ddlEmployee.SelectedValue.ToString() == "-1" ? "" : ddlEmployee.SelectedValue.ToString();

            DataTable _dt = new DataTable();
            _dt = GetSaleTargetToDataTable(sale_id);
            if (_dt != null)
            {
                dtList.Add(_dt);
                Session["SaleTargetReport"] = _dt;
            }

            _dt = GetSaleTargetPriceToDataTable(sale_id);
            if (_dt != null)
            {
                dtList.Add(_dt);
                Session["SaleTargetReportPrice"] = _dt;
            }

            if (dtList.Count > 0 && dtList[0] != null)
            {
                linkExportReport.Visible = !string.IsNullOrEmpty(dtList[0].Rows[0].ItemArray[0].ToString());
                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];
            }
            else
            {
                Session["SaleTargetReport"] = null;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(2)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }

        BindGridView();

        if (!chkAllSalseReport.Checked)
        {
            Session["STG_AllReportEmp"] = GetAllEmpReport();
        }
    }

    private bool ValidateSave()
    {
        List<bool> rets = new List<bool>();
        foreach (GridViewRow row in grdEditSTG.Rows)
        {
            string target_qty = "";
            TextBox _target_qty = (row.FindControl("txtTARGET_QTY") as TextBox);

            if (_target_qty != null && _target_qty is TextBox)
            {
                target_qty = _target_qty.Text;
            }

            if (!string.IsNullOrEmpty(target_qty))
            {
                rets.Add(true);
            }
            else
            {
                rets.Add(false);
            }
        }
        return rets.All(x => x);
    }

    private void SaveTarget()
    {
        try
        {
            bool saveFlag = ValidateSave();
            if (!saveFlag)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ValidateSave()", true);
                return;
            }

            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();
                foreach (GridViewRow row in grdEditSTG.Rows)
                {
                    string target_qty = "";
                    TextBox _target_qty = (row.FindControl("txtTARGET_QTY") as TextBox);

                    if (_target_qty != null && _target_qty is TextBox)
                    {
                        target_qty = _target_qty.Text;
                    }

                    if (!string.IsNullOrEmpty(target_qty))
                    {
                        string skuId = (row.FindControl("hfSKU_ID") as HiddenField).Value;
                        string saleId = (row.FindControl("hfSALE_ID") as HiddenField).Value;

                        SqlCommand cmd = null;

                        cmd = new SqlCommand("proc_sale_target_cust_report_edit_target", cn);

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SALE_ID", saleId));
                        cmd.Parameters.Add(new SqlParameter("@SKU_ID", skuId));
                        cmd.Parameters.Add(new SqlParameter("@TARGET_QTY", Convert.ToInt32(target_qty)));

                        cmd.CommandTimeout = 0;

                        cmd.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }

            SearchEdit();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SaveResult(1)", true);
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void BindVisitDate()
    {
        string sale_id = ddlEmployee.SelectedValue.ToString() == "-1" ? "" : ddlEmployee.SelectedValue.ToString();
        string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
        string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];

        Dictionary<string, string> paramList = new Dictionary<string, string>();
        Dictionary<string, string> visitList = new Dictionary<string, string>();
        paramList.Add("SALE_ID", sale_id);
        paramList.Add("TARGET_MONTH", target_month);
        paramList.Add("TARGET_YEAR", target_year);
        visitList.Add("-1", "---All---");
        visitList.ExecuteProcedureToList(conString, "proc_sale_target_cust_report_getVisitDate", "VISIT_DATE", "VISIT_DATE", paramList);
        ddlVisitDate.BindDropdownList(visitList);
    }

    private void BindCustomer()
    {
        Dictionary<string, string> customerList = new Dictionary<string, string>();

        if (ddlEmployee.SelectedValue.ToString() != "-1")
        {
            string sale_id = ddlEmployee.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(sale_id))
            {
                customerList.Add("-1", "---All---");
                customerList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getCustomer", "CUST_ID", "CUST_ID", "CUST_NAME", sale_id);
                ddlCustomer.BindDropdownList(customerList);
            }
        }
        else
        {
            customerList.Add("-1", "---All---");
            ddlCustomer.BindDropdownList(customerList);
        }
    }

    public List<T> DataTableToList<T>(DataTable table) where T : new()
    {
        List<T> list = new List<T>();
        var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
        {
            PropertyInfo = propertyInfo,
            Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
        }).ToList();

        foreach (var row in table.Rows.Cast<DataRow>())
        {
            T obj = new T();
            foreach (var typeProperty in typeProperties)
            {
                object value = row[typeProperty.PropertyInfo.Name];
                object safeValue = value == null || DBNull.Value.Equals(value)
                    ? null
                    : Convert.ChangeType(value, typeProperty.Type);

                typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
            }
            list.Add(obj);
        }
        return list;
    }

    private DataRow SubAddTotalRow(List<DataTable> dtList, string totalName, int index)
    {
        try
        {
            DataRow totalRow = dtList[index].NewRow();

            if (dtList != null && dtList.Count > 0)
            {
                if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                {
                    grdSaleTGReport.DataKeyNames = new string[] { "CUST_ID" };
                }
                else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                {
                    grdSaleTGReport.DataKeyNames = new string[] { "WORK_DATE" };
                }
                else
                {
                    grdSaleTGReport.DataKeyNames = new string[] { "SKU_ID" };
                }

                //for (int index = 0; index < dtList.Count; index++)
                {
                    if (ddlReportType.SelectedValue.ToString() == "cust" ||
                        ddlReportType.SelectedValue.ToString() == "visit")
                    {
                        //totalRow = dtList[index].NewRow();

                        totalRow[0] = totalName;
                        totalRow[1] = "";
                        totalRow[2] = "";
                        totalRow[3] = "";
                        totalRow[4] = DBNull.Value;
                        totalRow[5] = dtList[index].Compute("Sum([TARGET QTY])", "[TARGET QTY] IS NOT NULL");
                        totalRow[6] = dtList[index].Compute("Sum([TARGET PRICE])", "[TARGET PRICE] IS NOT NULL");
                        totalRow[7] = dtList[index].Compute("Sum([BALANCE QTY])", "[BALANCE QTY] IS NOT NULL");
                        totalRow[8] = dtList[index].Compute("Sum([BALANCE PRICE])", "[BALANCE PRICE] IS NOT NULL");
                        totalRow[9] = dtList[index].Compute("Sum([TOTAL QTY])", "[TOTAL QTY] IS NOT NULL");
                        totalRow[10] = dtList[index].Compute("Sum([TOTAL PRICE])", "[TOTAL PRICE] IS NOT NULL");

                        for (int i = 11; i < dtList[index].Columns.Count; i++)
                        {
                            string colName = dtList[index].Columns[i].ColumnName;

                            totalRow[i] = dtList[index].Compute("Sum([" + colName + "])", "[" + colName + "] IS NOT NULL");
                        }

                        //dtList[index].Rows.InsertAt(totalRow, rowIndex);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "cust_visit" ||
                            ddlReportType.SelectedValue.ToString() == "visit_cust")
                    {
                        //totalRow = dtList[index].NewRow();

                        if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                        {
                            totalRow[0] = totalName;
                            totalRow[1] = "";
                            totalRow[2] = "";
                            totalRow[3] = "";
                            totalRow[4] = dtList[index].Compute("Sum([SALE QTY])", "[SALE QTY] IS NOT NULL");
                            totalRow[5] = dtList[index].Compute("Sum([TOTAL PRICE])", "[TOTAL PRICE] IS NOT NULL");

                            for (int i = 6; i < dtList[index].Columns.Count; i++)
                            {
                                string colName = dtList[index].Columns[i].ColumnName;
                                //string ex = "Sum([" + colName + "])";
                                //string filter = "[" + colName + "] IS NOT NULL";
                                //totalRow[i] = dtList[index].Compute(ex, filter);
                                totalRow[i] = dtList[index].Compute("Sum([" + colName + "])", "[" + colName + "] IS NOT NULL");
                            }
                        }
                        else
                        {
                            totalRow[0] = totalName;
                            totalRow[1] = "";
                            totalRow[2] = "";
                            totalRow[3] = dtList[index].Compute("Sum([SALE QTY])", "[SALE QTY] IS NOT NULL");
                            totalRow[4] = dtList[index].Compute("Sum([TOTAL PRICE])", "[TOTAL PRICE] IS NOT NULL");

                            for (int i = 5; i < dtList[index].Columns.Count; i++)
                            {
                                string colName = dtList[index].Columns[i].ColumnName;
                                //string ex = "Sum([" + colName + "])";
                                //string filter = "[" + colName + "] IS NOT NULL";
                                //totalRow[i] = dtList[index].Compute(ex, filter);
                                totalRow[i] = dtList[index].Compute("Sum([" + colName + "])", "[" + colName + "] IS NOT NULL");
                            }
                        }



                        //dtList[index].Rows.InsertAt(totalRow, rowIndex);
                    }
                }
            }

            return totalRow;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private DataTable AddTotalRow()
    {
        DataTable retDT = new DataTable();
        try
        {
            List<DataTable> dtList = new List<DataTable>();

            if (Session["SaleTargetReport"] != null)
            {
                if (Session["SaleTargetReport"] is List<DataTable>)
                {
                    dtList.AddRange((List<DataTable>)Session["SaleTargetReport"]);
                }
                else if (Session["SaleTargetReport"] is DataTable)
                {
                    dtList.Add((DataTable)Session["SaleTargetReport"]);
                }
            }

            if (Session["SaleTargetReportPrice"] != null)
            {
                if (Session["SaleTargetReportPrice"] is List<DataTable>)
                {
                    dtList.AddRange((List<DataTable>)Session["SaleTargetReportPrice"]);
                }
                else if (Session["SaleTargetReportPrice"] is DataTable)
                {
                    dtList.Add((DataTable)Session["SaleTargetReportPrice"]);
                }
            }

            DataTable _reportDT = new DataTable();
            if (dtList.Count > 0)
            {
                _reportDT = dtList[0].Clone();
                _reportDT.Clear();

                if (Session["SaleTargetReportPrice"] != null)
                {
                    DataRow dr = SubAddTotalRow(dtList, "_TOTAL PRICE", 1);
                    if (dr != null)
                        _reportDT.Rows.Add(dr.ItemArray);
                }
                if (Session["SaleTargetReport"] != null)
                {
                    DataRow dr = SubAddTotalRow(dtList, "_TOTAL QTY", 0);
                    if (dr != null)
                        _reportDT.Rows.Add(dr.ItemArray);
                }

                foreach (DataRow row in dtList[0].Rows)
                {
                    _reportDT.Rows.Add(row.ItemArray);
                }

                retDT = _reportDT;

                if (Session["SaleTargetReport"] == null)
                {
                    retDT = new DataTable();
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
        return retDT;
    }

    private void BindGridView()
    {
        try
        {
            if (Session["SaleTargetReport"] != null)
            {
                DataTable _dt = (DataTable)Session["SaleTargetReport"];
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                    {
                        grdSaleTGReport.DataKeyNames = new string[] { "CUST_ID" };
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                    {
                        grdSaleTGReport.DataKeyNames = new string[] { "WORK_DATE" };
                    }
                    else
                    {
                        grdSaleTGReport.DataKeyNames = new string[] { "SKU_ID" };
                    }

                    if (ddlReportType.SelectedValue.ToString() == "cust" ||
                        ddlReportType.SelectedValue.ToString() == "visit")
                    {
                        grdSaleTGReport.DataSource = AddTotalRow();

                        grdSaleTGReport.DataBind();

                        foreach (GridViewRow row in grdSaleTGReport.Rows)
                        {
                            if (row.RowIndex == 0 || row.RowIndex == 1)
                            {
                                row.ForeColor = System.Drawing.Color.Black;
                                row.Font.Bold = true;
                                row.BackColor = System.Drawing.Color.Orange;
                            }

                            row.Cells[5].Text = row.Cells[5].Text.ToNumberFormat();
                            row.Cells[6].Text = row.Cells[6].Text.ToDecimalFormat();
                            row.Cells[7].Text = row.Cells[7].Text.ToNumberFormat();
                            row.Cells[8].Text = row.Cells[8].Text.ToDecimalFormat();
                            row.Cells[9].Text = row.Cells[9].Text.ToNumberFormat();
                            row.Cells[10].Text = row.Cells[10].Text.ToDecimalFormat();
                            for (int i = 11; i < _dt.Columns.Count; i++)
                            {
                                string decimalVal = row.Cells[i].Text.ToDecimalFormat();
                                string numberVal = row.Cells[i].Text.ToNumberFormat();
                                row.Cells[i].Text = row.RowIndex == 0 ? decimalVal : numberVal;
                            }
                        }
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "cust_visit" ||
                            ddlReportType.SelectedValue.ToString() == "visit_cust")
                    {
                        grdSaleTGReport.DataSource = AddTotalRow();

                        grdSaleTGReport.DataBind();

                        foreach (GridViewRow row in grdSaleTGReport.Rows)
                        {
                            if (row.RowIndex == 0 || row.RowIndex == 1)
                            {
                                row.ForeColor = System.Drawing.Color.Black;
                                row.Font.Bold = true;
                                row.BackColor = System.Drawing.Color.Orange;
                            }

                            if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                            {
                                row.Cells[4].Text = row.Cells[4].Text.ToNumberFormat();
                                row.Cells[5].Text = row.Cells[5].Text.ToDecimalFormat();
                                for (int i = 6; i < _dt.Columns.Count; i++)
                                {
                                    string decimalVal = row.Cells[i].Text.ToDecimalFormat();
                                    string numberVal = row.Cells[i].Text.ToNumberFormat();
                                    row.Cells[i].Text = row.RowIndex == 0 ? decimalVal : numberVal;
                                }
                            }
                            else
                            {
                                row.Cells[3].Text = row.Cells[3].Text.ToNumberFormat();
                                row.Cells[4].Text = row.Cells[4].Text.ToDecimalFormat();
                                for (int i = 5; i < _dt.Columns.Count; i++)
                                {
                                    string decimalVal = row.Cells[i].Text.ToDecimalFormat();
                                    string numberVal = row.Cells[i].Text.ToNumberFormat();
                                    row.Cells[i].Text = row.RowIndex == 0 ? decimalVal : numberVal;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                grdSaleTGReport.DataSource = null;

                grdSaleTGReport.DataBind();

                linkExportReport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void AddEditTotalRow(List<DataTable> dtList)
    {
        foreach (var dt in dtList)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow totalRow = dt.NewRow();
                totalRow["Row_NO"] = 0;
                totalRow["SKU_ID"] = "-99";
                totalRow["SALE_ID"] = "";
                totalRow["SALE_NAME"] = "";
                totalRow["SKU_NAME"] = "Total";
                totalRow["TARGET_QTY"] = dt.Compute("Sum(TARGET_QTY)", "TARGET_QTY IS NOT NULL");
                totalRow["TARGET_PRICE"] = dt.Compute("Sum(TARGET_PRICE)", "TARGET_PRICE IS NOT NULL");
                totalRow["BALANCE_QTY"] = dt.Compute("Sum(BALANCE_QTY)", "BALANCE_QTY IS NOT NULL");
                totalRow["BALANCE_PRICE"] = dt.Compute("Sum(BALANCE_PRICE)", "BALANCE_PRICE IS NOT NULL");
                totalRow["TOTAL_QTY"] = dt.Compute("Sum(TOTAL_QTY)", "TOTAL_QTY IS NOT NULL");
                totalRow["TOTAL_PRICE"] = dt.Compute("Sum(TOTAL_PRICE)", "TOTAL_PRICE  IS NOT NULL");
                totalRow["TARGET_MONTH"] = dt.Rows[0][11]; // ((System.Data.DataRow)((dt.Rows).Items[0])).ItemArray[11];
                totalRow["TARGET_YEAR"] = dt.Rows[0][12];// ((System.Data.DataRow)((dt.Rows).Items[0])).ItemArray[12];

                dt.Rows.InsertAt(totalRow, 0);
            }
        }
    }

    private void BindEditGridView()
    {
        try
        {
            if (Session["SaleTargetEDReport"] != null)
            {
                List<DataTable> dtList = (List<DataTable>)Session["SaleTargetEDReport"];

                if (dtList[0].Rows.Count > 0)
                {
                    AddEditTotalRow(dtList);

                    grdEditSTG.DataSource = dtList[0];

                    grdEditSTG.DataBind();

                    if (grdEditSTG.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in grdEditSTG.Rows)
                        {
                            Label _lblBalance = row.FindControl("lblBALANCE_QTY") as Label;
                            if (_lblBalance != null)
                            {
                                int _balance = 0;
                                bool checkBalance = true;
                                string bl = _lblBalance.Text;

                                if (!string.IsNullOrEmpty(bl))
                                {
                                    if (int.TryParse(bl, out _balance))
                                    {
                                        //do nothing
                                    }
                                    else
                                        _balance = int.Parse(bl, System.Globalization.NumberStyles.AllowThousands);
                                }
                                else
                                    _balance = -1;

                                if (_balance != 0)
                                    checkBalance = false;
                                else
                                    checkBalance = true;

                                if (checkBalance)
                                    row.ForeColor = System.Drawing.Color.Green;
                                else
                                    row.ForeColor = System.Drawing.Color.Red;
                            }

                            HiddenField _hfSKU_ID = row.FindControl("hfSKU_ID") as HiddenField;
                            if (_hfSKU_ID != null)
                            {
                                int _skuId = 0;
                                if (int.TryParse(_hfSKU_ID.Value, out _skuId))
                                {
                                    if (_skuId == -99)
                                    {
                                        row.ForeColor = System.Drawing.Color.Black;
                                        row.Font.Bold = true;
                                        row.BackColor = System.Drawing.Color.Orange;
                                        row.Enabled = false;
                                    }
                                }
                            }
                        }
                    }

                    btnSaveTG.Visible = true;
                    lnkExportEditTarget.Visible = true;
                    linkSumReportTarget.Visible = true;

                    List<string> othList = (new string[] { "-1", "-2", "-3", "-4", "-5", "-6", "-7" }).ToList();
                    if (othList.Contains(ddlED_Emp.SelectedValue.ToString()))
                    {
                        grdEditSTG.Enabled = false;
                    }
                    else
                    {
                        grdEditSTG.Enabled = true;
                    }
                }
                else
                {
                    grdEditSTG.DataSource = null;

                    grdEditSTG.DataBind();

                    btnSaveTG.Visible = false;
                    lnkExportEditTarget.Visible = false;
                    linkSumReportTarget.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private List<DataTable> GetSaleTargetToDataTable(List<string> sale_id_list, bool reportFlag = false)
    {
        List<DataTable> dtRet = new List<DataTable>();
        try
        {

            if (sale_id_list.Count > 0)
            {
                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];
                string cust_id = ddlCustomer.SelectedValue.ToString() == "-1" ? "" : ddlCustomer.SelectedValue.ToString();
                string sku_id = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();
                string visit_date = ddlVisitDate.SelectedValue.ToString() == "-1" ? "" : ddlVisitDate.SelectedValue.ToString();

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    foreach (var sale_id in sale_id_list)
                    {
                        SqlCommand cmd = null;

                        if (ddlReportType.SelectedValue.ToString() == "cust")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report" : "proc_sale_target_cust_report_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "visit")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_visit", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_visit" : "proc_sale_target_cust_report_visit_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_cv", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_cv" : "proc_sale_target_cust_report_cv_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_vc", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_vc" : "proc_sale_target_cust_report_vc_r", cn);
                        }

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SALE_ID", sale_id));
                        cmd.Parameters.Add(new SqlParameter("@TARGET_MONTH", target_month));
                        cmd.Parameters.Add(new SqlParameter("@TARGET_YEAR", target_year));
                        cmd.Parameters.Add(new SqlParameter("@VISIT_DATE", visit_date));
                        cmd.Parameters.Add(new SqlParameter("@CUST_ID", cust_id));
                        cmd.Parameters.Add(new SqlParameter("@SKU_ID", sku_id));

                        cmd.CommandTimeout = 0;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        string rName = "SaleTargetReport-" + sale_id;
                        da.Fill(ds, rName);
                        if (ds.Tables[rName] != null && ds.Tables[rName].Rows.Count > 0)
                        {
                            dtRet.Add(ds.Tables[rName]);
                        }
                        //else
                        //{
                        //    dtRet = null;
                        //}
                    }

                    cn.Close();
                }
            }
            else
            {
                dtRet = new List<DataTable>();
            }

            return dtRet;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    private DataTable GetSaleTargetToDataTable(string sale_id, bool reportFlag = false)
    {
        DataTable dtRet = new DataTable();
        try
        {

            if (!string.IsNullOrEmpty(sale_id))
            {
                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];
                string cust_id = ddlCustomer.SelectedValue.ToString() == "-1" ? "" : ddlCustomer.SelectedValue.ToString();
                string sku_id = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();
                string visit_date = ddlVisitDate.SelectedValue.ToString() == "-1" ? "" : ddlVisitDate.SelectedValue.ToString();

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    if (ddlReportType.SelectedValue.ToString() == "cust")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report" : "proc_sale_target_cust_report_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "visit")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_visit", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_visit" : "proc_sale_target_cust_report_visit_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_cv", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_cv" : "proc_sale_target_cust_report_cv_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_vc", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_vc" : "proc_sale_target_cust_report_vc_r", cn);
                    }

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SALE_ID", sale_id));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_MONTH", target_month));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_YEAR", target_year));
                    cmd.Parameters.Add(new SqlParameter("@VISIT_DATE", visit_date));
                    cmd.Parameters.Add(new SqlParameter("@CUST_ID", cust_id));
                    cmd.Parameters.Add(new SqlParameter("@SKU_ID", sku_id));

                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SaleTargetReport");
                    if (ds.Tables["SaleTargetReport"] != null)
                    {
                        dtRet = ds.Tables["SaleTargetReport"];
                    }
                    else
                    {
                        dtRet = null;
                    }

                    cn.Close();
                }
            }

            return dtRet;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    private DataTable GetSaleTargetPriceToDataTable(string sale_id, bool reportFlag = false)
    {
        DataTable dtRet = new DataTable();
        try
        {
            if (!string.IsNullOrEmpty(sale_id))
            {
                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];
                string cust_id = ddlCustomer.SelectedValue.ToString() == "-1" ? "" : ddlCustomer.SelectedValue.ToString();
                string sku_id = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();
                string visit_date = ddlVisitDate.SelectedValue.ToString() == "-1" ? "" : ddlVisitDate.SelectedValue.ToString();

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    if (ddlReportType.SelectedValue.ToString() == "cust")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_price", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report" : "proc_sale_target_cust_report_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "visit")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_visit_price", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_visit" : "proc_sale_target_cust_report_visit_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_cv_price", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_cv" : "proc_sale_target_cust_report_cv_r", cn);
                    }
                    else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                    {
                        cmd = new SqlCommand("proc_sale_target_cust_report_vc_price", cn);
                        //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_vc" : "proc_sale_target_cust_report_vc_r", cn);
                    }

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SALE_ID", sale_id));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_MONTH", target_month));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_YEAR", target_year));
                    cmd.Parameters.Add(new SqlParameter("@VISIT_DATE", visit_date));
                    cmd.Parameters.Add(new SqlParameter("@CUST_ID", cust_id));
                    cmd.Parameters.Add(new SqlParameter("@SKU_ID", sku_id));

                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SaleTargetReport_Price");
                    if (ds.Tables["SaleTargetReport_Price"] != null)
                    {
                        dtRet = ds.Tables["SaleTargetReport_Price"];
                    }
                    else
                    {
                        dtRet = null;
                    }

                    cn.Close();
                }
            }

            return dtRet;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    private List<DataTable> GetSaleTargetPriceToDataTable(List<string> sale_id_list, bool reportFlag = false)
    {
        List<DataTable> dtRet = new List<DataTable>();
        try
        {
            if (sale_id_list.Count > 0)
            {
                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];
                string cust_id = ddlCustomer.SelectedValue.ToString() == "-1" ? "" : ddlCustomer.SelectedValue.ToString();
                string sku_id = ddlProduct.SelectedValue.ToString() == "-1" ? "" : ddlProduct.SelectedValue.ToString();
                string visit_date = ddlVisitDate.SelectedValue.ToString() == "-1" ? "" : ddlVisitDate.SelectedValue.ToString();

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    foreach (var sale_id in sale_id_list)
                    {
                        SqlCommand cmd = null;

                        if (ddlReportType.SelectedValue.ToString() == "cust")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_price", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report" : "proc_sale_target_cust_report_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "visit")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_visit_price", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_visit" : "proc_sale_target_cust_report_visit_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_cv_price", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_cv" : "proc_sale_target_cust_report_cv_r", cn);
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
                        {
                            cmd = new SqlCommand("proc_sale_target_cust_report_vc_price", cn);
                            //cmd = new SqlCommand(!reportFlag ? "proc_sale_target_cust_report_vc" : "proc_sale_target_cust_report_vc_r", cn);
                        }

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SALE_ID", sale_id));
                        cmd.Parameters.Add(new SqlParameter("@TARGET_MONTH", target_month));
                        cmd.Parameters.Add(new SqlParameter("@TARGET_YEAR", target_year));
                        cmd.Parameters.Add(new SqlParameter("@VISIT_DATE", visit_date));
                        cmd.Parameters.Add(new SqlParameter("@CUST_ID", cust_id));
                        cmd.Parameters.Add(new SqlParameter("@SKU_ID", sku_id));

                        cmd.CommandTimeout = 0;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        string rName = "SaleTargetReport_Price-" + sale_id;

                        da.Fill(ds, rName);
                        if (ds.Tables[rName] != null && ds.Tables[rName].Rows.Count > 0)
                        {
                            dtRet.Add(ds.Tables[rName]);
                        }

                    }

                    cn.Close();
                }
            }
            else
            {
                dtRet = new List<DataTable>();
            }

            return dtRet;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    private void SearchEdit()
    {
        try
        {
            List<DataTable> dtList = new List<DataTable>();
            string sale_id = ddlED_Emp.SelectedValue.ToString();
            dtList.Add(GetEditReportBySaleID(sale_id));

            Session["SaleTargetEDReport"] = dtList;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }

        BindEditGridView();
    }

    private DataTable GetEditReportBySaleID(string sale_id)
    {
        DataTable dt = new DataTable();

        try
        {
            if (!string.IsNullOrEmpty(sale_id))
            {
                string target_month = ddlED_Month.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlED_Year.SelectedValue.ToString().Split('|')[0];

                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand("proc_sale_target_cust_report_edit_list", cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SALE_ID", sale_id));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_MONTH", target_month));
                    cmd.Parameters.Add(new SqlParameter("@TARGET_YEAR", target_year));
                    //cmd.Parameters.Add(new SqlParameter("@ReportType", ""));
                    //cmd.Parameters.Add(new SqlParameter("@CUST_ID", cust_id));
                    //cmd.Parameters.Add(new SqlParameter("@SKU_ID", sku_id));

                    cmd.CommandTimeout = 0;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SaleTargetEDReport");
                    if (ds.Tables["SaleTargetEDReport"] != null)
                    {
                        dt = ds.Tables["SaleTargetEDReport"];
                    }

                    cn.Close();
                }

            }
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    private void ImportSaleTarget()
    {
        List<int> indexList = new List<int>();
        int i = 0;
        try
        {
            string excel_month = ddlExcelMonth.SelectedValue.ToString();
            string excel_year = ddlExcelYear.SelectedValue.ToString();

            //string path = string.Concat(Server.MapPath("~/App_Data/" + FileUpload1.FileName)); //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "~/UploadFile/" + FileUpload1.FileName); //
            //FileUpload1.SaveAs(path);

            //DataTable dt = Helper.exceldata(FileUpload1, path, "import_sale_target");

            string path = string.Concat(Server.MapPath("~/App_Data/" + FileUpload1.FileName));
            FileUpload1.SaveAs(path);

            var dt = new DataTable();
            dt = Helper.ReadExcelToDataTable(conString, FileUpload1.FileName, "import_sale_target");

            //DataTable dt = Helper.GetSheetDataAsDataTable(path, "import_sale_target");

            using (SqlConnection dataConnection = new SqlConnection(conString))
            {
                using (SqlCommand dataCommand = dataConnection.CreateCommand())
                {
                    dataConnection.Open();

                    string cmd_clear = "DELETE FROM tbl_sale_target WHERE TARGET_MONTH = " + Convert.ToInt32(excel_month) + " AND TARGET_YEAR = " + Convert.ToInt32(excel_year);

                    dataCommand.CommandType = CommandType.Text;
                    dataCommand.CommandTimeout = 0;
                    dataCommand.CommandText = cmd_clear;
                    dataCommand.ExecuteNonQuery();

                    dataConnection.Close();

                    dataConnection.Open();

                    foreach (DataRow row in dt.Rows)
                    {
                        string SALE_ID = "";
                        string CUST_ID = "";
                        int VISIT_DATE = 0;

                        foreach (DataColumn column in dt.Columns)
                        {
                            string columnName = column.ColumnName;
                            string columnData = row[column].ToString();

                            if (columnName == "SALE_ID")
                            {
                                SALE_ID = row[column].ToString();
                            }
                            if (columnName == "CUST_ID")
                            {
                                CUST_ID = row[column].ToString();
                            }
                            if (columnName == "VISIT_DATE")
                            {
                                VISIT_DATE = Convert.ToInt32(row[column].ToString());
                            }

                            if (!string.IsNullOrEmpty(columnData) && columnName != "SALE_ID" && columnName != "CUST_ID" && columnName != "VISIT_DATE")
                            {
                                decimal colValue = 0;
                                if (decimal.TryParse(columnData, out colValue))
                                {
                                    if (colValue > 0 && !string.IsNullOrEmpty(SALE_ID) && VISIT_DATE != 0)
                                    {

                                        string cmd = "";
                                        cmd += "INSERT INTO tbl_sale_target ";
                                        cmd += "       (SALE_QTY ";
                                        cmd += "        , VISIT_DATE ";
                                        cmd += "        , SALE_ID ";
                                        cmd += "        , SKU_ID ";
                                        cmd += "        , CUST_ID ";
                                        cmd += "        , TARGET_MONTH ";
                                        cmd += "        , TARGET_YEAR ";
                                        cmd += "        , UPDATE_DATE) ";
                                        cmd += " VALUES ";
                                        cmd += "     (  " + Convert.ToInt32(colValue);
                                        cmd += "        ," + VISIT_DATE;
                                        cmd += "        ,'" + SALE_ID + "' ";
                                        cmd += "        ,'" + columnName + "' ";
                                        cmd += "        ,'" + CUST_ID + "' ";
                                        cmd += "        ," + Convert.ToInt32(excel_month);
                                        cmd += "        ," + Convert.ToInt32(excel_year);
                                        cmd += "        ,GETDATE()); ";

                                        dataCommand.CommandType = CommandType.Text;
                                        dataCommand.CommandTimeout = 0;
                                        dataCommand.CommandText = cmd;
                                        dataCommand.ExecuteNonQuery();

                                        indexList.Add(i);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }

                dataConnection.Close();
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private string AddExcelToDB(FileUpload fileUP, string sheetName, string tableName)
    {
        string result = "";

        try
        {
            if (fileUP.HasFile)
            {
                string path = string.Concat(Server.MapPath("~/App_Data/" + fileUP.FileName));
                fileUP.SaveAs(path);

                var dataTable = new DataTable();
                dataTable = Helper.ReadExcelToDataTable(conString, fileUP.FileName, sheetName);

                var _dt = new DataTable();
                if (sheetName == "products_master")
                {
                    _dt.Columns.Add("SKU_ID", typeof(string));
                    _dt.Columns.Add("SKU_NAME", typeof(string));
                    _dt.Columns.Add("PRICE", typeof(decimal));
                    _dt.Columns.Add("SKU_TYPE", typeof(string));
                    _dt.Columns.Add("SKU_GROUP_ID", typeof(string));
                    _dt.Columns.Add("PrdSubGrpID", typeof(string));
                    _dt.Columns.Add("SellPrice", typeof(decimal));
                    _dt.Columns.Add("UPDATE_DATE", typeof(DateTime));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string skuID = row["SKU_ID"].ToString();
                        Dictionary<string, object> prd_params = new Dictionary<string, object>();
                        prd_params.Add("SKU_ID", skuID);

                        DataTable prdDT = Helper.ExecuteProcedureToTable(conString, "proc_sale_target_cust_report_fix_prod_master", prd_params);
                        if (prdDT != null && prdDT.Rows.Count > 0)
                        {
                            DataRow _prdRow = prdDT.AsEnumerable().FirstOrDefault(x => x.Field<string>("PrdCode") == skuID);
                            if (_prdRow != null)
                                _dt.Rows.Add(row["SKU_ID"], row["SKU_NAME"], row["PRICE"], row["SKU_TYPE"], _prdRow["PrdGrpCode"], _prdRow["PrdSubGrpID"], _prdRow["SellPrice"], DateTime.Now);
                        }
                    }
                }
                else if (sheetName == "customers_master")
                {
                    _dt.Columns.Add("CUSTOMER_ID", typeof(string));
                    _dt.Columns.Add("CUSTOMER_NAME", typeof(string));
                    _dt.Columns.Add("SALE_ID", typeof(string));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string _CUSTOMER_ID = row["CUSTOMER_ID"].ToString();
                        string _CUSTOMER_NAME = row["CUSTOMER_NAME"].ToString();
                        string _SALE_ID = row["SALE_ID"].ToString();

                        if (!string.IsNullOrEmpty(_CUSTOMER_ID) && !string.IsNullOrEmpty(_CUSTOMER_NAME) && !string.IsNullOrEmpty(_SALE_ID))
                            _dt.Rows.Add(_CUSTOMER_ID, _CUSTOMER_NAME, _SALE_ID);
                    }
                }
                else if (sheetName == "sales_target")
                {
                    _dt.Columns.Add("SKU_CODE", typeof(string));
                    _dt.Columns.Add("SKU_TARGET", typeof(int));
                    _dt.Columns.Add("SALE_ID", typeof(string));

                    Dictionary<string, int> saleIDList = new Dictionary<string, int>();
                    int saleIDRowIndex = 4;

                    var row5 = dataTable.Rows[saleIDRowIndex]; //fix sale row index

                    for (int colIndex = 0; colIndex < row5.ItemArray.Length; colIndex++)
                    {
                        int verifyNumber = 0;
                        var sale_row = row5[colIndex].ToString();
                        if (!string.IsNullOrEmpty(sale_row) && int.TryParse(sale_row, out verifyNumber))
                        {
                            string _saleID = "";
                            if (verifyNumber < 10)
                                _saleID = "S00";

                            else
                                _saleID = "S0";

                            _saleID += verifyNumber.ToString();
                            saleIDList.Add(_saleID, colIndex);
                        } 
                    }

                    foreach (KeyValuePair<string, int> col in saleIDList)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string _SKU_CODE = "";
                            string skuID = row[0].ToString();
                            if (!string.IsNullOrEmpty(skuID))
                            {
                                Dictionary<string, object> prd_params = new Dictionary<string, object>();
                                prd_params.Add("SKU_ID", skuID);

                                DataTable prdDT = Helper.ExecuteProcedureToTable(conString, "proc_sale_target_cust_check_exist_prod", prd_params);
                                if (prdDT != null && prdDT.Rows.Count > 0)
                                {
                                    string ret = prdDT.Rows[0]["Result"].ToString();
                                    if (ret == "1")
                                        _SKU_CODE = row[0].ToString();
                                }

                                string _SALE_ID = col.Key;

                                decimal _SKU_TARGET = 0;
                                if (!string.IsNullOrEmpty(row[col.Value].ToString()))
                                    _SKU_TARGET = Helper.ConvertColToInt(row, col.Value); //row[col.Value].ToString();

                                if (!string.IsNullOrEmpty(_SKU_CODE) && !string.IsNullOrEmpty(_SALE_ID))
                                    _dt.Rows.Add(_SKU_CODE, _SKU_TARGET, _SALE_ID);
                            }
                        }
                    }
                }

                if (_dt != null && _dt.Rows.Count > 0)
                {
                    dataTable = new DataTable();
                    dataTable = _dt;
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Bulk Copy to SQL Server 
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(conString);
                    bulkInsert.DestinationTableName = tableName;
                    bulkInsert.WriteToServer(dataTable);
                }
            }
        }
        catch (Exception ex)
        {
            result = ex.Message;
            throw ex;
        }

        return result;
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        SearchReport();
    }

    private List<DataTable> GetAllEmpReport()
    {
        empList = new Dictionary<string, string>();
        List<DataTable> _dtList = new List<DataTable>();
        List<DataTable> _reportDTList = new List<DataTable>();

        string sale_id = ddlEmployee.SelectedValue.ToString() == "-1" ? "" : ddlEmployee.SelectedValue.ToString();

        if (chkExportAllSale.Checked)
        {
            empList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getEmployee", "SALE_ID", "SALE_ID", "SALE_NAME");

            //List<string> bkk = new List<string>() { "S001", "S002", "S003", "S004" };
            //List<string> oth = new List<string>() { "S005", "S006", "S007", "S008", "S009", "S010", "S022", "S028", "S029" };

            //foreach (var item in empList)
            {
                //_dtList.Add(GetSaleTargetToDataTable(item.Key, true));
                //if (bkk.Contains(item.Key) || oth.Contains(item.Key))
                {
                    var sale_id_list = empList.Select(x => x.Key).ToList();

                    var qList = GetSaleTargetToDataTable(sale_id_list, true);
                    var pList = GetSaleTargetPriceToDataTable(sale_id_list, true);

                    for (int i = 0; i < qList.Count; i++)
                    {
                        Session["SaleTargetReport"] = qList[i];
                        Session["SaleTargetReportPrice"] = pList[i];
                        var dt = AddTotalRow();
                        if (dt.Rows.Count > 3)
                        {
                            _dtList.Add(dt);
                        }
                    }
                }
            }
        }
        else
        {
            //DataTable _dt = (DataTable)grdSaleTGReport.DataSource;
            //_dtList.Add(_dt);
            var q = Session["SaleTargetReport"];
            var p = Session["SaleTargetReportPrice"];
            //Session["SaleTargetReport"] = GetSaleTargetToDataTable(sale_id, true);
            //Session["SaleTargetReportPrice"] = GetSaleTargetPriceToDataTable(sale_id, true);
            var dt = AddTotalRow();
            if (dt.Rows.Count > 3)
            {
                _dtList.Add(dt);
            }
        }

        if (_dtList != null && _dtList.Count > 0)
        {
            foreach (var _dt in _dtList)
            {
                DataTable _reportDT = new DataTable();
                _reportDT = _dt.Clone();
                _reportDT.Clear();

                for (int i = 0; i < _reportDT.Columns.Count; i++)
                {
                    _reportDT.Columns[i].DataType = typeof(string);
                }

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    DataRow newRow = _reportDT.NewRow();
                    _reportDT.Rows.InsertAt(newRow, i);

                    for (int j = 0; j < _dt.Columns.Count; j++)
                    {
                        if (ddlReportType.SelectedValue.ToString() == "cust" || ddlReportType.SelectedValue.ToString() == "visit")
                        {
                            string _Numbervalue = "";
                            string _decimalValue = "";
                            if (j >= 5)
                            {
                                _Numbervalue = _dt.Rows[i][j].ToString().ToNumberFormat();
                                _decimalValue = _dt.Rows[i][j].ToString().ToDecimalFormat();
                            }

                            if (j >= 5 && j <= 10)
                                _reportDT.Rows[i][j] = _Numbervalue;
                            else if (j >= 11)
                            {
                                _reportDT.Rows[i][j] = i == 0 ? _decimalValue : _Numbervalue;
                            }
                            else
                                _reportDT.Rows[i][j] = _dt.Rows[i][j].ToString();
                        }
                        else if (ddlReportType.SelectedValue.ToString() == "cust_visit" || ddlReportType.SelectedValue.ToString() == "visit_cust")
                        {
                            if (ddlReportType.SelectedValue.ToString() == "cust_visit")
                            {
                                string _Numbervalue = "";
                                string _decimalValue = "";
                                if (j >= 4)
                                {
                                    _Numbervalue = _dt.Rows[i][j].ToString().ToNumberFormat();
                                    _decimalValue = _dt.Rows[i][j].ToString().ToDecimalFormat();
                                }

                                if (j >= 4 && j <= 5)
                                    _reportDT.Rows[i][j] = _Numbervalue;
                                else if (j >= 6)
                                {
                                    _reportDT.Rows[i][j] = i == 0 ? _decimalValue : _Numbervalue;
                                }
                                else
                                    _reportDT.Rows[i][j] = _dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                string _Numbervalue = "";
                                string _decimalValue = "";
                                if (j >= 3)
                                {
                                    _Numbervalue = _dt.Rows[i][j].ToString().ToNumberFormat();
                                    _decimalValue = _dt.Rows[i][j].ToString().ToDecimalFormat();
                                }

                                if (j >= 3 && j <= 4)
                                    _reportDT.Rows[i][j] = _Numbervalue;
                                else if (j >= 5)
                                {
                                    _reportDT.Rows[i][j] = i == 0 ? _decimalValue : _Numbervalue;
                                }
                                else
                                    _reportDT.Rows[i][j] = _dt.Rows[i][j].ToString();
                            }
                        }
                    }
                }
                _reportDTList.Add(_reportDT);
            }
        }

        return _reportDTList;
    }

    protected void linkExportReport_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "closeDialog()", true);

            List<DataTable> _dtList = new List<DataTable>();
            _dtList = (List<DataTable>)Session["STG_AllReportEmp"];

            string excelReportName = "SaleTargetReport";

            List<string> reportName = new List<string>();
            reportName.Add(excelReportName);

            string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
            string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];

            string currentDate = DateTime.Now.ToString("ddMMyyyyhhmmss");

            if (ddlReportType.SelectedValue.ToString() == "cust")
            {
                excelReportName = excelReportName + "_" + "customer-" + target_month + "-" + target_year + "-" + currentDate;
            }
            else if (ddlReportType.SelectedValue.ToString() == "visit")
            {
                excelReportName = excelReportName + "_" + "work_date-" + target_month + "-" + target_year + "-" + currentDate;
            }
            else if (ddlReportType.SelectedValue.ToString() == "cust_visit")
            {
                excelReportName = excelReportName + "_" + "cust-work_date-" + target_month + "-" + target_year + "-" + currentDate;
            }
            else if (ddlReportType.SelectedValue.ToString() == "visit_cust")
            {
                excelReportName = excelReportName + "_" + "work_date-cust-" + target_month + "-" + target_year + "-" + currentDate;
            }

            SubGetReport(_dtList, excelReportName);
            //SubGetReport(_reportOTH, "ต่างจังหวัด-" + excelReportName);

        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    private void SubGetReport(List<DataTable> _reportDTList, string excelReportName)
    {
        XLWorkbook wb = new XLWorkbook();
        using (XLWorkbook _wb = new XLWorkbook())
        {
            foreach (DataTable _dt in _reportDTList)
            {
                if (_dt.Rows.Count > 2)
                {
                    string saleName = _dt.Rows[2]["SALE NAME"].ToString().ToCharArray().Count() < 27 ? _dt.Rows[2]["SALE NAME"].ToString().Replace(' ', '_') : _dt.Rows[2]["SALE NAME"].ToString().Substring(0, 27).Replace(' ', '_');
                    string sheetName = _dt.Rows[2]["SALE ID"].ToString() + "-" + saleName;
                    _wb.Worksheets.Add(_dt, sheetName);
                }
            }

            wb = _wb;
        }

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=" + excelReportName + ".xlsx");
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
            wb.SaveAs(MyMemoryStream);

            MyMemoryStream.WriteTo(Response.OutputStream);
        }
        Response.Flush();
        Response.End();
    }

    protected void btnED_Search_Click(object sender, EventArgs e)
    {
        SearchEdit();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void grdEditSTG_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //grdEditSTG.EditIndex = e.NewEditIndex;
        //BindEditGridView();
    }

    protected void grdEditSTG_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //TextBox txtTargetQty = grdEditSTG.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        bool checkImportFalg = false;
        if (chkimport_sale_target.Checked)
        {
            ImportSaleTarget();
            checkImportFalg = true;
        }

        //TestReadExcel();

        List<string> results = new List<string>();
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

                string path = string.Concat(Server.MapPath("~/App_Data/" + FileUpload1.FileName)); //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "~/UploadFile/" + FileUpload1.FileName); //
                FileUpload1.SaveAs(path);

                //string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //string path = Server.MapPath("~/App_Data/" + filename);
                //FileUpload1.PostedFile.SaveAs(path);

                //string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";

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

                string excel_month = ddlExcelMonth.SelectedValue.ToString();
                string excel_year = ddlExcelYear.SelectedValue.ToString();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("month", excel_month);
                parameters.Add("year", excel_year);

                //// Connection String to Excel Workbook
                //string excelCS = string.Format(fullPath);
                //using (OleDbConnection con = new OleDbConnection(excelCS))
                {
                    if (chkcustomers_master.Checked)
                    {
                        Dictionary<string, string> procedureParam1 = new Dictionary<string, string>();
                        procedureParam1.Add("tableName", "tbl_customer_master");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam1);
                        results.Add(AddExcelToDB(FileUpload1, "customers_master", "tbl_customer_master"));


                        Dictionary<string, string> procedureParam3 = new Dictionary<string, string>();
                        procedureParam3.Add("tableName", "tbl_work_date_mapping");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam3);
                        results.Add(AddExcelToDB(FileUpload1, "work_date_mapping", "tbl_work_date_mapping"));
                        Helper.ExecuteProcedure(conString, "proc_sale_target_cust_workdate_his_update", null);


                        Dictionary<string, string> procedureParam2 = new Dictionary<string, string>();
                        procedureParam2.Add("tableName", "tbl_temp_cust_visit_date");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam2);
                        results.Add(AddExcelToDB(FileUpload1, "sales_visit_date", "tbl_temp_cust_visit_date"));
                        Helper.ExecuteProcedure(conString, "proc_init_customer_master_dt");

                        checkImportFalg = true;
                    }

                    if (chkproducts_master.Checked)
                    {
                        Dictionary<string, string> procedureParam1 = new Dictionary<string, string>();
                        procedureParam1.Add("tableName", "tbl_product_master");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam1);
                        results.Add(AddExcelToDB(FileUpload1, "products_master", "tbl_product_master"));
                        Helper.ExecuteProcedure(conString, "proc_init_prod_master");

                        Dictionary<string, string> procedureParam2 = new Dictionary<string, string>();
                        procedureParam2.Add("tableName", "tbl_temp_prod_target");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam2);
                        results.Add(AddExcelToDB(FileUpload1, "sales_target", "tbl_temp_prod_target"));

                        Helper.ExecuteProcedure(conString, "proc_init_prod_sale_target", parameters);

                        checkImportFalg = true;
                    }

                    if (chkusers_master.Checked)
                    {
                        Dictionary<string, string> procedureParam = new Dictionary<string, string>();
                        procedureParam.Add("tableName", "tbl_user");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam);
                        results.Add(AddExcelToDB(FileUpload1, "users_master", "tbl_user"));

                        checkImportFalg = true;
                    }

                    if (chksales_master.Checked)
                    {
                        Dictionary<string, string> procedureParam = new Dictionary<string, string>();
                        procedureParam.Add("tableName", "tbl_seller_master");
                        Helper.ExecuteProcedure(conString, "proc_sale_target_clear_master_data", procedureParam);
                        results.Add(AddExcelToDB(FileUpload1, "sales_master", "tbl_seller_master"));

                        checkImportFalg = true;
                    }

                    if (chkNextSaletarget.Checked)
                    {
                        Helper.ExecuteProcedure(conString, "proc_init_next_sale_target", parameters);

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
                        //lblMessage.Text = "Your file uploaded successfully";
                        //lblMessage.ForeColor = System.Drawing.Color.Green;

                        lblUploadResult.Text = "อัพโหลด excel เรียบร้อยแล้ว!";
                        lblUploadResult.ForeColor = System.Drawing.Color.Green;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
                    }
                    else
                    {
                        //lblUploadResult.Text = "1) เกิดข้อผิดพลาดในการอัพโหลด excel!";
                        List<string> errMsgList = results.Where(x => !string.IsNullOrEmpty(x)).ToList();
                        for (int i = 0; i < errMsgList.Count; i++)
                        {
                            errMsgList[i] = (i + 1).ToString() + ") " + errMsgList[i];
                        }
                        string errMsg = "";
                        errMsg = string.Join("<br />", errMsgList);
                        lblUploadResult.Text = errMsg;
                        lblUploadResult.ForeColor = System.Drawing.Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);

                        //lblMessage.Text = "Your file not uploaded";
                        //lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Your file not uploaded";
                //lblMessage.ForeColor = System.Drawing.Color.Red;

                //Helper.WriteLog(ex.Message);
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

            //lblMessage.Text = "Your file not uploaded";
            //lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindCustomer();

        BindVisitDate();
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchReport();
    }

    protected void ddlSTGMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, string> customerList = new Dictionary<string, string>();

        string sale_id = ddlEmployee.SelectedValue.ToString();
        string target_month = ddlSTGMonth.SelectedValue.ToString();
        string target_year = ddlSTGYear.SelectedValue.ToString();

        if (!string.IsNullOrEmpty(sale_id))
        {
            BindVisitDate();
        }
    }

    protected void ddlSTGYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, string> customerList = new Dictionary<string, string>();

        string sale_id = ddlEmployee.SelectedValue.ToString();
        string target_month = ddlSTGMonth.SelectedValue.ToString();
        string target_year = ddlSTGYear.SelectedValue.ToString();

        if (!string.IsNullOrEmpty(sale_id))
        {
            BindVisitDate();
        }

    }

    protected void lnkExportEditTarget_Click(object sender, EventArgs e)
    {
        try
        {
            List<DataTable> _dtList = new List<DataTable>();

            if (chkAllSalseReport.Checked)
            {
                if (empList.Count == 0)
                {
                    empList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getEmployee", "SALE_ID", "SALE_ID", "SALE_NAME");
                }

                if (empList.Count > 0)
                {
                    foreach (KeyValuePair<string, string> emp in empList)
                    {
                        _dtList.Add(GetEditReportBySaleID(emp.Key));
                    }
                }
            }
            else
            {
                string sale_id = ddlED_Emp.SelectedValue.ToString();
                _dtList.Add(GetEditReportBySaleID(sale_id));
            }

            AddEditTotalRow(_dtList);

            if (_dtList != null && _dtList.Count > 0)
            {
                string excelReportName = "SummaryTarget_";
                string sheetName = "";

                string target_month = ddlSTGMonth.SelectedValue.ToString().Split('|')[0];
                string target_year = ddlSTGYear.SelectedValue.ToString().Split('|')[0];

                string currentDate = DateTime.Now.ToString("ddMMyyyyhhmmss");

                if (chkAllSalseReport.Checked)
                {
                    excelReportName = excelReportName + "รายบุคคล_" + target_month + "-" + target_year + "-" + currentDate;
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-1")
                {
                    excelReportName = excelReportName + "ทั้งหมด_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ทั้งหมด";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-2")
                {
                    excelReportName = excelReportName + "กรุงเทพฯ_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "กรุงเทพฯ";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-3")
                {
                    excelReportName = excelReportName + "ต่างจังหวัด_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ต่างจังหวัด";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-4")
                {
                    excelReportName = excelReportName + "ภาคเหนือ_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ภาคเหนือ";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-5")
                {
                    excelReportName = excelReportName + "ภาคอีสาน_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ภาคอีสาน";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-6")
                {
                    excelReportName = excelReportName + "ภาคกลาง_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ภาคกลาง";
                }
                else if (ddlED_Emp.SelectedValue.ToString() == "-7")
                {
                    excelReportName = excelReportName + "ภาคใต้_" + target_month + "-" + target_year + "-" + currentDate;
                    sheetName = "ภาคใต้";
                }
                else
                {
                    if (!chkAllSalseReport.Checked)
                    {
                        var row = _dtList[0].Rows[1];
                        sheetName = row["SALE_NAME"].ToString().ToCharArray().Count() < 31 ? row["SALE_NAME"].ToString().Replace(' ', '_').Replace(':', '-') : row["SALE_NAME"].ToString().Substring(0, 30).Replace(' ', '_').Replace(':', '-');
                    }
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    List<DataTable> _reportDTList = new List<DataTable>();
                    List<int> decimalList = new List<int>() { 6, 8, 10 };
                    List<int> numberList = new List<int>() { 5, 7, 9 };

                    foreach (var _dt in _dtList)
                    {
                        if (_dt != null && _dt.Rows.Count > 0)
                        {
                            DataTable _reportDT = new DataTable();
                            _reportDT = _dt.Clone();
                            _reportDT.Clear();

                            for (int i = 0; i < _reportDT.Columns.Count; i++)
                            {
                                _reportDT.Columns[i].DataType = typeof(string);
                            }

                            for (int i = 0; i < _dt.Rows.Count; i++)
                            {
                                DataRow newRow = _reportDT.NewRow();
                                _reportDT.Rows.InsertAt(newRow, i);

                                for (int j = 0; j < _dt.Columns.Count; j++)
                                {
                                    string _Numbervalue = "";
                                    string _decimalValue = "";
                                    if (j >= 5)
                                    {
                                        _Numbervalue = _dt.Rows[i][j].ToString().ToNumberFormat();
                                        _decimalValue = _dt.Rows[i][j].ToString().ToDecimalFormat();
                                    }

                                    if (decimalList.Contains(j))
                                        _reportDT.Rows[i][j] = _decimalValue;
                                    else if (numberList.Contains(j))
                                    {
                                        _reportDT.Rows[i][j] = _Numbervalue;
                                    }
                                    else
                                        _reportDT.Rows[i][j] = _dt.Rows[i][j].ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(_reportDT.Rows[1]["SALE_NAME"].ToString()))
                                _reportDTList.Add(_reportDT);
                        }
                    }

                    if (empList.Count > 0 && chkAllSalseReport.Checked)
                    {
                        foreach (DataTable _dt in _reportDTList)
                        {
                            if (_dt.Rows.Count > 1)
                            {
                                if (!string.IsNullOrEmpty(_dt.Rows[1]["SALE_NAME"].ToString()))
                                {
                                    sheetName = _dt.Rows[1]["SALE_NAME"].ToString().ToCharArray().Count() < 31 ? _dt.Rows[1]["SALE_NAME"].ToString().Replace(' ', '_').Replace(':', '-') : _dt.Rows[1]["SALE_NAME"].ToString().Substring(0, 30).Replace(' ', '_').Replace(':', '-');
                                    wb.Worksheets.Add(_dt, sheetName);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_reportDTList[0].Rows.Count > 1)
                        {
                            wb.Worksheets.Add(_reportDTList[0], sheetName);
                        }
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + excelReportName + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                    }
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void ddlED_Emp_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchEdit();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ActiveTab(1)", true);
    }

    protected void chkExportAllSale_CheckedChanged(object sender, EventArgs e)
    {
        Session["STG_AllReportEmp"] = GetAllEmpReport();
    }

    protected void linkSumReportTarget_Click(object sender, EventArgs e)
    {
        List<DataTable> _dtListTemp = new List<DataTable>();
        List<DataTable> _dtList = new List<DataTable>();
        empList = new Dictionary<string, string>();

        if (empList.Count == 0)
            empList.StoredProceduresToListWith2Value(conString, "proc_sale_target_cust_report_getEmployee", "SALE_ID", "SALE_ID", "SALE_NAME");

        if (empList.Count > 0)
        {
            foreach (KeyValuePair<string, string> emp in empList)
            {
                _dtListTemp.Add(GetEditReportBySaleID(emp.Key));
            }

            foreach (DataTable item in _dtListTemp)
            {
                if (item.Rows.Count > 3)
                    _dtList.Add(item);
            }

            DataTable dt = new DataTable("SumReportTarget");

            dt.Columns.Add("ROW_NO");
            dt.Columns.Add("SKU_ID");
            dt.Columns.Add("SKU_NAME");
            dt.Columns.Add("TOTAL_QTY");
            dt.Columns.Add("TOTAL_PRICE");
            foreach (KeyValuePair<string, string> emp in empList)
            {
                dt.Columns.Add(emp.Key + "_QTY");
                dt.Columns.Add(emp.Key + "_PRICE");
            }

            Dictionary<string, decimal> sumQty = new Dictionary<string, decimal>();
            Dictionary<string, decimal> sumPrice = new Dictionary<string, decimal>();

            for (int i = (_dtList[0].Rows.Count - 1); i >= 0; i--)
            {
                DataTable item = _dtList[0];
                List<DataRow> list = item.AsEnumerable().ToList();

                DataRow _row = dt.NewRow();
                _row["ROW_NO"] = item.Rows[i]["ROW_NO"];
                _row["SKU_ID"] = item.Rows[i]["SKU_ID"];
                _row["SKU_NAME"] = item.Rows[i]["SKU_NAME"];

                decimal total_qty = 0;
                decimal total_price = 0;

                foreach (DataTable item2 in _dtList)
                {
                    var col1 = item2.Rows[i]["SALE_ID"] + "_QTY";
                    var col2 = item2.Rows[i]["SALE_ID"] + "_PRICE";

                    _row[col1] = item2.Rows[i]["TARGET_QTY"];
                    _row[col2] = item2.Rows[i]["TARGET_PRICE"];

                    total_qty += Convert.ToDecimal(item2.Rows[i]["TARGET_QTY"]);
                    total_price += Convert.ToDecimal(item2.Rows[i]["TARGET_PRICE"]);

                    string saleName = string.Join("|", item2.Rows[i]["SALE_ID"].ToString(), item.Rows[i]["SKU_ID"].ToString());

                    sumQty.Add(saleName, Convert.ToDecimal(item2.Rows[i]["TARGET_QTY"]));
                    sumPrice.Add(saleName, Convert.ToDecimal(item2.Rows[i]["TARGET_PRICE"]));
                }

                _row["TOTAL_QTY"] = total_qty;
                _row["TOTAL_PRICE"] = total_price;

                dt.Rows.InsertAt(_row, 0);
            }

            if (dt.Rows.Count > 0)
            {
                DataRow totalRow = dt.NewRow();
                totalRow["ROW_NO"] = 0;
                totalRow["SKU_ID"] = "";
                totalRow["SKU_NAME"] = "Total";

                decimal total_qty = 0;
                decimal total_price = 0;

                foreach (KeyValuePair<string, string> emp in empList)
                {
                    var col1 = emp.Key + "_QTY";
                    var col2 = emp.Key + "_PRICE";

                    decimal sQty = sumQty.Where(x => x.Key.Split('|')[0].ToString() == emp.Key).Sum(a => a.Value);
                    decimal sPrice = sumPrice.Where(x => x.Key.Split('|')[0].ToString() == emp.Key).Sum(a => a.Value);
                    totalRow[col1] = sQty;
                    totalRow[col2] = sPrice;

                    total_qty += sQty;
                    total_price += sPrice;
                }

                totalRow["TOTAL_QTY"] = total_qty;
                totalRow["TOTAL_PRICE"] = total_price;

                dt.Rows.InsertAt(totalRow, 0);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                List<DataTable> _reportDTList = new List<DataTable>();
                List<int> decimalList = new List<int>() { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
                List<int> numberList = new List<int>() { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27 };

                var _dt = dt;

                DataTable _reportDT = new DataTable();
                _reportDT = _dt.Clone();
                _reportDT.Clear();

                for (int i = 0; i < _reportDT.Columns.Count; i++)
                {
                    _reportDT.Columns[i].DataType = typeof(string);
                }

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    DataRow newRow = _reportDT.NewRow();
                    _reportDT.Rows.InsertAt(newRow, i);

                    for (int j = 0; j < _dt.Columns.Count; j++)
                    {
                        string _Numbervalue = "";
                        string _decimalValue = "";
                        if (j >= 2)
                        {
                            _Numbervalue = _dt.Rows[i][j].ToString().ToNumberFormat();
                            _decimalValue = _dt.Rows[i][j].ToString().ToDecimalFormat();
                        }

                        if (decimalList.Contains(j))
                            _reportDT.Rows[i][j] = _decimalValue;
                        else if (numberList.Contains(j))
                        {
                            _reportDT.Rows[i][j] = _Numbervalue;
                        }
                        else
                            _reportDT.Rows[i][j] = _dt.Rows[i][j].ToString();
                    }
                }


                if (_reportDT.Rows.Count > 1)
                {
                    wb.Worksheets.Add(_reportDT, "รายงาน");
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + "รายงานสรุป Target" + ".xlsx");
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

    protected void grdSaleTGReport_DataBound(object sender, EventArgs e)
    {

    }

    protected void grdSaleTGReport_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
}














//private string AddExcelToDB(OleDbConnection con, string sheetName, string tableName)
//{
//    string result = "";
//    try
//    {
//        OleDbCommand cmd = new OleDbCommand("select * from [" + sheetName + "$]", con);
//        con.Open();
//        // Create DbDataReader to Data Worksheet
//        DbDataReader dr = cmd.ExecuteReader();
//        // SQL Server Connection String
//        string CS = ConfigurationManager.ConnectionStrings["myConnectionString_saletarget"].ConnectionString;
//        // Bulk Copy to SQL Server 
//        SqlBulkCopy bulkInsert = new SqlBulkCopy(CS);
//        bulkInsert.DestinationTableName = tableName; //"tbl_temp_cust_visit_date";
//        bulkInsert.WriteToServer(dr);
//        //BindGridview();
//        con.Close();
//    }
//    catch (Exception ex)
//    {
//        con.Close();

//        result = ex.Message;
//        //lblUploadResult.Text = ex.Message;
//        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowResultUpload()", true);
//        //throw ex;
//    }

//    return result;
//}
