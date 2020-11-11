using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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

public partial class MasterPage : System.Web.UI.MasterPage
{
    private List<User> userList = new List<User>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            p1.Visible = false;
            p2.Visible = false;
            p3.Visible = false;
            p4.Visible = false;
            p5.Visible = false;
            p6.Visible = false;
            p7.Visible = false;
            p8.Visible = false;
            p9.Visible = false;
            p10.Visible = false;
            p11.Visible = false;
            p12.Visible = false;
            p13.Visible = false;
            p14.Visible = false;
            //p15.Visible = false;
            p16.Visible = false;
            p17.Visible = false;
            p18.Visible = false;
            //p19.Visible = false;
            p20.Visible = false;
            p21.Visible = false;
            p22.Visible = false;
            p23.Visible = false;
            p24.Visible = false;
            p25.Visible = false;
            p26.Visible = false;
            p27.Visible = false;
            p28.Visible = false;
            p29.Visible = false;

            s1.Visible = false;
            s2.Visible = false;

            try
            {
                if (Session["UserList"] != null)
                {
                    userList = (List<User>)Session["UserList"];
                    ManageUserPermission();
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex.Message);
                throw ex;
            }
        }
    }

    private void ManageUserPermission()
    {
        if (userList.Count > 0 && Session["AllCashReportLogin"] != null)
        {
            Dictionary<string, bool> login = (Dictionary<string, bool>)Session["AllCashReportLogin"];

            List<User> users = new List<User>();
            users = userList.Where(x => x.UserName == login.First().Key).ToList();
            if (users.Count > 0)
            {
                p1.Visible = users.Any(x => x.PageName == p1.HRef);
                p2.Visible = users.Any(x => x.PageName == p2.HRef);
                p3.Visible = users.Any(x => x.PageName == p3.HRef);
                p4.Visible = users.Any(x => x.PageName == p4.HRef);  
                p5.Visible = users.Any(x => x.PageName == p5.HRef);
                p6.Visible = users.Any(x => x.PageName == p6.HRef);
                p7.Visible = users.Any(x => x.PageName == p7.HRef);
                p8.Visible = users.Any(x => x.PageName == p8.HRef);
                p9.Visible = users.Any(x => x.PageName == p9.HRef);
                p10.Visible = users.Any(x => x.PageName == p10.HRef);
                p11.Visible = users.Any(x => x.PageName == p11.HRef);
                p12.Visible = users.Any(x => x.PageName == p12.HRef);
                p13.Visible = users.Any(x => x.PageName == p13.HRef);
                p14.Visible = users.Any(x => x.PageName == p14.HRef);
                //p15.Visible = users.Any(x => x.PageName == p15.HRef);
                p16.Visible = users.Any(x => x.PageName == p16.HRef);
                p17.Visible = users.Any(x => x.PageName == p17.HRef);
                p18.Visible = users.Any(x => x.PageName == p18.HRef);
                //p19.Visible = users.Any(x => x.PageName == p19.HRef);
                p20.Visible = users.Any(x => x.PageName == p20.HRef);
                p21.Visible = users.Any(x => x.PageName == p21.HRef);
                p22.Visible = users.Any(x => x.PageName == p22.HRef);
                p23.Visible = users.Any(x => x.PageName == p23.HRef);
                p24.Visible = users.Any(x => x.PageName == p24.HRef);
                p25.Visible = users.Any(x => x.PageName == p25.HRef);
                p26.Visible = users.Any(x => x.PageName == p26.HRef);
                p27.Visible = users.Any(x => x.PageName == p27.HRef);
                p28.Visible = users.Any(x => x.PageName == p28.HRef);
                p29.Visible = users.Any(x => x.PageName == p29.HRef);

                s1.Visible = users.Any(x => x.PageName == s1.HRef);
                s2.Visible = users.Any(x => x.PageName == s2.HRef);
            }
        }
    }
}
