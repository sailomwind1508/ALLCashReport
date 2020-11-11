using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Login : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    private List<UserLogin> LoginUser = new List<UserLogin>();
    string conStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
   
        if (!IsPostBack)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            Session["QALogin"] = null;

            txtusername.Focus();
        }

        var requestTarget = this.Request["__EVENTTARGET"];
        //var requestArgs = this.Request["__EVENTARGUMENT"];
        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "enter_pass")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "hideValidateMsg();", true);
                LogOn();
            }
        }
    }

    private void LogOn()
    {
        bool verifyLogin = false;
        string username = txtusername.Text;
        string password = txtpassword.Text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("UserName", username);
            p.Add("Password", password);

            var dt = Helper.ExecuteProcedureToTable(conStr, "proc_qa_user_get", p);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    UserLogin u = new UserLogin();
                    u.UserName = row["UserName"].ToString();
                    u.Password = row["Password"].ToString();
                    u.BranchID = row["BranchID"].ToString();
                    u.FullName = row["FullName"].ToString();
                    u.PhoneNo = row["PhoneNo"].ToString();

                    LoginUser.Add(u);
                }

                if (LoginUser.Count > 0)
                {
                    verifyLogin = true;
                    Session["QALogin"] = LoginUser;
                }
            } 
        }

        if (verifyLogin)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "hideValidateMsg();", true);
            Response.Redirect("~/Page1.aspx");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showValidateMsg();", true);
            Session["QALogin"] = null;
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        LogOn();
    }
}
