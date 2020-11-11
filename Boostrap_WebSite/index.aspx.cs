using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    private List<User> LoginUser = new List<User>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            ViewState["MonthlySalesSumReport"] = null;
            ViewState.Remove("MonthlySalesSumReport");

            txtusername.Focus();
        }


        var requestTarget = this.Request["__EVENTTARGET"];
        //var requestArgs = this.Request["__EVENTARGUMENT"];
        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "enter_pass")
            {
                login();
            }
        }
    }

    private void login()
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand("proc_AllCashReport_GetPermission", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User u = new User();
                u.UserName = reader["UserName"].ToString();
                u.Password = reader["Password"].ToString();
                u.PageName = reader["PageName"].ToString();

                LoginUser.Add(u);
            }

            Session["UserList"] = LoginUser;

            cn.Close();

            cn.Open();

            string username = txtusername.Text;
            string password = txtpassword.Text;

            cmd = new SqlCommand("proc_AllCashReport_Login", cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@UserName", username));
            cmd.Parameters.Add(new SqlParameter("@Password", password));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            int ret = 0;
            ret = Convert.ToInt32(cmd.ExecuteScalar());
            if (ret == 1)
            {
                if (UserList.Count > 0 && UserList.All(x => x.Key != username))
                {
                    UserList.Add(username, true);
                }
                else
                {
                    UserList.Add(username, true);
                }

                Session["AllCashReportLogin"] = UserList;
                Dictionary<string, bool> login = UserList;

                List<User> users = new List<User>();
                users = LoginUser.Where(x => x.UserName == login.First().Key).ToList();
                if (users.Count > 0)
                {
                    Response.Redirect("~/" + users.First().PageName);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "showValidateMsg();", true);

                if (UserList.Count > 0 && UserList.All(x => x.Key != username))
                {
                    UserList.Add(username, false);
                }
                else
                {
                    UserList.Add(username, false);
                }
                Session["AllCashReportLogin"] = UserList;

            }
            cn.Close();
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        login();
    }
}
