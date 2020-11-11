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

public partial class TestMasterPage2 : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    static List<VanDetails> vanList = new List<VanDetails>();
    static List<SaleArea> salArea = new List<SaleArea>();
    List<User> LoginUser = new List<User>();
    List<UserPermission> upList = new List<UserPermission>();
    List<User> userList = new List<User>();
    Dictionary<string, string> permisList1 = new Dictionary<string, string>();
    Dictionary<string, string> permisList2 = new Dictionary<string, string>();

    ReportDocument rdoc = new ReportDocument();
    bool validateFilter = false;
    string connStrng = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["AllCashReportLogin"] != null)
        //{
        //    UserList = (Dictionary<string, bool>)Session["AllCashReportLogin"];

        //    if (UserList.First().Value.ToString() == "0")
        //    {
        //        Response.Redirect("~/index.aspx");
        //    }
        //}
        //if (Session["AllCashReportLogin"] == null)
        //{
        //    Response.Redirect("~/index.aspx");
        //}

        if (!IsPostBack)
        {
            BindDDLUser();
        }

        ManageEvent();
    }

    private void ManageEvent()
    {
        BindGridView();

        if (ViewState["AccMngList"] != null)
        {
            DataTable accMngList = (DataTable)ViewState["AccMngList"];

            upList = Helper.ConvertDataTable<UserPermission>(accMngList);

            if (upList.Count > 0)
            {
                var requestTarget = this.Request["__EVENTTARGET"];
                //var requestArgs = this.Request["__EVENTARGUMENT"];
                if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
                {
                    if (requestTarget.Contains("|"))
                    {
                        string perPK = requestTarget.Split('|')[0];
                        string mode = requestTarget.Split('|')[1];

                        foreach (UserPermission item in upList)
                        {
                            string _userPk = item.UserPK.ToString();
                            string _perPk = item.PerPK.ToString();
                            if (perPK == _perPk)
                            {
                                if (mode == "DEL")
                                {
                                    RemoveData(perPK);
                                }
                                else if (mode == "EDIT")
                                {
                                    ViewState["UserPerMode"] = "EDIT";

                                    txtUserName.Text = item.UserName;
                                    txtPassword.Text = item.Password;
                                    hfUserPK.Value = _userPk;

                                    txtPageName.Text = item.PageName;
                                    txtPageDesc.Text = item.PageDesc;
                                    hdfPermissionPK.Value = _perPk;

                                    ddlUser.SelectedValue = item.UserPK.ToString();
                                }
                            }
                        }
                    }
                    else if (requestTarget == "save_per")
                    {
                        SavePermission();
                    }
                    else if (requestTarget == "save_user")
                    {
                        SaveUser();
                    }
                    else if (requestTarget == "delete_user")
                    {
                        DeleteUser();
                    }
                }
            }
        }
        if (ViewState["UserMngList"] != null)
        {
            DataTable userMngList = (DataTable)ViewState["UserMngList"];

            userList = Helper.ConvertDataTable<User>(userMngList);

            if (userList.Count > 0)
            {
                var requestTarget = this.Request["__EVENTTARGET"];
                //var requestArgs = this.Request["__EVENTARGUMENT"];
                if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
                {
                    if (requestTarget.Contains("|"))
                    {
                        string userPK = requestTarget.Split('|')[0];
                        string mode = requestTarget.Split('|')[1];

                        foreach (User item in userList)
                        {
                            string _userPk = item.UserPK.ToString();

                            if (userPK == _userPk)
                            {
                                if (mode == "DEL_U")
                                {
                                    DeleteUser(Convert.ToInt32(userPK));
                                }
                                else if (mode == "EDIT_U")
                                {
                                    ViewState["UserPerMode"] = "EDIT";

                                    txtUserName.Text = item.UserName;
                                    txtPassword.Text = item.Password;
                                    hfUserPK.Value = _userPk;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void BindDDLUser()
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand("proc_AccountManagement_GetUserData", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "AllUser");

                DataTable allUser = ds.Tables["AllUser"];

                ddlUser.DataSource = allUser;
                ddlUser.DataTextField = "UserName";
                ddlUser.DataValueField = "UserPK";
                ddlUser.DataBind();


                cn.Close();
            }
        }
        catch (Exception ex)
        {

            Helper.WriteLog(ex.Message);
            throw ex;
        }

    }

    private void BindGridView()
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand("proc_AccountManagement_GetData", cn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = 0;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "AccMng");


                cmd = new SqlCommand("proc_AccountManagement_GetUserData", cn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = 0;

                SqlDataAdapter da2 = new SqlDataAdapter(cmd);
                DataSet ds2 = new DataSet();
                da2.Fill(ds2, "UserMng");




                grdAccMng.DataSource = ds.Tables["AccMng"];
                grdAccMng.DataBind();

                ViewState["AccMngList"] = ds.Tables["AccMng"];

                grdUser.DataSource = ds2.Tables["UserMng"];
                grdUser.DataBind();

                ViewState["UserMngList"] = ds2.Tables["UserMng"];

                cn.Close();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdAccMng_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //int index = Convert.ToInt32(e.CommandArgument);
            //string _pk = "";
            //_pk = grdAccMng.DataKeys[index].Value.ToString();

            //if (!string.IsNullOrEmpty(_pk))
            //{
            //    if (e.CommandName == "imgEdit")
            //    {

            //    }
            //    else if (e.CommandName == "imgRemove")
            //    {


            //    }
            //}
        }
        catch (Exception ex)
        {

            Helper.WriteLog(ex.Message);
            throw ex;
        }

    }

    private void RemoveData(string pk)
    {
        try
        {
            if (!string.IsNullOrEmpty(pk))
            {
                using (SqlConnection cn = new SqlConnection(connStrng))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand("proc_AccountManagement_RemovePermission", cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PermissionPK", Convert.ToInt32(pk)));
                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();

                    cn.Close();

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showDeleteMsg(0)", true);
                }

                BindGridView();

                ClearPermission();
                ClearUser();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void linkExportReport_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand("proc_AccountManagement_ExportExcel", cn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "AccMngExcel");

                DataTable _dt = ds.Tables["AccMngExcel"];

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_dt, "AccMngExcel");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=AccountReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                    }
                    Response.Flush();
                    Response.End();
                }

                cn.Close();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearUser();
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "javascript: $('#tabs').tabs({ active: 1 });", true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "javascript: $('#tabs').tabs({ active: 0 });", true);
    }

    protected void grdAccMng_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton edit = e.Row.Cells[4].Controls[0] as ImageButton;
                if (edit != null)
                {
                    string edit_jScript = "javascript: return editItem(this.name, this.alt); ";

                    edit.Attributes.Add("OnClick", edit_jScript);
                }

                ImageButton del = e.Row.Cells[5].Controls[0] as ImageButton;
                if (del != null)
                {
                    string del_jScript = "javascript: return deleteItem(this.name, this.alt); ";

                    del.Attributes.Add("OnClick", del_jScript);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void btnCancel_Per_Click(object sender, EventArgs e)
    {
        ClearPermission();
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "javascript: $('#tabs').tabs({ active: 2 });", true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "javascript: $('#tabs').tabs({ active: 0 });", true);
    }

    protected void linkAddUser_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["UserPerMode"] = "ADD";

            txtUserName.Text = "";
            txtPassword.Text = "";
            hfUserPK.Value = null;

            txtPageName.Text = "";
            txtPageDesc.Text = "";
            hdfPermissionPK.Value = null;

            //ddlPageName.SelectedIndex = 0;
            ddlUser.SelectedIndex = 0;
            //hdfPermissionPKR.Value = null;

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "javascript: $('#tabs').tabs({ active: 2 });", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "javascript: $('#tabs').tabs({ active: 2 });", true);
        }
        catch (Exception ex)
        {

            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void DeleteUser(int userPK = -1)
    {
        try
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblVUserName.Visible = true;
                lblVPassword.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ValidateDelete(1)", true);
                return;
            }

            int pk = -1;
            if (userPK == -1)
            {
                pk = hfUserPK.Value != null && !string.IsNullOrEmpty(hfUserPK.Value) ? Convert.ToInt32(hfUserPK.Value) : -1;
            }
            else
            {
                pk = userPK;
            }

            if (pk != -1)
            {
                using (SqlConnection cn = new SqlConnection(connStrng))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand("proc_AccountManagement_RemoveUser", cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserPK", pk));
                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();

                    cn.Close();

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showDeleteMsg(1)", true);
                }

                BindGridView();

                ClearUser();
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SavePermission()
    {
        try
        {
            if (string.IsNullOrEmpty(txtPageName.Text) || string.IsNullOrEmpty(txtPageDesc.Text))
            {
                lblVPageName.Visible = true;
                lblVPageDesc.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ValidateSave(2)", true);
                return;
            }

            int userPk = Convert.ToInt32(ddlUser.SelectedValue);
            int perPk = hdfPermissionPK.Value != null && !string.IsNullOrEmpty(hdfPermissionPK.Value) ? Convert.ToInt32(hdfPermissionPK.Value) : -1;
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                if (hdfPermissionPK.Value != null && !string.IsNullOrEmpty(hdfPermissionPK.Value))
                {
                    cmd = new SqlCommand("proc_AccountManagement_EditPermission", cn);

                }
                else
                {
                    cmd = new SqlCommand("proc_AccountManagement_AddPermission", cn);
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserPK", userPk));
                cmd.Parameters.Add(new SqlParameter("@PageName", txtPageName.Text));
                cmd.Parameters.Add(new SqlParameter("@PageDesc", txtPageDesc.Text));
                cmd.Parameters.Add(new SqlParameter("@PermissionPK", perPk));
                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();

                cn.Close();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showSaveMsg(0)", true);
            }

            BindGridView();

            ClearPermission();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void SaveUser()
    {
        try
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblVUserName.Visible = true;
                lblVPassword.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ValidateSave(1)", true);
                return;
            }

            int userPk = hfUserPK.Value != null && !string.IsNullOrEmpty(hfUserPK.Value) ? Convert.ToInt32(hfUserPK.Value) : -1;
            using (SqlConnection cn = new SqlConnection(connStrng))
            {
                cn.Open();

                SqlCommand cmd = null;

                if (hfUserPK.Value != null && !string.IsNullOrEmpty(hfUserPK.Value))
                {
                    cmd = new SqlCommand("proc_AccountManagement_EditUser", cn);

                }
                else
                {
                    cmd = new SqlCommand("proc_AccountManagement_AddUser", cn);
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserName", txtUserName.Text));
                cmd.Parameters.Add(new SqlParameter("@Password", txtPassword.Text));
                cmd.Parameters.Add(new SqlParameter("@UserPK", userPk));
                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();

                cn.Close();

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showSaveMsg(1)", true);
            }

            BindDDLUser();

            BindGridView();
            ClearUser();
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    protected void grdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton edit = e.Row.Cells[2].Controls[0] as ImageButton;
                if (edit != null)
                {
                    string edit_jScript = "javascript: return editItem_user(this.name, this.alt); ";

                    edit.Attributes.Add("OnClick", edit_jScript);
                }

                ImageButton del = e.Row.Cells[3].Controls[0] as ImageButton;
                if (del != null)
                {
                    string del_jScript = "javascript: return deleteItem_user(this.name, this.alt); ";

                    del.Attributes.Add("OnClick", del_jScript);
                }
            }
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            throw ex;
        }
    }

    private void ClearUser()
    {
        txtUserName.Text = "";
        txtPassword.Text = "";
        hfUserPK.Value = null;
    }

    private void ClearPermission()
    {
        txtPageName.Text = "";
        txtPageDesc.Text = "";
        hdfPermissionPK.Value = null;
    }
}