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
using System.Web.UI.HtmlControls;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Runtime.CompilerServices;

public partial class TransportationPlan : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    string conString = ConfigurationManager.ConnectionStrings["myConnectionString_LogisticUni"].ConnectionString;
    
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

            //ClearForm();

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
                //Save();
            }
            else if (requestTarget == "remove")
            {
                if (!string.IsNullOrEmpty(pk))
                {
                    //Remove(pk);
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
        Dictionary<string, string> routeSeltList = new Dictionary<string, string>();

        for (int i = 1; i <= 30; i++)
        {
            routeSeltList.Add(i.ToString(), "แสดง " + i.ToString() + " Route");
        }
        ddlRouteAmt.BindDropdownList(routeSeltList);

        DataTable route_DT = Helper.ExecuteProcedureToTable(conString, "proc_TransportationPlan_RouteMaster_Get");

        //DataRow cRow = data_DT.NewRow();
        //cRow["RouteNo"] = "30";
        //cRow["RouteName"] = "---All---";
        //data_DT.Rows.InsertAt(cRow, 0);

        if (route_DT != null && route_DT.Rows.Count > 0)
            Session["RouteList"] = route_DT;
        else
            Session["RouteList"] = null;

        DataTable docNo_DT = Helper.ExecuteProcedureToTable(conString, "proc_TransportationPlan_DocNo_Get");
        if (docNo_DT != null && docNo_DT.Rows.Count > 0)
            Session["DocNoList"] = docNo_DT;
        else
            Session["DocNoList"] = null;

        DataTable truckNo_DT = Helper.ExecuteProcedureToTable(conString, "proc_TransportationPlan_TruckMaster_Get");
        DataRow cRow = truckNo_DT.NewRow();
        cRow["TruckNo"] = "-1";
        cRow["LicencepleteNo"] = "---เลือกหมายเลขรถ---";
        truckNo_DT.Rows.InsertAt(cRow, 0);

        if (truckNo_DT != null && truckNo_DT.Rows.Count > 0)
            Session["TruckNoList"] = truckNo_DT;
        else
            Session["TruckNoList"] = null;

        var ddl = ddlRouteAmt;
        int routeAmt = Convert.ToInt32(ddl.SelectedValue);
        GenerateRouteTable(routeAmt);
    }

    private void GenerateRouteTable(int routeAmt)
    {
        HtmlTable tbl = tblLayout;
        //tbl.Border = 1;
        HtmlTableRow tRow = new HtmlTableRow();
        tRow.Height = "550px";//.Style.Add(HtmlTextWriterStyle.Height, "530px");
        List<string> listGrd = new List<string>();
        for (int i = 1; i <= routeAmt; i++)
        {
            if (i % 2 != 0)
            {
                tRow = new HtmlTableRow();
                tRow.Style.Add(HtmlTextWriterStyle.Height, "530px");
            }

            HtmlTableCell tCell = new HtmlTableCell();
            //tCell.Style.Value = "width: 50%;";
            tCell.Width = "45%";
            tCell.Height = "550px";
            tCell.InnerText = "";

            HtmlTableCell tCell2 = new HtmlTableCell();
            //tCell.Style.Value = "width: 50%;";
            tCell2.Width = "5%";
            tCell2.Height = "550px";
            tCell2.InnerText = "___";
            tCell2.Style.Value = "color:white;";

            HtmlGenericControl newDiv1 = new HtmlGenericControl("DIV");
            newDiv1.Style.Value = "width: 100%; font-size: 14px;height:30px;";
            Label lblTruckNoTxt = new Label();
            lblTruckNoTxt.Text = "หมายเลขรถ : ";
            lblTruckNoTxt.CssClass = "font-size: 14px;";

            newDiv1.Controls.Add(lblTruckNoTxt);

            DropDownList ddlTruckNo = new DropDownList();
            ddlTruckNo.ID = "ddlTruckNo" + i.ToString();
            ddlTruckNo.AutoPostBack = true;
            ddlTruckNo.ViewStateMode = ViewStateMode.Enabled;
            ddlTruckNo.EnableViewState = true;
            ddlTruckNo.CssClass = "font-size: 14px;";
            ddlTruckNo.SelectedIndexChanged += DdlTruckNo_SelectedIndexChanged1;
            ddlTruckNo.BindDropdownList((DataTable)Session["TruckNoList"], "LicencepleteNo", "TruckNo");

            newDiv1.Controls.Add(ddlTruckNo);

            HtmlGenericControl newDiv2 = new HtmlGenericControl("DIV");
            newDiv2.Style.Value = "width: 100%; font-size: 12px; height:500px; overflow-y: auto;";

            

            GridView grd = new GridView();
            grd = CreateGridView(i.ToString());

            listGrd.Add(grd.ID);

            grd.RowDataBound += Grd_RowDataBound;

            GetRouteData(grd, i.ToString());

            if (grd.DataSource != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)grd.DataSource;
                if (dt != null && dt.Rows.Count > 0)
                {
                    string _truckNo = dt.Rows[1]["TruckNo"].ToString();
                    if (!string.IsNullOrEmpty(_truckNo))
                    {
                        _truckNo = _truckNo.Split(':')[0];
                        ddlTruckNo.SelectedValue = _truckNo;
                    }
                    else
                        ddlTruckNo.SelectedValue = "-1";
                }
            }

            newDiv2.Controls.Add(grd);

            tCell.Controls.Add(newDiv1);
            tCell.Controls.Add(newDiv2);

            tRow.Controls.Add(tCell);
            tRow.Controls.Add(tCell2);

            tbl.Rows.Add(tRow);

        }

        Session["GridViewNameList"] = listGrd;
    }

    private void DdlTruckNo_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (sender is DropDownList)
        {
            var ddl = sender as DropDownList;
            string truckNo = "";
            truckNo = ddl.SelectedValue;
            if (truckNo != "-1")
            {
                PrepareAllGridView(ddlRouteAmt);

                int r = Convert.ToInt32(ddlRouteAmt.SelectedValue);
                for (int i = 1; i <= r; i++)
                {
                    string ctrlName = "grdR" + i.ToString();
                    GridView grd = (this.FindControl(ctrlName) as GridView);
                    if (grd != null && grd.DataSource != null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)grd.DataSource;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {

                            }
                        }
                    }
                }
                
            }
        }
    }

    private void PrepareAllGridView(DropDownList ddl)
    {
        var ddlRouteAmt = ddl;
        int routeAmt = Convert.ToInt32(ddlRouteAmt.SelectedValue);
        GenerateRouteTable(routeAmt);
    }

    private void GetRouteData(GridView grd, string routeID)
    {
        Dictionary<string, object> prmt = new Dictionary<string, object>();
        if (Session["RouteList"] != null)
        {
            var dt = (DataTable)Session["RouteList"];
            foreach (DataRow item in dt.Rows)
            {
                string _routeNo = item["RouteNo"].ToString();
                int routeNo = Convert.ToInt32(_routeNo.Substring(8, 2));
                if (routeNo.ToString() == routeID)
                {
                    prmt.Add("RouteNo", _routeNo);
                    break;
                }
            }

            DataTable data_DT = new DataTable(grd.ID);
            data_DT = Helper.ExecuteProcedureToTable(conString, "proc_TransportationPlan_Transport_Get", prmt);
            if (data_DT != null && data_DT.Rows.Count > 0)
            {
                BindGridView(grd, data_DT);
                ViewState[grd.ID] = data_DT;
            }
            else
            {
                BindGridView(grd, null);
                ViewState[grd.ID] = null;
            }
        }
        else
        {
            BindGridView(grd, null);
            ViewState[grd.ID] = null;
        }
    }


    private void BindGridView(GridView grd, DataTable data)
    {
        try
        {
            grd.DataSource = data;
            grd.DataBind();

            //DataTable dt = new DataTable();
            //dt.Columns.Add("RowNo");
            //dt.Columns.Add("RouteID");
            //dt.Columns.Add("CardName");
            //dt.Columns.Add("TransportDate");
            //dt.Columns.Add("Quantity");
            //dt.Columns.Add("VolumeFG");
            //dt.Columns.Add("CANCELED");

            //for (int j = 0; j < data.Rows.Count; j++)
            //{
            //    var cell = data.Rows[j];
            //    DataRow newRow = dt.NewRow();
            //    dt.Rows.InsertAt(newRow, j);

            //    string _RowNo = cell["RowNo"].ToString();
            //    string _RouteID = cell["RouteID"].ToString();
            //    string _TransportDate = cell["TransportDate"].ToString();
            //    string _CardName = cell["CardName"].ToString();
            //    string _Quantity = cell["Quantity"].ToString();
            //    string _VolumeFG = cell["VolumeFG"].ToString();
            //    string _CANCELED = cell["CANCELED"].ToString();

            //    dt.Rows[j]["RowNo"] = _RowNo;
            //    dt.Rows[j]["RouteID"] = _RouteID;
            //    dt.Rows[j]["TransportDate"] = _TransportDate;
            //    dt.Rows[j]["CardName"] = _CardName;

            //    dt.Rows[j]["Quantity"] = _Quantity;
            //    dt.Rows[j]["VolumeFG"] = _VolumeFG;
            //    dt.Rows[j]["CANCELED"] = _CANCELED;
            //}
            //grd.DataSource = dt;
            //grd.DataBind();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private GridView CreateGridView(string id)
    {
        GridView grd = new GridView();
        grd.ID = "grdR" + id;
        grd.AutoGenerateColumns = false;
        grd.CssClass = "mydatagrid";
        grd.HeaderStyle.CssClass = "header_grid";
        grd.HeaderStyle.BackColor = ColorTranslator.FromHtml("#003399");
        grd.RowStyle.CssClass = "rows";
        grd.BackColor = System.Drawing.Color.White;
        grd.BorderColor = ColorTranslator.FromHtml("#3366CC");
        grd.BorderStyle = BorderStyle.None;
        grd.BorderWidth = new Unit(1, UnitType.Pixel);
        //grd.PageSize = 15;
        //grd.AllowPaging = true;
        grd.CellPadding = 4;
        grd.DataKeyNames = new string[] { "RowNo" };
        grd.EmptyDataText = "No records Found";
        grd.Width = new Unit(100, UnitType.Percentage);
        grd.ShowHeaderWhenEmpty = true;


        grd.GenBoundField("ลำดับ", "RowNo", "centerRow", HorizontalAlign.Center);
        grd.GenTemplateField("เลขที่เอกสาร", "DocNo", "ddlDocNo", "centerRow", "header-center", HorizontalAlign.Center);
        grd.GenBoundField("หมายเลขรถ", "TruckNo", "leftRow", HorizontalAlign.Left);
        //grd.GenTemplateFieldButton("หมายเลขรถ", "TruckNo", "", "centerRow", "header-center", HorizontalAlign.Center, "return openPopup();");
        //grd.GenBoundFieldWithRoWCmd("หมายเลขรถ", "TruckNo", "", HorizontalAlign.Center);
        //grd.GenTemplateField("หมายเลขรถ", "หมายเลขรถ", "ddlTruckNo", "centerRow", "header-center", HorizontalAlign.Center);
        grd.GenBoundField("Route ID", "RouteID", "leftRow", HorizontalAlign.Left);
        grd.GenBoundField("วันส่ง", "TransportDate", "centerRow", HorizontalAlign.Center, "{0:dd/MM/yyyy}");
        grd.GenBoundField("ร้านค้า", "CardName", "leftRow", HorizontalAlign.Left);
        grd.GenBoundField("Invoice Doc No.", "InvDocNo", "leftRow", HorizontalAlign.Left);
        grd.GenBoundField("สถานะเอกสาร", "DocStatus", "leftRow", HorizontalAlign.Left);
        grd.GenBoundField("จำนวน", "Quantity", "rigthRow", HorizontalAlign.Right, "{0:N2}");
        grd.GenBoundField("valume", "VolumeFG", "rigthRow", HorizontalAlign.Right, "{0:N2}");
        grd.GenBoundField("จำนวน(เป้าหมาย)", "TargetQty", "rigthRow", HorizontalAlign.Right, "{0:N2}");
        grd.GenBoundField("valume(เป้าหมาย)", "TargetVolume", "rigthRow", HorizontalAlign.Right, "{0:N2}");
        //grd.GenBoundField("สถานะ", "สถานะ", "centerRow", HorizontalAlign.Center);
        grd.GenBoundField("หมายเหตุ", "SO_Remark", "leftRow", HorizontalAlign.Center);

        return grd;
    }

    private void Grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // CREATE A LINK BUTTON.
                //LinkButton lb = new LinkButton();
                //lb.ID = e.Row.Cells[0].Text;
                //lb.Text = "Update";
                //// ADD LINK BUTTON CLICKED EVENT.
                //lb.Click += new EventHandler(LinkClicked);

                //Label col0 = new Label();
                //col0.ID = "lblRowNo";
                //col0.Text = "";
                //e.Row.Cells[0].Controls.Add(col0);

                //// NOW CREATE A DROPDOWN LIST CONTROL.
                //DropDownList ddlDocNo = new DropDownList();
                //ddlDocNo.ID = "ddlDocNo";
                //ddlDocNo.BindDropdownList((DataTable)Session["DocNoList"], "DocName", "DocNo");
                //// ADD DROPDOWN CHANGED EVENT.
                //ddlDocNo.SelectedIndexChanged += new EventHandler(DdlDocNo_SelectedIndexChanged);
                //e.Row.Cells[1].Controls.Add(ddlDocNo);

                //DropDownList ddlTruckNo = new DropDownList();
                //ddlTruckNo.ID = "ddlTruckNo";
                //ddlTruckNo.BindDropdownList((DataTable)Session["TruckNoList"], "LicencepleteNo", "TruckNo");
                //ddlTruckNo.SelectedIndexChanged += new EventHandler(DdlTruckNo_SelectedIndexChanged);
                //e.Row.Cells[2].Controls.Add(ddlTruckNo);

                //if (sender is GridView)
                //{
                //    GridView grd = sender as GridView;
                //    if (grd.DataSource != null)
                //    {
                //        DataTable dt = new DataTable();
                //        dt = (DataTable)grd.DataSource;
                //        if (dt != null && dt.Rows.Count > 0)
                //        {
                //            for (int i = 3; i < dt.Columns.Count - 1; i++)
                //            {
                //                Label col = new Label();
                //                col.ID = "lblCol" + i.ToString();
                //                col.Text = "";
                //                e.Row.Cells[i].Controls.Add(col);
                //            }
                //        }
                //    }
                //}

                if (e.Row.RowIndex > 0)
                {
                    DropDownList ddlDocNo = (e.Row.FindControl("ddlDocNo") as DropDownList);
                    if (ddlDocNo != null)
                    {
                        ddlDocNo.BindDropdownList((DataTable)Session["DocNoList"], "DocName", "DocNo");

                        //ddlDocNo.AutoPostBack = true;
                        //ddlDocNo.SelectedIndexChanged += new EventHandler(DdlDocNo_SelectedIndexChanged);
                        //ddlDocNo.SelectedIndexChanged += DdlDocNo_SelectedIndexChanged;
                    }

                    //DropDownList ddlTruckNo = (e.Row.FindControl("ddlTruckNo") as DropDownList);
                    //if (ddlTruckNo != null)
                    //{
                    //    ddlTruckNo.BindDropdownList((DataTable)Session["TruckNoList"], "LicencepleteNo", "TruckNo");
                    //    //ddlTruckNo.AutoPostBack = true;
                    //    //ddlTruckNo.SelectedIndexChanged += new EventHandler(DdlTruckNo_SelectedIndexChanged);
                    //    //ddlTruckNo.SelectedIndexChanged += DdlTruckNo_SelectedIndexChanged;
                    //}

                    if (sender is GridView)
                    {
                        GridView grd = sender as GridView;
                        if (grd.DataSource != null)
                        {
                            DataTable dt = new DataTable();
                            dt = (DataTable)grd.DataSource;
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                //string truckNo = dt.Rows[1]["หมายเลขรถ"] != null && dt.Rows[1]["หมายเลขรถ"] != DBNull.Value ? dt.Rows[1]["หมายเลขรถ"].ToString() : "";
                                //if (!string.IsNullOrEmpty(truckNo))
                                //{
                                //    ddlTruckNo.SelectedValue = truckNo;
                                //}

                                foreach (DataRow row in dt.Rows)
                                {
                                    string docNo = "";
                                    docNo = row["DocNo"].ToString();
                                    if (!string.IsNullOrEmpty(docNo))
                                    {
                                        ddlDocNo.SelectedValue = docNo;
                                        if (grd.ID == "grdR1" && docNo == "TRN6300113")
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (e.Row.RowIndex == 0)
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#FFC000");
                    e.Row.Font.Bold = true;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
        
    }

    protected void DdlTruckNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
    }

    protected void DdlDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
    }

    #endregion

    #region Event Methods

    protected void ddlRouteAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is DropDownList)
        {
            var ddl = sender as DropDownList;
            PrepareAllGridView(ddl);
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {

    }

    protected void ddlRoute1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    #endregion
}