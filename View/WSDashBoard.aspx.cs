using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_WSDashBoard : System.Web.UI.Page
{
    static string Condition = "";
    DashBoard d = new DashBoard();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Condition = "gamt";

            InitPage();

            BindGridView(grdDash1);
            BindGridView(grdData, ddlDailyMonth, ddlDailyYear);
            BindGridView(grdData2, ddlDailyMonth2, ddlDailyYear2);

            divHDaily1.Visible = false;
            divHDaily2.Visible = false;

            //ALLC
            //BDT
            //EX1
            //EX2
            //MT
            //WS
        }
    }

    private void InitPage()
    {
        Dictionary<string, string> listValueType = new Dictionary<string, string>();
        listValueType.Add("gamt", "Gross Amount");
        listValueType.Add("namt", "Net Amount");
        listValueType.Add("qty", "Quantity");
        BindDropdownList(ddlValueType, listValueType);

        var _date = DateTime.Now;
        Dictionary<string, string> monthList = new Dictionary<string, string>();
        for (int i = 1; i <= 12; i++)
        {
            var _month = new DateTime(_date.Year, i, 1);
            monthList.Add(i.ToString(), _month.ToString("MMMM"));
        }
        BindDropdownList(ddlMonth, monthList);
        ddlMonth.SelectedValue = _date.Month.ToString();

        Dictionary<string, string> yearList = new Dictionary<string, string>();
        for (int i = 2010; i <= _date.Year; i++)
        {
            yearList.Add(i.ToString(), i.ToString());
        }
        BindDropdownList(ddlYear, yearList);
        ddlYear.SelectedValue = _date.Year.ToString();

        var _date_b = DateTime.Now.AddMonths(-1);
        BindDropdownList(ddlDailyMonth, monthList);
        ddlDailyMonth.SelectedValue = _date_b.Month.ToString();

        BindDropdownList(ddlDailyYear, yearList);
        ddlDailyYear.SelectedValue = _date_b.Year.ToString();


        BindDropdownList(ddlDailyMonth2, monthList);
        ddlDailyMonth2.SelectedValue = _date.Month.ToString();

        BindDropdownList(ddlDailyYear2, yearList);
        ddlDailyYear2.SelectedValue = _date.Year.ToString();

        Dictionary<string, string> periodList = new Dictionary<string, string>();
        periodList.Add("1-10", "1-10");
        periodList.Add("11-20", "11-20");
        periodList.Add("21-31", "21-31");
        BindDropdownList(ddlPeriod, periodList);

        if (_date.Day <= 10)
            ddlPeriod.SelectedValue = "1-10";
        if (_date.Day > 10 && _date.Day <= 20)
            ddlPeriod.SelectedValue = "11-20";
        if (_date.Day > 20 && _date.Day <= 31)
            ddlPeriod.SelectedValue = "21-31";
    }

    private void BindDropdownList(DropDownList ddl, Dictionary<string, string> data)
    {
        ddl.DataSource = data;
        ddl.DataTextField = "value";
        ddl.DataValueField = "key";
        ddl.DataBind();
    }

    private void BindGridView(GridView grd)
    {
        DataTable dt = d.LoadData(ddlMonth, ddlYear, ddlValueType, ddlPeriod, "WS", true);
        d.GetHeader(lblTitle, ddlMonth, ddlYear, ddlValueType, ddlPeriod, "WS");

        if (dt.Rows.Count > 0)
        {
            DataRow totalRow = dt.NewRow();
            //totalRow["#"] = dt.Rows.Count + 1;
            totalRow["Sales Area"] = "TOTAL";

            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "#" && column.ColumnName != "Sales Area")
                {
                    var _col_total = 0;
                    foreach (var x in dt.AsEnumerable())
                    {
                        var _temp_data = x.Field<string>(column.ColumnName);
                        var _data = !string.IsNullOrEmpty(_temp_data) ? Convert.ToInt32(_temp_data.ToString().Replace(",", "")) : 0;
                        _col_total += _data;
                    }

                    totalRow[column.ColumnName] = _col_total.ToString("#,##0");
                }
            }

            dt.Rows.InsertAt(totalRow, dt.Rows.Count);
        }


        if (dt.Rows.Count > 0)
            grd.DataSource = dt;
        else
            grd.DataSource = null;

        grd.DataBind();
    }

    private void BindGridView(GridView grd, DropDownList _ddlMonth, DropDownList _ddlYear)
    {
        DataTable dt = d.GetDailyData(_ddlMonth, _ddlYear, null);

        if (dt.Rows.Count > 0)
            grd.DataSource = dt;
        else
            grd.DataSource = null;

        grd.DataBind();

        if (grd.ID == "grdData")
        {
            lblDaily1.Text = _ddlMonth.SelectedItem.Text + " / " + _ddlYear.SelectedValue.ToString();
        }
        else
        {
            lblDaily2.Text = _ddlMonth.SelectedItem.Text + " / " + _ddlYear.SelectedValue.ToString();
        }
    }

    protected void grdDash1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j == 0)
            {
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.Text = cell.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }
            else
                cell.HorizontalAlign = HorizontalAlign.Right;

            if (j >= 1 && j <= 3)
                cell.BackColor = ColorTranslator.FromHtml("#A9D08E");

            if (string.IsNullOrEmpty(cell.Text) || cell.Text == "0" || cell.Text == "&nbsp;")
            {
                cell.Text = "-";
                cell.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            }

            if (j == 0)
            {
                cell.Width = 120;
            }
            else if (j > 0 && j <= 3)
            {
                cell.Width = 60;
            }
            else
            {
                cell.Width = 55;
            }
        }

        if (e.Row.RowIndex == dt.Rows.Count - 1)
        {
            e.Row.Font.Bold = true;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        BindGridView(grdDash1);
        BindGridView(grdData, ddlDailyMonth, ddlDailyYear);
        BindGridView(grdData2, ddlDailyMonth2, ddlDailyYear2);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGridView(grdData, ddlDailyMonth, ddlDailyYear);
    }

    protected void btnSearch2_Click(object sender, EventArgs e)
    {
        BindGridView(grdData2, ddlDailyMonth2, ddlDailyYear2);
    }

    protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e);
    }

    protected void grdData2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e);
    }

    private void SetRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j <= 3)
                cell.HorizontalAlign = HorizontalAlign.Left;
            else
                cell.HorizontalAlign = HorizontalAlign.Right;

            if (string.IsNullOrEmpty(cell.Text) || cell.Text == "0.00" || cell.Text == "&nbsp;")
            {
                cell.Text = "-";
                cell.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            }

            if (j == 0)
            {
                cell.Width = 20;
            }
            else if (j == 1)
            {
                cell.Width = 80;
            }
            else if (j == 2)
            {
                cell.Width = 80;
            }
            else if (j == 3)
            {
                cell.Width = 120;
            }
            else
            {
                cell.Width = 55;
            }
        }
    }
}