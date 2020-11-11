using Report_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class SalesSumLocation : System.Web.UI.Page
{
    Dictionary<string, bool> UserList = new Dictionary<string, bool>();
    static List<CustInfo> custInfoList = new List<CustInfo>();
    static string json = string.Empty;
    List<User> LoginUser = new List<User>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AllCashReportLogin"] != null)
        {
            UserList = (Dictionary<string, bool>)Session["AllCashReportLogin"];

            if (UserList.First().Value.ToString() == "0")
            {
                //Session["AllCashReportLogin"] = "0";
                Response.Redirect("~/index.aspx");
            }

            //if (UserList.First().Key.Contains("acc"))
            //{
            //    Response.Redirect("~/AllCashReport_V2.aspx");
            //}
        }
        if (Session["AllCashReportLogin"] == null)
        {
            //UserList.Add(username, false);

            //Session["AllCashReportLogin"] = "0";
            Response.Redirect("~/index.aspx");
        }

        if (!IsPostBack)
        {
            if (Session["custInfoList"] != null)
            {
                custInfoList = (List<CustInfo>)Session["custInfoList"];

                json = new JavaScriptSerializer().Serialize(custInfoList);

                //if (custInfoList.Count > 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "initMap(" + custInfoList + ")", true);
                //}

            }
        }
    }

    [System.Web.Services.WebMethod]
    public static string GetMapInfo(string data)
    {
        if (custInfoList != null && custInfoList.Count > 0 && !string.IsNullOrEmpty(json))
        {
            return json;
        }
        else
        {
            return null;
        }

    }
}