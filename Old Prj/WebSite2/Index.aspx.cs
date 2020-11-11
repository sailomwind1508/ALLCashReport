using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();

    protected void Page_Load(object sender, EventArgs e)
    {
        string clearSessionFlag = "0";
        if (Request.QueryString["ClearSessionFlag"] != null)
        {
            clearSessionFlag = Request.QueryString["ClearSessionFlag"].ToString();
            if (clearSessionFlag == "1")
            {
                Session.Clear();
            }
        }
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
        {
            cn.Open();

            string username = txtUser.Text;
            string password = txtPass.Text;
           
            SqlCommand cmd = new SqlCommand("proc_AllCashReport_Login", cn);
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
                Response.Redirect("~/AllCashReport_V2.aspx");
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
    }