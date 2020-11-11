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

public partial class ASDashBoard : System.Web.UI.Page
{
    static string Condition = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Condition = "gamt";

            BindGridView(grdDash1, "WS", "gamt");
            BindGridView(grdDash2, "EX1", "gamt");

            //ALLC
            //BDT
            //EX1
            //EX2
            //MT
            //WS
        }
    }

    private void BindGridView(GridView grd, string channel, string condition)
    {
        var dt = LoadData(channel, condition);
        var dtHd = GetHeader(channel, condition);
        if (dt.Rows.Count > 0 && dtHd.Rows.Count > 0)
        {
            grd.DataSource = dt;

            var m = dtHd.Rows[0]["Month"].ToString();
            var y = dtHd.Rows[0]["Year"].ToString();
            var ch = dtHd.Rows[0]["Channel"].ToString();
            var ds = "";
            switch (condition)
            {
                case "gamt": { ds = "Gross Amount"; } break;
                case "namt": { ds = "Net Amount"; } break;
                case "qty": { ds = "Quantity"; } break;
                default:
                    break;
            }

            var title = ds + " - " + ch + " ประจำเดือน " + m + "/" + y;

            lblTitle.Text = title;

        }
        else
            grd.DataSource = null;

        grd.DataBind();
    }

    private DataTable LoadData(string channel, string condition)
    {
        try
        {
            DataTable dt = new DataTable("Dash1");
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("proc_as_report_by_ch_r2", con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

     
                        cmd.Parameters.Add(new SqlParameter("@Channel", channel));
                        cmd.Parameters.Add(new SqlParameter("@Condition", condition));
                        cmd.Parameters.Add(new SqlParameter("@Month", 4));
                        cmd.Parameters.Add(new SqlParameter("@Year", 2020));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", 1));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", 10));

                        Condition = condition;

                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    private DataTable GetHeader(string channel, string condition)
    {
        try
        {
            DataTable dt = new DataTable("Dash1_Header");
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("proc_as_report_by_ch_header", con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@Channel", channel));
                        cmd.Parameters.Add(new SqlParameter("@Condition", condition));
                        cmd.Parameters.Add(new SqlParameter("@Month", 4));
                        cmd.Parameters.Add(new SqlParameter("@Year", 2020));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", 1));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", 10));

                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);
            return null;
        }
    }

    protected void btnGAmt_Click(object sender, EventArgs e)
    {
        BindGridView(grdDash1, "WS", "gamt");
        BindGridView(grdDash2, "EX1", "gamt");
    }

    protected void btnNAmt_Click(object sender, EventArgs e)
    {
        BindGridView(grdDash1, "WS", "namt");
        BindGridView(grdDash2, "EX1", "namt");
    }

    protected void btnQty_Click(object sender, EventArgs e)
    {
        BindGridView(grdDash1, "WS", "qty");
        BindGridView(grdDash2, "EX1", "qty");
    }
}