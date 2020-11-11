using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test5 : System.Web.UI.Page
{
    DashBoard d = new DashBoard();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            InitPage();

            BindGridView(grdData, ddlMonth, ddlYear);
            BindGridView(grdData2, ddlMonth2, ddlYear2);
        }

        var requestTarget = this.Request["__EVENTTARGET"];
        var requestArgs = this.Request["__EVENTARGUMENT"];
        if (requestTarget == "save")
        {
            SaveData();
        }
        else if (requestTarget == "remove")
        {
            RemoveData();
        }
    }

    private void InitPage()
    {
        Dictionary<string, string> listOrder = new Dictionary<string, string>();
        listOrder.Add("oil", "น้ำมัน");
        listOrder.Add("plastic", "พลาสติก");
        listOrder.Add("gold", "ทองคำ");
        BindDropdownList(ddlOrder, listOrder);

        Dictionary<string, string> listType = new Dictionary<string, string>();
        listType.Add("1", "แท่ง 96.5%");
        listType.Add("2", "WTI Crude");
        listType.Add("3", "BENT Crude");
        listType.Add("4", "PET");
        listType.Add("5", "PP INJ");
        BindDropdownList(ddlType, listType);

        Dictionary<string, string> listRemark = new Dictionary<string, string>();
        listRemark.Add("sold", "ราคาขายออก(บาท/บาท)");
        listRemark.Add("barrel", "ดอลล่าร์/บาร์เรล");
        listRemark.Add("kg", "กิโลกรัม/บาท");
        BindDropdownList(ddlRemark, listRemark);

        var _date = DateTime.Now;

        Dictionary<string, string> monthList = new Dictionary<string, string>();
        for (int i = 1; i <= 12; i++)
        {
            var _month = new DateTime(_date.Year, i, 1);
            monthList.Add(i.ToString(), _month.ToString("MMMM"));
        }
        BindDropdownList(ddlMonth, monthList);
        ddlMonth.SelectedValue = _date.AddMonths(-1).Month.ToString();

        Dictionary<string, string> yearList = new Dictionary<string, string>();
        for (int i = 2010; i <= _date.Year; i++)
        {
            yearList.Add(i.ToString(), i.ToString());
        }
        BindDropdownList(ddlYear, yearList);
        ddlYear.SelectedValue = _date.AddMonths(-1).Year.ToString();


        BindDropdownList(ddlMonth2, monthList);
        ddlMonth2.SelectedValue = _date.Month.ToString();

        BindDropdownList(ddlYear2, yearList);
        ddlYear2.SelectedValue = _date.Year.ToString();

    }

    private void BindDropdownList(DropDownList ddl, Dictionary<string, string> data)
    {
        ddl.DataSource = data;
        ddl.DataTextField = "value";
        ddl.DataValueField = "key";
        ddl.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {


    }

    private void SaveData()
    {
        try
        {
            string date = "";

            var _date = txtDate.Text.Split('/').ToList();
            date = string.Join("/", _date[1], _date[0], _date[2]);

            DashBoardModel data = new DashBoardModel();
            data.CDate = Convert.ToDateTime(date);
            data.Order = ddlOrder.SelectedItem.Text;
            data.Type = ddlType.SelectedItem.Text;
            data.Price = Convert.ToDecimal(txtPrice.Text);
            data.Remark = ddlRemark.SelectedItem.Text;
            d.UpdateData(data);

            BindGridView(grdData, ddlMonth, ddlYear);
            BindGridView(grdData2, ddlMonth2, ddlYear2);

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showSaveResultMsg()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "alert('Save Fail!!! Please try agian.');", true);
        }
    }

    private void BindGridView(GridView grd, DropDownList _ddlMonth, DropDownList _ddlYear)
    {
        DataTable dt = d.GetDailyData(_ddlMonth, _ddlYear);

        if (dt.Rows.Count > 0)
            grd.DataSource = dt;
        else
            grd.DataSource = null;

        grd.DataBind();

        if (grd.ID == "grdData")
        {
            lblTitle.Text = _ddlMonth.SelectedItem.Text + " / " + _ddlYear.SelectedValue.ToString();
        }
        else
        {
            lblTitle2.Text = _ddlMonth.SelectedItem.Text + " / " + _ddlYear.SelectedValue.ToString();
        }
    }

    protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int j = 0; j < e.Row.Cells.Count; j++)
        {
            var cell = e.Row.Cells[j];
            if (j <= 3)
                cell.HorizontalAlign = HorizontalAlign.Left;
            else
                cell.HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGridView(grdData, ddlMonth, ddlYear);
    }

    private void RemoveData()
    {
        try
        {
            string date = "";

            var _date = txtDate.Text.Split('/').ToList();
            date = string.Join("/", _date[1], _date[0], _date[2]);
            string _order = ddlOrder.SelectedItem.Text;
            string _type = ddlType.SelectedItem.Text;

            DashBoardModel data = new DashBoardModel();
            data.CDate = Convert.ToDateTime(date);

            int day = Convert.ToInt32(_date[0]);
            int month = Convert.ToInt32(_date[1]);
            int year = Convert.ToInt32(_date[2]);
            d.RemoveData(day, month, year, _order, _type);

            BindGridView(grdData, ddlMonth, ddlYear);
            BindGridView(grdData2, ddlMonth2, ddlYear2);

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showRemoveResultMsg()", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "alert('Remove Fail!!! Please try agian.');", true);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {

    }

    protected void btnSearch2_Click(object sender, EventArgs e)
    {
        BindGridView(grdData2, ddlMonth2, ddlYear2);
    }
}