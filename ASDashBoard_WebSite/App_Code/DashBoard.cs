using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Report_ClassLibrary;
using System.Configuration;

/// <summary>
/// Summary description for DashBoard
/// </summary>
public class DashBoard
{
    public DashBoard()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataTable LoadData(DropDownList ddlMonth, DropDownList ddlYear, DropDownList ddlValueType, DropDownList ddlPeriod, string channel, bool isTest = false)
    {
        try
        {
            DataTable dt = new DataTable("Dash1");
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            //string channel = "WS";
            string condition = ddlValueType.SelectedValue.ToString();
            var period = ddlPeriod.SelectedValue.ToString();

            var _temp_date = DateTime.DaysInMonth(year, month);

            int sPeriod = 0;
            int ePeriod = 0;

            if (period == "1-10")
            {
                sPeriod = 1;
                ePeriod = 10;
            }
            else if (period == "11-20")
            {
                sPeriod = 11;
                ePeriod = 20;
            }
            else if (period == "21-31")
            {
                sPeriod = 21;
                ePeriod = _temp_date;
            }

            if (isTest)
            {
                sPeriod = 1;
                ePeriod = _temp_date;
            }

            var exChannel = new List<string> { "BDT", "EX1", "EX2" };

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                var storeName = exChannel.Any(x => x == channel) ? "proc_as_report_by_ch_ex" : "proc_as_report_by_ch_r2";

                using (var cmd = new SqlCommand(storeName, con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@Channel", channel));
                        cmd.Parameters.Add(new SqlParameter("@Condition", condition));
                        cmd.Parameters.Add(new SqlParameter("@Month", month));
                        cmd.Parameters.Add(new SqlParameter("@Year", year));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", sPeriod));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", ePeriod));

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

    public DataTable LoadData_PerDay(DropDownList ddlMonth, DropDownList ddlYear, DropDownList ddlValueType, DropDownList ddlPeriod, string channel, bool isTest = false)
    {
        try
        {
            DataTable dt = new DataTable("Dash_PerDay");
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            //string channel = "WS";
            string condition = ddlValueType.SelectedValue.ToString();
            var period = ddlPeriod.SelectedValue.ToString();

            var _temp_date = DateTime.DaysInMonth(year, month);

            int sPeriod = 0;
            int ePeriod = 0;

            if (period == "1-10")
            {
                sPeriod = 1;
                ePeriod = 10;
            }
            else if (period == "11-20")
            {
                sPeriod = 11;
                ePeriod = 20;
            }
            else if (period == "21-31")
            {
                sPeriod = 21;
                ePeriod = _temp_date;
            }

            if (isTest)
            {
                sPeriod = 1;
                ePeriod = _temp_date;
            }

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("proc_as_report_by_ch_per_day", con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;


                        cmd.Parameters.Add(new SqlParameter("@Channel", channel));
                        cmd.Parameters.Add(new SqlParameter("@Condition", condition));
                        cmd.Parameters.Add(new SqlParameter("@Month", month));
                        cmd.Parameters.Add(new SqlParameter("@Year", year));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", sPeriod));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", ePeriod));

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

    public void GetHeader(Literal lbl, DropDownList ddlMonth, DropDownList ddlYear, DropDownList ddlValueType, DropDownList ddlPeriod, string channel)
    {
        try
        {
            DataTable dt = new DataTable("Dash1");
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            //string channel = "WS";
            string condition = ddlValueType.SelectedValue.ToString();
            var period = ddlPeriod.SelectedValue.ToString();

            int sPeriod = 0;
            int ePeriod = 0;

            if (period == "1-10")
            {
                sPeriod = 1;
                ePeriod = 10;
            }
            else if (period == "11-20")
            {
                sPeriod = 11;
                ePeriod = 20;
            }
            else if (period == "21-31")
            {
                sPeriod = 21;
                ePeriod = 31;
            }

            DataTable dtHd = new DataTable("Dash1_Header");
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
                        cmd.Parameters.Add(new SqlParameter("@Month", month));
                        cmd.Parameters.Add(new SqlParameter("@Year", year));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", sPeriod));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", ePeriod));

                        da.Fill(dtHd);
                    }
                }
            }

            //if (dtHd.Rows.Count > 0)
            //{
            //    var m = dtHd.Rows[0]["Month"].ToString();
            //    var y = dtHd.Rows[0]["Year"].ToString();
            //    var ch = dtHd.Rows[0]["Channel"].ToString();
            //    var ds = "";
            //    switch (condition)
            //    {
            //        case "gamt": { ds = "Gross Amount"; } break;
            //        case "namt": { ds = "Net Amount"; } break;
            //        case "qty": { ds = "Quantity"; } break;
            //        default:
            //            break;
            //    }
            //}

            var ds = "";
            switch (condition)
            {
                case "gamt": { ds = "Gross Amount"; } break;
                case "namt": { ds = "Net Amount"; } break;
                case "qty": { ds = "Quantity"; } break;
                default:
                    break;
            }

            var title = ds + " - " + channel + " ประจำเดือน " + month + "/" + year;

            lbl.Text = title;
        }
        catch (Exception ex)
        {
            Helper.WriteLog(ex.Message);

        }
    }

    public void UpdateData(DashBoardModel _model)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
        {
            cn.Open();

            SqlCommand cmd = null;

            cmd = new SqlCommand("proc_as_report_update", cn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CDate", _model.CDate));
            cmd.Parameters.Add(new SqlParameter("@ORDER", _model.Order));
            cmd.Parameters.Add(new SqlParameter("@TYPE", _model.Type));
            cmd.Parameters.Add(new SqlParameter("@Price", _model.Price));
            cmd.Parameters.Add(new SqlParameter("@Remark", _model.Remark));
            cmd.CommandTimeout = 0;

            cmd.ExecuteNonQuery();

            cn.Close();

           
        }
    }

    public void RemoveData(int day, int month, int year, string order, string type)
    {
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
        {
            cn.Open();

            SqlCommand cmd = null;

            cmd = new SqlCommand("proc_as_report_delete", cn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Day", day));
            cmd.Parameters.Add(new SqlParameter("@Month", month));
            cmd.Parameters.Add(new SqlParameter("@Year", year));
            cmd.Parameters.Add(new SqlParameter("@Order", order));
            cmd.Parameters.Add(new SqlParameter("@Type", type));
            cmd.CommandTimeout = 0;

            cmd.ExecuteNonQuery();

            cn.Close();


        }
    }

    public DataTable GetDailyData(DropDownList ddlMonth, DropDownList ddlYear, DropDownList ddlPeriod = null)
    {
        try
        {
            DataTable dt = new DataTable("dailydata");
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);
            var _temp_date = DateTime.DaysInMonth(year, month);

            int sPeriod = 1;
            int ePeriod = _temp_date;

            if (ddlPeriod != null)
            {
                var period = ddlPeriod.SelectedValue.ToString();

                if (period == "1-10")
                {
                    sPeriod = 1;
                    ePeriod = 10;
                }
                else if (period == "11-20")
                {
                    sPeriod = 11;
                    ePeriod = 20;
                }
                else if (period == "21-31")
                {
                    sPeriod = 21;
                    ePeriod = _temp_date;
                }
            }

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("proc_as_report_get_dailydata", con))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@Month", month));
                        cmd.Parameters.Add(new SqlParameter("@Year", year));
                        cmd.Parameters.Add(new SqlParameter("@SPeriod", sPeriod));
                        cmd.Parameters.Add(new SqlParameter("@EPeriod", ePeriod));

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
}