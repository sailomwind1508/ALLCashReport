using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_BDTDashBoard : System.Web.UI.Page
{
    static string Condition = "";
    DashBoard d = new DashBoard();
    public static string Channel { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Condition = "gamt";

            InitPage();

            BindGridView(grdDash1, lblTitleBDT, "BDT");
            BindGridView(grdDash2, lblTitleMT, "MT");
            BindGridView(grdDash3, lblTitleEX1, "EX1");
            BindGridView(grdDash4, lblTitleEX2, "EX2");

            //BindGridView(grdDash2, "EX1", "gamt");

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

    private void BindGridView(GridView grd, Literal lbl, string channel)
    {
        DataTable dt = d.LoadData(ddlMonth, ddlYear, ddlValueType, ddlPeriod, channel, true);
        d.GetHeader(lbl, ddlMonth, ddlYear, ddlValueType, ddlPeriod, channel);

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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        BindGridView(grdDash1, lblTitleBDT, "BDT");
        BindGridView(grdDash2, lblTitleMT, "MT");
        BindGridView(grdDash3, lblTitleEX1, "EX1");
        BindGridView(grdDash4, lblTitleEX2, "EX2");
    }

    protected void grdDash1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e, "#ED7D31");
    }

    protected void grdDash2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e, "#FFC000");
    }

    protected void grdDash3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e, "#00B0F0");
    }

    protected void grdDash4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SetRowDataBound(sender, e, "#99FF99");
    }

    private void SetRowDataBound(object sender, GridViewRowEventArgs e, string colorCode)
    {
        DataTable dt = ((DataTable)((BaseDataBoundControl)sender).DataSource);

        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j == 0)
                cell.HorizontalAlign = HorizontalAlign.Left;
            else
                cell.HorizontalAlign = HorizontalAlign.Right;

            if (j >= 1 && j <= 3)
            {
                cell.BackColor = ColorTranslator.FromHtml(colorCode);
            }

            if (string.IsNullOrEmpty(cell.Text) || cell.Text == "0" || cell.Text == "&nbsp;")
            {
                cell.Text = "-";
                cell.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            }

            //if (j == 0)
            //{
            //    cell.Width = 120;
            //}
            //else if (j > 0 && j <= 3)
            //{
            //    cell.Width = 60;
            //}
            //else
            {
                cell.Width = 60;
            }
        }

        if (e.Row.RowIndex == dt.Rows.Count - 1)
        {
            e.Row.Font.Bold = true;
        }
    }
}