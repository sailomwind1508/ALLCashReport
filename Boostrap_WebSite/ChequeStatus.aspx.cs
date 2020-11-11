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
using System.IO;
using ClosedXML.Excel;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;

public partial class ChequeStatus : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    static List<VanDetails> vanList = new List<VanDetails>();
    static List<SaleArea> salArea = new List<SaleArea>();
    List<User> LoginUser = new List<User>();

    ReportDocument rdoc = new ReportDocument();
    bool validateFilter = false;
    string connStrng = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    DataTable _dt = new DataTable();

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
        var requestArgs = this.Request["__EVENTARGUMENT"];
        if (requestTarget == "save")
        {
            SaveData();
        }

        if (chkShowAll.Checked)
        {
            grdChequeStatus.AllowPaging = false;
        }
        else
        {
            grdChequeStatus.AllowPaging = true;
        }

        //if (ViewState["ChequeStatusReport"] != null)
        //{
        //    grdChequeStatus.DataSource = (DataTable)ViewState["ChequeStatusReport"];
        //    grdChequeStatus.DataBind();
        //}
    }

    private void InitPage()
    {
        Dictionary<string, string> companyList = new Dictionary<string, string>();

        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("proc_ChequeStatusReport_GetCompany", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                companyList.Add("-1", "---All---");
                while (reader.Read())
                {
                    companyList.Add(reader["RowNum"].ToString(), reader["Company"].ToString());
                }

                cn.Close();
            }

            ddlCompany.Items.Clear();
            ddlCompany.DataSource = companyList;
            ddlCompany.DataTextField = "Value";
            ddlCompany.DataValueField = "Key";
            ddlCompany.DataBind();

            Dictionary<string, string> statusList = new Dictionary<string, string>();
            statusList.Add("All", "---All---");
            statusList.Add("Y", "---Received---");
            statusList.Add("N", "---Not Receive---");


            ddlStatus.Items.Clear();
            ddlStatus.DataSource = statusList;
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataBind();


            Dictionary<string, string> statementList = new Dictionary<string, string>();
            statementList.Add("All", "---All---");
            statementList.Add("Y", "มีข้อมูล Statement");
            statementList.Add("N", "ไม่มีข้อมูล Statement");


            ddlStatement.Items.Clear();
            ddlStatement.DataSource = statementList;
            ddlStatement.DataTextField = "Value";
            ddlStatement.DataValueField = "Key";
            ddlStatement.DataBind();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw;
        }
    }

    private void BindGridView()
    {
        try
        {
            if (chkShowAll.Checked)
            {
                grdChequeStatus.AllowPaging = false;
            }
            else
            {
                grdChequeStatus.AllowPaging = true;
            }

            DataTable _dt = GetDataFromDB();

            grdChequeStatus.DataSource = _dt;
            grdChequeStatus.DataBind();

            ViewState["ChequeStatusReport"] = _dt;

        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

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


    public DataTable ReturnDataTable(string lvCommand, String lvConnectionString)
    {
        // This will return a System.Data.SQLClient.DataTable based on the command received
        // Will return null if an error occurs

        string dateFrom = "";
        string cardCode = "";
        string dateTo = "";
        string statmentDateFrom = "";
        string statmentDateTo = "";
        string receiveDateFrom = "";
        string receiveDateTo = "";
        
        string chequeNoFrom = "";
        decimal checksum = 0;
        string checkdateFrom = "";
        string checkdateTo = "";
        string chequeNoTo = "";

        string docNoFrom = "";
        string docNoTo = "";

        List<string> _dateFrom = new List<string>();
        List<string> _dateTo = new List<string>();
        List<string> _statmentDateFrom = new List<string>();
        List<string> _statmentDateTo = new List<string>();
        List<string> _receiveDateFrom = new List<string>();
        List<string> _receiveDateTo = new List<string>();
        List<string> _checkdateFrom = new List<string>();
        List<string> _checkdateTo = new List<string>();

        string company = ddlCompany.SelectedItem.Text == "---All---" ? "" : ddlCompany.SelectedItem.Text;
        string status = ddlStatus.SelectedValue == "All" ? "" : ddlStatus.SelectedValue;

        _dateFrom = txtStartDate.Text.Split('/').ToList();
        dateFrom = string.Join("/", _dateFrom[1], _dateFrom[0], _dateFrom[2]);

        _dateTo = txtEndDate.Text.Split('/').ToList();
        dateTo = string.Join("/", _dateTo[1], _dateTo[0], _dateTo[2]);

        if (txtSStatmentDate.Text != "")
        {
            _statmentDateFrom = txtSStatmentDate.Text.Split('/').ToList();
            statmentDateFrom = string.Join("/", _statmentDateFrom[1], _statmentDateFrom[0], _statmentDateFrom[2]);
        }
        else
        {
            _statmentDateFrom = DateTime.Now.AddYears(-10).ToString("dd/MM/yyyy").Split('/').ToList();
            statmentDateFrom = string.Join("/", _statmentDateFrom[1], _statmentDateFrom[0], _statmentDateFrom[2]);
        }

        if (txtEStatmentDate.Text != "")
        {
            _statmentDateTo = txtEStatmentDate.Text.Split('/').ToList();
            statmentDateTo = string.Join("/", _statmentDateTo[1], _statmentDateTo[0], _statmentDateTo[2]);
        }
        else
        {
            _statmentDateTo = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy").Split('/').ToList();
            statmentDateTo = string.Join("/", _statmentDateTo[1], _statmentDateTo[0], _statmentDateTo[2]);
        }

        if (txtSReceiveDate.Text != "")
        {
            _receiveDateFrom = txtSReceiveDate.Text.Split('/').ToList();
            receiveDateFrom = string.Join("/", _receiveDateFrom[1], _receiveDateFrom[0], _receiveDateFrom[2]);
        }

        if (txtEReceiveDate.Text != "")
        {
            _receiveDateTo = txtEReceiveDate.Text.Split('/').ToList();
            receiveDateTo = string.Join("/", _receiveDateTo[1], _receiveDateTo[0], _receiveDateTo[2]);
        }

        if (txtCheckDateFrom.Text != "")
        {
            _checkdateFrom = txtCheckDateFrom.Text.Split('/').ToList();
            checkdateFrom = string.Join("/", _checkdateFrom[1], _checkdateFrom[0], _checkdateFrom[2]);
        }
        else
        {
            _checkdateFrom = DateTime.Now.AddYears(-10).ToString("dd/MM/yyyy").Split('/').ToList();
            checkdateFrom = string.Join("/", _checkdateFrom[1], _checkdateFrom[0], _checkdateFrom[2]);
        }

        if (txtCheckDateTo.Text != "")
        {
            _checkdateTo = txtCheckDateTo.Text.Split('/').ToList();
            checkdateTo = string.Join("/", _checkdateTo[1], _checkdateTo[0], _checkdateTo[2]);
        }
        else
        {
            _checkdateTo = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy").Split('/').ToList();
            checkdateTo = string.Join("/", _checkdateTo[1], _checkdateTo[0], _checkdateTo[2]);
        }

        if (txtDocNo.Text != "")
        {
            docNoFrom = txtDocNo.Text;
        }
        if (txtDocNoTo.Text != "")
        {
            docNoTo = txtDocNoTo.Text;
        }

        if (txtCheque.Text != "")
        {
            chequeNoFrom = txtCheque.Text;
        }
        if (txtChequeTo.Text != "")
        {
            chequeNoTo = txtChequeTo.Text;
        }

        if (txtSearchCheckSum.Text != "")
        {
            checksum = Convert.ToDecimal(txtSearchCheckSum.Text);
        }

        if (txtCardCode.Text != "")
        {
            cardCode = txtCardCode.Text;
        }

        DataTable dtReturn = new DataTable("dtTemp");
        using (SqlConnection cn = new SqlConnection(lvConnectionString))
        {
            cn.Open();
            using (SqlCommand cmd = new SqlCommand("proc_ChequeStatusReport_GetData", cn))
            {

                //    if (excelFlag)
                //{
                //    cmd = new SqlCommand("proc_ChequeStatusReport_GenExcel", cn);
                //}
                //else
                //{
                //    cmd = new SqlCommand("proc_ChequeStatusReport_GetData", cn);
                //}

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Company", company));
                cmd.Parameters.Add(new SqlParameter("@Startdate", dateFrom));
                cmd.Parameters.Add(new SqlParameter("@EndDate", dateTo));
                cmd.Parameters.Add(new SqlParameter("@Status", status));
                cmd.Parameters.Add(new SqlParameter("@StmStartdate", statmentDateFrom));
                cmd.Parameters.Add(new SqlParameter("@StmEndDate", statmentDateTo));
                cmd.Parameters.Add(new SqlParameter("@RStartdate", receiveDateFrom));
                cmd.Parameters.Add(new SqlParameter("@REndDate", receiveDateTo));
                cmd.Parameters.Add(new SqlParameter("@docNoF", docNoFrom));
                cmd.Parameters.Add(new SqlParameter("@docNoT", docNoTo));
                cmd.Parameters.Add(new SqlParameter("@chequeNoF", chequeNoFrom));
                cmd.Parameters.Add(new SqlParameter("@chequeNoT", chequeNoTo));
                cmd.Parameters.Add(new SqlParameter("@checksum", checksum.ToString()));
                cmd.Parameters.Add(new SqlParameter("@CheckDateFrom", checkdateFrom));
                cmd.Parameters.Add(new SqlParameter("@CheckDateTo", checkdateTo));
                cmd.Parameters.Add(new SqlParameter("@CardCode", cardCode));
                cmd.CommandTimeout = 0;

                try
                {
                    //DataSet dsTemp = ReturnDataSet(cmd);
                    //dtReturn = dsTemp.Tables[0];
                    SqlDataReader reader = cmd.ExecuteReader();

                    dtReturn.Load(reader);
                    cn.Close();

                    //DataTable _dt = new DataTable("SdtTemp");
                    //_dt = dtReturn.Clone();
                    //_dt.Clear();

                    //DataRow[] filteredRows = null;

                    //if (!string.IsNullOrEmpty(chequeNoFrom) && !string.IsNullOrEmpty(chequeNoTo))
                    //{
                    //    filteredRows = dtReturn.Select(string.Format("{0} >= '{1}' AND {0} <= '{2}'", "ChequeNoSAP", chequeNoFrom, chequeNoTo));
                    //    if (filteredRows != null)
                    //    {
                    //        AddDataTableRow(_dt, ref filteredRows);
                    //    }
                    //}
                    //else if (!string.IsNullOrEmpty(chequeNoFrom))
                    //{
                    //    filteredRows = dtReturn.Select(string.Format("{0} LIKE '{1}%'", "ChequeNoSAP", chequeNoFrom));
                    //    if (filteredRows != null)
                    //    {
                    //        AddDataTableRow(_dt, ref filteredRows);
                    //    }
                    //}
                    //else if (!string.IsNullOrEmpty(chequeNoTo))
                    //{
                    //    filteredRows = dtReturn.Select(string.Format("{0} LIKE '{1}%'", "ChequeNoSAP", chequeNoTo));
                    //    if (filteredRows != null)
                    //    {
                    //        AddDataTableRow(_dt, ref filteredRows);
                    //    }
                    //}

                    //if (_dt != null && _dt.Rows.Count > 0)
                    //{
                    //    dtReturn = _dt;
                    //}
                    //dtReturn = ReturnDataTable(cmd);
                }
                catch (Exception ex)
                {
                    cn.Close();
                    dtReturn = null;
                    throw ex;
                }
            }
        }


        return dtReturn;
    }

    public void AddDataTableRow(DataTable _dt, ref DataRow[] filteredRows)
    {
        foreach (var row in filteredRows)
        {
            List<object> data = new List<object>();
            for (int i = 0; i < row.ItemArray.Count(); i++)
            {
                data.Add(row[i]);
            }
            _dt.Rows.Add(data.ToArray());
        }
    }

    public DataSet ReturnDataSet(SqlCommand lvSQLCommand)
    {
        DataSet dsTemp = new DataSet("dsTemp");
        SqlDataAdapter daTemp = new SqlDataAdapter(lvSQLCommand);

        try
        {
            daTemp.Fill(dsTemp);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dsTemp;
    }

    public DataTable ReturnDataTable(SqlCommand lvSQLCommand)
    {
        DataTable dataTable = new DataTable("dtTemp");
        try
        {
            SqlDataReader reader = lvSQLCommand.ExecuteReader();

            dataTable.Load(reader);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataTable;
    }

    private DataTable GetDataFromDB(bool excelFlag = false)
    {
        DataTable _dt = new DataTable();

        try
        {
            if (chkShowAll.Checked)
            {
                grdChequeStatus.AllowPaging = false;
            }
            else
            {
                grdChequeStatus.AllowPaging = true;
            }

            _dt = ReturnDataTable("", connStrng);

            DataView dv = new DataView(_dt);
            string _sqlWhere = "";

            string statmentStatus = ddlStatement.SelectedValue;
            if (statmentStatus == "Y")
            {
                _sqlWhere = excelFlag ? "Statement IS NOT NULL" : "StatementDate IS NOT NULL";

                dv.RowFilter = _sqlWhere;
            }
            else if (statmentStatus == "N")
            {
                _sqlWhere = excelFlag ? "Statement IS NULL" : "StatementDate IS NULL";

                dv.RowFilter = _sqlWhere;
            }

            DataTable _newDataTable = new DataTable();
            _newDataTable = dv.ToTable();

            //ViewState["ChequeDataForExcel"] = _newDataTable;

            return _newDataTable;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        InitialSAPData();

        BindGridView();

        if (grdChequeStatus.DataSource != null && grdChequeStatus.Rows.Count > 0)
        {
            //linkSave.Visible = true;
            linkExportReport.Visible = true;
        }
    }

    private void InitialSAPData()
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();
                SqlCommand cmd = null;
                cmd = new SqlCommand("proc_ChequeStatusReport_InitailData", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                cn.Close();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdChequeStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdChequeStatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridViewSortExpression = e.SortExpression;
        int pageIndex = grdChequeStatus.PageIndex;
        grdChequeStatus.DataSource = SortDataTable((DataTable)ViewState["ChequeStatusReport"], false);
        grdChequeStatus.DataBind();
        for (int i = 1; i < grdChequeStatus.Columns.Count - 1; i++)
        {
            string ht = ((LinkButton)grdChequeStatus.HeaderRow.Cells[i].Controls[0]).Text;
            if (ht == e.SortExpression)
            {
                TableCell tableCell = grdChequeStatus.HeaderRow.Cells[i];
                Image img = new Image();
                img.ImageUrl = (GridViewSortDirection == "ASC") ? "~/Images/asc.gif" : "~/Images/desc.gif";
                tableCell.Controls.Add(img);
            }
        }

        grdChequeStatus.PageIndex = pageIndex;
    }

    protected void grdChequeStatus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = 1;
            foreach (TableCell tc in e.Row.Cells)
            {
                if (tc.HasControls() && colIndex > 1)
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
                colIndex++;
            }
        }
    }

    protected void grdChequeStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdChequeStatus.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    //protected void linkSave_Click(object sender, EventArgs e)
    private void SaveData()
    {
        //string confirmValue = Request.Form["confirm_value"];
        //if (confirmValue == "Yes")
        //{
        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);

        try
        {
            using (SqlConnection dataConnection = new SqlConnection(connStrng))
            {
                using (SqlCommand dataCommand = dataConnection.CreateCommand())
                {
                    dataConnection.Open();
                    foreach (GridViewRow row in grdChequeStatus.Rows)
                    {
                        string chequeStatus = "";

                        CheckBox _chkChequeStatus = (row.FindControl("chkChequeStatus") as CheckBox);
                        string u_chequestatus = (row.FindControl("lblU_chequestatus") as Label).Text;
                        if (_chkChequeStatus != null && _chkChequeStatus is CheckBox)
                        {
                            chequeStatus = _chkChequeStatus.Checked == true ? "Y" : "N"; //(u_chequestatus == "Not Receive" ? "N" : "");
                        }

                        string chequeNo = "";
                        TextBox _txtChequeNo = (row.FindControl("txtChequeNo") as TextBox);
                        string _chequeNoSAP = (row.FindControl("lblChequeNoSAP") as Label).Text;
                        if (_txtChequeNo != null && _txtChequeNo is TextBox)
                        {
                            chequeNo = _txtChequeNo.Text; //_chequeNoSAP == _txtChequeNo.Text ? "" : _txtChequeNo.Text;
                        }

                        List<string> _statementDate = new List<string>();
                        List<string> _receiveDate = new List<string>();

                        string receiveDate = "";
                        TextBox _txtReceiveDate = (row.FindControl("txtReceiveDate") as TextBox);
                        if (_txtReceiveDate != null && _txtReceiveDate is TextBox)
                        {
                            if (_txtReceiveDate.Text != "")
                            {
                                _receiveDate = _txtReceiveDate.Text.Split('/').ToList();
                                receiveDate = string.Join("", _receiveDate[2], _receiveDate[1], _receiveDate[0]);
                            }
                        }

                        string statementDate = "";
                        TextBox _txtStatementDate = (row.FindControl("txtStatementDate") as TextBox);
                        if (_txtStatementDate != null && _txtStatementDate is TextBox)
                        {
                            if (_txtStatementDate.Text != "")
                            {
                                _statementDate = _txtStatementDate.Text.Split('/').ToList();
                                statementDate = string.Join("", _statementDate[2], _statementDate[1], _statementDate[0]);
                            }
                        }

                        string docNo = (row.FindControl("lblDocNo") as Label).Text;
                        string company = (row.FindControl("lblCompany") as Label).Text;

                        string query = "";
                        query += "IF NOT EXISTS(SELECT 1 FROM [dbo].[tbl_JEEntry] WHERE [DocNo] = '" + docNo + "' AND Company = '" + company + "') ";
                        query += "BEGIN ";
                        query += "  INSERT INTO [dbo].[tbl_JEEntry] ";
                        query += "  ([DocNo] ";
                        query += "  ,[Status] ";
                        query += "  ,[ChequeNo] ";
                        query += "  ,[StatusDate] ";
                        query += "  ,[ChequeModifyDate] ";
                        query += "  ,[Company] ";
                        query += "  ,StatementDate ";
                        query += "  ,ReceiveDate) ";
                        query += "  VALUES ";
                        query += "  ('" + docNo + "' ";
                        query += "  , IIF('" + chequeStatus + "' = '', NULL, '" + chequeStatus + "') ";
                        query += "  , '" + chequeNo + "' ";
                        if (!string.IsNullOrEmpty(chequeStatus))
                        {
                            query += "  , case when EXISTS(SELECT 1 FROM [dbo].tbl_JEEntry_Temp WHERE [DocNo] = '" + docNo + "' AND Company = '" + company + "' AND ISNULL(Status, 'N') = '" + chequeStatus + "') THEN NULL ELSE GETDATE() END";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(statementDate) || !string.IsNullOrEmpty(receiveDate))
                            {
                                query += "  , GETDATE() ";
                            }
                            else
                            {
                                query += "  , NULL ";
                            }
                        }

                        if (!string.IsNullOrEmpty(chequeNo))
                        {
                            query += "  , case when EXISTS(SELECT 1 FROM [dbo].tbl_JEEntry_Temp WHERE [DocNo] = '" + docNo + "' AND Company = '" + company + "' AND ChequeNo = '" + chequeNo + "') THEN NULL ELSE GETDATE() END";
                        }
                        else
                        {
                            query += "  , NULL ";
                        }

                        query += "  , '" + company + "' ";
                        if (string.IsNullOrEmpty(statementDate))
                        {
                            query += "  , NULL ";
                        }
                        else
                        {
                            query += "  , '" + statementDate + "' ";
                        }

                        if (string.IsNullOrEmpty(receiveDate))
                        {
                            query += "  , NULL )";
                        }
                        else
                        {
                            query += "  , '" + receiveDate + "') ";
                        }
                        query += "END ";
                        query += "ELSE ";
                        query += "  BEGIN ";
                        query += "  IF EXISTS(SELECT 1 FROM [dbo].[tbl_JEEntry] WHERE [DocNo] = '" + docNo + "' AND Company = '" + company + "') ";
                        query += "  BEGIN ";
                        query += "      UPDATE [dbo].[tbl_JEEntry] ";
                        query += "      SET [ChequeNo] = IIF('" + chequeNo + "' = '', NULL, '" + chequeNo + "') ";
                        query += "         ,[ChequeModifyDate] = GETDATE() ";
                        query += "         ,[Status] = IIF('" + chequeStatus + "' = '', NULL, '" + chequeStatus + "') ";
                        query += "	       ,[StatusDate] = GETDATE() ";

                        if (string.IsNullOrEmpty(statementDate))
                        {
                            query += "	       ,[StatementDate] = NULL";
                        }
                        else
                        {
                            query += "	       ,[StatementDate] = '" + statementDate + "' ";
                        }

                        if (string.IsNullOrEmpty(receiveDate))
                        {
                            query += "	       ,[ReceiveDate] = NULL";
                        }
                        else
                        {
                            query += "	       ,[ReceiveDate] =  '" + receiveDate + "' ";
                        }

                        query += "      WHERE [DocNo] = '" + docNo + "' AND Company = '" + company + "' ";
                        query += "  END ";
                        query += "END ";

                        dataCommand.CommandType = CommandType.Text;
                        dataCommand.CommandTimeout = 0;
                        dataCommand.CommandText = query;
                        dataCommand.ExecuteNonQuery();

                    }
                    dataConnection.Close();
                }
            }

            BindGridView();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showValidateMsg()", true);
        }
        catch (SqlException sqlEx)
        {
            Helper.WriteLog(sqlEx.Message);
            throw sqlEx;
        }

        //}

    }

    protected void linkExportReport_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand("proc_ChequeStatusReport_GenExcel_R2", cn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ChequeExcel");

                DataTable _dt = ds.Tables["ChequeExcel"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "ChequeExcel");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ChequeStatusReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.Close();
                        //Response.End();
                    }
                }

                cn.Close();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }

        //try
        //{
        //    DataTable _dt = GetDataFromDB(true);


        //    if (_dt != null && _dt.Rows.Count > 0)
        //    {
        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(_dt, "ChequeExcel");

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;filename=ChequeStatusReport.xlsx");
        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                wb.SaveAs(MyMemoryStream);
        //                MyMemoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }

        //        //using (XLWorkbook wb = new XLWorkbook())
        //        //{
        //        //    wb.Worksheets.Add(_dt, "ChequeStatus");

        //        //    Response.Clear();
        //        //    Response.Buffer = true;
        //        //    Response.Charset = "";
        //        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        //    Response.AddHeader("content-disposition", "attachment;filename=ChequeStatusReport.xlsx");
        //        //    using (MemoryStream MyMemoryStream = new MemoryStream())
        //        //    {
        //        //        wb.SaveAs(MyMemoryStream);
        //        //        MyMemoryStream.WriteTo(Response.OutputStream);
        //        //        Response.Flush();
        //        //        Response.Close();
        //        //    }
        //        //}
        //    }
        //}
        //catch (Exception ex)
        //{

        //    throw ex;
        //}
    }

    protected void grdChequeStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //e.Row.Attributes.Add("style", "cursor:help;");
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    e.Row.BackColor = System.Drawing.Color.FromName("#C2D69B");
                }
            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("white");
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");

                e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

                CheckBox chk = (CheckBox)e.Row.FindControl("chkChequeStatus");
                HiddenField lblSatatus = (HiddenField)e.Row.FindControl("hfChequeStatus");

                if (lblSatatus.Value.ToString() == "Y")
                {
                    chk.Checked = true;
                    e.Row.BackColor = System.Drawing.Color.FromName("aqua");
                }
                else
                {
                    chk.Checked = false;
                }

            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

}