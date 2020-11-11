using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Report_ClassLibrary
{
    public static class Helper
    {
        static string logsPath = "D:\\AllCashReport_WebSite_Log";
        static string logFullPath = logsPath + "\\AllCashReport_Log.txt";
        static string excelPath = ConfigurationManager.AppSettings["serverExcelPath"];

        public static void PrepareMonthDropdown(this DropDownList ddl)
        {
            var _date = DateTime.Now;
            Dictionary<string, string> monthList = new Dictionary<string, string>();
            monthList.Add("-1", "--All--");
            for (int i = 1; i <= 12; i++)
            {
                var _month = new DateTime(_date.Year, i, 1);
                monthList.Add(i.ToString(), _month.ToString("MMMM"));
            }
            ddl.BindDropdownList(monthList);
            ddl.SelectedValue = _date.Month.ToString();
        }

        public static void PrepareYearDropdown(this DropDownList ddl)
        {
            var _date = DateTime.Now;
            Dictionary<string, string> yearList = new Dictionary<string, string>();
            for (int i = 2010; i <= _date.Year; i++)
            {
                yearList.Add(i.ToString(), i.ToString());
            }
            ddl.BindDropdownList(yearList);
            ddl.SelectedValue = _date.Year.ToString();

        }


        public static decimal ConvertColToDec(DataRow row, int colIndex)
        {
            decimal ret = 0;
            try
            {
                string colStr = "";
                colStr = row[colIndex].ToString();

                if (!string.IsNullOrEmpty(colStr) && colStr != "-")
                {
                    decimal _tmp = 0;
                    if (decimal.TryParse(colStr, out _tmp))
                        ret = _tmp;
                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

        public static int ConvertColToInt(DataRow row, int colIndex)
        {
            int ret = 0;
            try
            {
                string colStr = "";
                colStr = row[colIndex].ToString();

                if (!string.IsNullOrEmpty(colStr) && colStr != "-")
                {
                    int _tmp = 0;
                    if (int.TryParse(colStr, out _tmp))
                        ret = _tmp;
                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

        public static void CreateTextCell(this GridViewRowEventArgs e, int cellIndex, string headerText, string dataField, string itemStyle, HorizontalAlign cellAlign, string dataFormatString = "")
        {
            Label col0 = new Label();
            col0.ID = "lblRowNo";
            col0.Text = "";
            e.Row.Cells[cellIndex].Controls.Add(col0);
        }

        public static void GenTemplateField(this GridView grd, string headerText, string dataField, string ddlID, string itemStyle, string headerStyle, HorizontalAlign cellAlign)
        {
            TemplateField col = new TemplateField();
            col.ItemTemplate = new TemplateGenerator(ListItemType.Item, dataField, ddlID, "ddl");
            col.HeaderText = headerText;
            col.ItemStyle.Wrap = false;
            col.HeaderStyle.CssClass = headerStyle;
            col.ItemStyle.CssClass = itemStyle;
            col.ItemStyle.Wrap = false;
            col.ItemStyle.HorizontalAlign = cellAlign;

            grd.Columns.Add(col);
        }

        public static void GenTemplateFieldButton(this GridView grd, string headerText, string dataField, string ddlID, string itemStyle, string headerStyle, HorizontalAlign cellAlign, string script)
        {
            TemplateField col = new TemplateField();
            col.ItemTemplate = new TemplateGenerator(ListItemType.Item, dataField, ddlID, "btn", script);
            col.HeaderText = headerText;
            col.ItemStyle.Wrap = false;
            col.HeaderStyle.CssClass = headerStyle;
            col.ItemStyle.CssClass = itemStyle;
            col.ItemStyle.Wrap = false;
            col.ItemStyle.HorizontalAlign = cellAlign;

            grd.Columns.Add(col);
        }

        public static void GenBoundField(this GridView grd, string headerText, string dataField, string itemStyle, HorizontalAlign cellAlign, string dataFormatString = "")
        {
            BoundField col = new BoundField();
            col.HeaderText = headerText;
            col.DataField = dataField;
            col.ItemStyle.Wrap = false;
            col.HeaderStyle.CssClass = "header-center";
            col.ItemStyle.CssClass = itemStyle;

            if (!string.IsNullOrEmpty(dataFormatString))
            {
                col.ItemStyle.HorizontalAlign = cellAlign;
                col.DataFormatString = dataFormatString;
            }

            grd.Columns.Add(col);
            //return col;
        }

        public static void GenBoundFieldWithRoWCmd(this GridView grd, string headerText, string dataField, string itemStyle, HorizontalAlign cellAlign)
        {
            ButtonField col = new ButtonField();
            col.HeaderText = headerText;
            col.DataTextField = dataField;
            col.ButtonType = ButtonType.Link;
            col.ItemStyle.Wrap = false;
            col.HeaderStyle.CssClass = "header-center";
            col.ItemStyle.CssClass = itemStyle;
            col.CommandName = "linkTruckNo";
            col.ItemStyle.HorizontalAlign = cellAlign;
            col.ControlStyle.ForeColor = Color.Blue;
            col.ControlStyle.BackColor = ColorTranslator.FromHtml("#BDD7EE");

            grd.Columns.Add(col);
        }

        public static string ToDecimalFormat(this string _val)
        {
            decimal val = 0;
            if (!string.IsNullOrEmpty(_val) && decimal.TryParse(_val, out val))
            {
                return string.Format("{0:#,0.00}", Convert.ToDecimal(_val));
            }
            else
            {
                return "";
            }
        }

        public static string ToNumberFormat(this string _val)
        {
            decimal val = 0;
            if (!string.IsNullOrEmpty(_val) && decimal.TryParse(_val, out val))
            {
                return string.Format("{0:#,###}", Convert.ToDecimal(_val));
            }
            else
            {
                return "";
            }
        }

        public static void ReadSample(FileUpload fileUpload, string path, string sheetName)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            if (excelApp != null)
            {
                Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelWorksheet;
                //for (int index = 1; index <= excelWorkbook.Sheets.Count; index++)
                {
                    excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.Sheets[3];

                    Microsoft.Office.Interop.Excel.Range excelRange = excelWorksheet.UsedRange;
                    int rowCount = excelRange.Rows.Count;
                    int colCount = excelRange.Columns.Count;

                    for (int i = 1; i <= rowCount; i++)
                    {
                        for (int j = 1; j <= colCount; j++)
                        {
                            Microsoft.Office.Interop.Excel.Range range = (excelWorksheet.Cells[i, j] as Microsoft.Office.Interop.Excel.Range);
                            
                            string cellValue = range.Value.ToString();

                            //do anything
                        }
                    }
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorksheet);
                excelWorkbook.Close();
                excelApp.Quit();
            }
        }

        public static DataTable exceldata2(FileUpload fileUpload, string path, string sheetName)
        {
            DataTable dtexcel = new DataTable();
            string con = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0;", path);
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from ["+ sheetName + "$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(dr);
                    //while (dr.Read())
                    //{
                    //    var row1Col0 = dr[0];
                    //    Console.WriteLine(row1Col0);
                    //}
                }
            }
            return dtexcel;

        }

        public static DataTable exceldata(FileUpload fileUpload, string path, string sheetName)
        {
            DataTable dtexcel = new DataTable();
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (Path.GetExtension(fileUpload.FileName).ToUpper() == ".XLS")
            {
                strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;HDR=YES;", path);
            }
            else
            {
                strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0;", path);
            }

            //if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
            //    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            //else
            //    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //Looping Total Sheet of Xl File
            /*foreach (DataRow schemaRow in schemaTable.Rows)
            {
            }*/
            //Looping a first Sheet of Xl File
            DataRow schemaRow = schemaTable.Rows[0];
            string sheet = schemaRow["TABLE_NAME"].ToString();
            if (!sheet.EndsWith("_"))
            {
                string query = "SELECT  * FROM [" + sheetName + "$]";
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.CurrentCulture;
                daexcel.Fill(dtexcel);
            }

            conn.Close();
            return dtexcel;

        }

        public static DataTable ReadExcelToDataTable2(string conString, string fileName, string sheetName)
        {
            var dataTable = new DataTable();
            try
            {
                string serverExcelPath = excelPath + fileName;

                string cmd = " SELECT * ";
                cmd += " FROM OPENROWSET ( ";
                if (Path.GetExtension(fileName).ToUpper() == ".XLS")
                {
                    cmd += " 'Microsoft.Jet.OLEDB.4.0', ";
                    cmd += string.Format(" 'Excel 8.0;Database={0};HDR=YES;', ", serverExcelPath);
                }
                else
                {
                    cmd += " 'Microsoft.ACE.OLEDB.12.0', ";
                    cmd += string.Format(" 'Excel 8.0;Database={0};', ", serverExcelPath);
                }
                cmd += string.Format(" 'SELECT * FROM [{0}$]' ", sheetName);
                cmd += " )";

                dataTable = ExecuteProcedureToTable(conString, cmd, System.Data.CommandType.Text, null);
            }
            catch (Exception ex)
            {
                dataTable = null;
                throw ex;
            }

            return dataTable;
        }

        public static DataTable ReadExcelToDataTable(string conString, string fileName, string sheetName)
        {
            var dataTable = new DataTable();
            try
            {
                string serverExcelPath = excelPath + fileName;

                string cmd = " SELECT * ";
                cmd += " FROM OPENROWSET ( ";
                if (Path.GetExtension(fileName).ToUpper() == ".XLS")
                {
                    cmd += " 'Microsoft.Jet.OLEDB.4.0', ";
                    cmd += string.Format(" 'Excel 8.0;Database={0};HDR=YES;', ", serverExcelPath);
                }
                else
                {
                    cmd += " 'Microsoft.ACE.OLEDB.12.0', ";
                    cmd += string.Format(" 'Excel 8.0;Database={0};', ", serverExcelPath);
                }
                cmd += string.Format(" 'SELECT * FROM [{0}$]' ", sheetName);
                cmd += " )";

                dataTable = ExecuteProcedureToTable(conString, cmd, System.Data.CommandType.Text, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return dataTable;
        }

        public static List<string> GetExcelSheetNames(string fileName, string path)
        {
            List<string> excelSheets = new List<string>();

            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet;

            try
            {

                excelWorkbook = app.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
        Type.Missing, Type.Missing);

                if (excelWorkbook.Sheets.Count > 0)
                {
                    for (int i = 1; i <= excelWorkbook.Sheets.Count; i++)
                    {
                        excelWorkSheet = excelWorkbook.Sheets[i];

                        string _name = excelWorkSheet.ToString().Split('$')[0];
                        string shName = _name.Substring(1, _name.Length - 1);

                        if (excelSheets.All(x => x != shName))
                            excelSheets.Add(shName);
                    }
                }

                //Save the excel 
                excelWorkbook.Save();
                //Close the excel
                excelWorkbook.Close();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


            finally
            {
                //Clean up objects 

                app.Quit();
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

            //try
            //{
            //    DataTable dtexcel = new DataTable();
            //    bool hasHeaders = false;
            //    string HDR = hasHeaders ? "Yes" : "No";
            //    string strConn;
            //    if (Path.GetExtension(fileName).ToUpper() == ".XLS")
            //    {
            //        strConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;HDR=YES;", path);
            //    }
            //    else
            //    {
            //        strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0;", path);
            //    }


            //    OleDbConnection conn = new OleDbConnection(strConn);
            //    conn.Open();
            //    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            //    DataRow schemaRow = schemaTable.Rows[0];
            //    string sheet = schemaRow["TABLE_NAME"].ToString();
            //    if (!sheet.EndsWith("_"))
            //    {
            //        string _name = sheet.ToString().Split('$')[0];
            //        string shName = _name.Substring(1, _name.Length - 1);

            //        if (excelSheets.All(x => x != shName))
            //            excelSheets.Add(shName);
            //    }

            //    conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw ex;
            //}


            return excelSheets;


            //OleDbConnection objConn = null;
            //System.Data.DataTable dt = null;
            //try
            //{
            //    string connString = "";
            //    // Connection String. Change the excel file to the file you
            //    // will search.
            //    if (Path.GetExtension(excelFile).ToUpper() == ".XLS")
            //        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
            //    else
            //        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";

            //    // Create connection object by using the preceding connection string.
            //    objConn = new OleDbConnection(connString);
            //    // Open connection with the database.
            //    objConn.Open();
            //    // Get the data table containg the schema guid.
            //    dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //    if (dt == null)
            //        return null;

            //    List<string> excelSheets = new List<string>();

            //    // Add the sheet name to the string array.
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        string _name = row["TABLE_NAME"].ToString().Split('$')[0];
            //        string shName = _name.Substring(1, _name.Length - 1);

            //        if (excelSheets.All(x => x != shName))
            //            excelSheets.Add(shName);
            //    }

            //    return excelSheets;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            //finally
            //{
            //    // Clean up.
            //    if (objConn != null)
            //    {
            //        objConn.Close();
            //        objConn.Dispose();
            //    }
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //    }
            //}
        }
        
        //public static DataTable ReadExcelToDataTable_Multi(string conString, string fileName, string sheetName)
        //{
        //    var dataTable = new DataTable();
        //    try
        //    {
        //        string serverExcelPath = excelPath + fileName;

        //        string cmd = " SELECT * ";
        //        cmd += " FROM OPENROWSET ( ";
        //        if (Path.GetExtension(fileName).ToUpper() == ".XLS")
        //        {
        //            cmd += " 'Microsoft.Jet.OLEDB.4.0', ";
        //            cmd += string.Format(" 'Excel 8.0;Database={0};HDR=YES;', ", serverExcelPath);
        //        }
        //        else
        //        {
        //            cmd += " 'Microsoft.ACE.OLEDB.12.0', ";
        //            cmd += string.Format(" 'Excel 8.0;Database={0};', ", serverExcelPath);
        //        }
        //        //cmd += string.Format(" 'SELECT * FROM [{0}$]' ", sheetName);
        //        cmd += string.Format(" 'SELECT * FROM {0}'", sheetName);
        //        cmd += " )";

        //        dataTable = ExecuteProcedureToTable(conString, cmd, System.Data.CommandType.Text, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw ex;
        //    }

        //    return dataTable;
        //}

        public static DataTable GetSheetDataAsDataTable(string filePath, string sheetName)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Microsoft.Office.Interop.Excel.Workbook xlBook = null;
            Microsoft.Office.Interop.Excel.Range xlRange = null;
            Microsoft.Office.Interop.Excel.Worksheet xlSheet = null;

            DataTable dt = new DataTable();
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlBook = xlApp.Workbooks.Open(filePath);
                xlSheet = xlBook.Worksheets[sheetName];
                xlRange = xlSheet.UsedRange;
                DataRow row = null;
                for (int i = 1; i <= xlRange.Rows.Count; i++)
                {
                    if (i != 1)
                        row = dt.NewRow();
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        if (i == 1)
                            dt.Columns.Add(xlRange.Cells[1, j].value);
                        else
                            row[j - 1] = xlRange.Cells[i, j].value;
                    }
                    if (row != null)
                        dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                xlBook.Close();
                xlApp.Quit();
            }
            return dt;
        }

        public static void BindDropdownList(this DropDownList ddl, Dictionary<string, string> objs)
        {
            ddl.Items.Clear();
            ddl.DataSource = objs;
            ddl.DataTextField = "Value";
            ddl.DataValueField = "Key";
            ddl.DataBind();
        }

        public static void BindDropdownList(this DropDownList ddl, DataTable objs, string text, string value)
        {
            ddl.Items.Clear();
            ddl.DataSource = objs;
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
        }

        public static void ExecuteProcedure(string conString, string procedureName)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteProcedureToList(this Dictionary<string, string> objs, string conString, string procedureName, string key, string value, Dictionary<string, string> parameters = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        objs.Add(reader[key].ToString(), reader[value].ToString());
                    }

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteProcedure(string conString, string procedureName, Dictionary<string, string> parameters = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecuteProcedureOBJ(string conString, string procedureName, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ExecuteProcedureToTable(string conString, string procedureName, Dictionary<string, object> parameters = null)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    //var dataReader = cmd.ExecuteReader();

                    //dt.Load(dataReader);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dt");
                    if (ds.Tables["dt"] != null)
                    {
                        dt = ds.Tables["dt"];
                    }

                    cn.Close();
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public static DataTable ExecuteProcedureToTable(string conString, string procedureName, CommandType cmdType, Dictionary<string, object> parameters = null)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    //var dataReader = cmd.ExecuteReader();

                    //dt.Load(dataReader);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dt");
                    if (ds.Tables["dt"] != null)
                    {
                        dt = ds.Tables["dt"];
                    }

                    cn.Close();
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ExecuteProcedureToTable2(string conString, string procedureName, CommandType cmdType, Dictionary<string, object> parameters = null)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(conString))
                {
                    cn.Open();

                    SqlCommand cmd = null;

                    cmd = new SqlCommand(procedureName, cn);

                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                        }
                    }

                    cmd.CommandTimeout = 0;

                    //var dataReader = cmd.ExecuteReader();

                    //dt.Load(dataReader);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "dt");
                    if (ds.Tables["dt"] != null)
                    {
                        dt = ds.Tables["dt"];
                    }

                    cn.Close();
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void StoredProceduresToList(this Dictionary<string, string> objs, string conString, string storeName, string key, string value)
        {
            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(storeName, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(reader[key].ToString(), reader[value].ToString());
                }

                cn.Close();
            }
        }

        public static void StoredProceduresToListWith2Key(this Dictionary<string, string> objs, string conString, string storeName, string key1, string key2, string value)
        {
            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(storeName, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(reader[key1].ToString() + "|" + reader[key2].ToString(), reader[value].ToString());
                }

                cn.Close();
            }
        }

        public static void StoredProceduresToListWith2Value(this Dictionary<string, string> objs, string conString, string storeName, string key, string value1, string value2, string SALE_ID = "")
        {
            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(storeName, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (SALE_ID != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@SALE_ID", SALE_ID));
                }

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(reader[key].ToString(), reader[value1].ToString() + " : " + reader[value2].ToString());
                }

                cn.Close();
            }
        }

        public static void WriteLog(string message)
        {
            if (!File.Exists(logFullPath))
            {
                System.IO.Directory.CreateDirectory(logsPath);
                message = DateTime.Now.ToString() + " => " + message;

                File.WriteAllText(logFullPath, message);
            }
            else
            {
                message = Environment.NewLine + DateTime.Now.ToString() + " => " + message;
                System.IO.File.AppendAllText(logFullPath, Environment.NewLine + message);

            }
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }



        /// <summary>
        /// Assumption: Worksheet is in table format with no weird padding or blank column headers.
        /// 
        /// Assertion: Duplicate column names will be aliased by appending a sequence number (eg. Column, Column1, Column2)
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static DataTable GetWorksheetAsDataTable(Microsoft.Office.Interop.Excel.Worksheet worksheet)
        {
            var dt = new DataTable(worksheet.Name);
            dt.Columns.AddRange(GetDataColumns(worksheet).ToArray());
            var headerOffset = 1; //have to skip header row
            var width = dt.Columns.Count;
            var depth = GetTableDepth(worksheet, headerOffset);
            for (var i = 1; i <= depth; i++)
            {
                var row = dt.NewRow();
                for (var j = 1; j <= width; j++)
                {
                    var currentValue = worksheet.Cells[i + headerOffset, j].ToString();

                    //have to decrement b/c excel is 1 based and datatable is 0 based.
                    row[j - 1] = currentValue == null ? null : currentValue.ToString();
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Assumption: There are no null or empty cells in the first column
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private static int GetTableDepth(Microsoft.Office.Interop.Excel.Worksheet worksheet, int headerOffset)
        {
            var i = 1;
            var j = 1;
            var cellValue = worksheet.Cells[i + headerOffset, j].ToString();
            while (cellValue != null)
            {
                i++;
                cellValue = worksheet.Cells[i + headerOffset, j].ToString();
            }

            return i - 1; //subtract one because we're going from rownumber (1 based) to depth (0 based)
        }

        private static IEnumerable<DataColumn> GetDataColumns(Microsoft.Office.Interop.Excel.Worksheet worksheet)
        {
            return GatherColumnNames(worksheet).Select(x => new DataColumn(x));
        }

        private static IEnumerable<string> GatherColumnNames(Microsoft.Office.Interop.Excel.Worksheet worksheet)
        {
            var columns = new List<string>();

            var i = 1;
            var j = 1;
            var columnName = worksheet.Cells[i, j].ToString();
            while (columnName != null)
            {
                columns.Add(GetUniqueColumnName(columns, columnName.ToString()));
                j++;
                columnName = worksheet.Cells[i, j].ToString();
            }

            return columns;
        }

        private static string GetUniqueColumnName(IEnumerable<string> columnNames, string columnName)
        {
            var colName = columnName;
            var i = 1;
            while (columnNames.Contains(colName))
            {
                colName = columnName + i.ToString();
                i++;
            }

            return colName;
        }


        //private void TestReadExcel()
        //{
        //    List<bool> results = new List<bool>();
        //    if (FileUpload1.PostedFile != null)
        //    {
        //        try
        //        {
        //            //string path = string.Concat(Server.MapPath("~/UploadFile/" + FileUpload1.FileName)); //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "~/UploadFile/" + FileUpload1.FileName); //
        //            //FileUpload1.SaveAs(path);

        //            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        //            string path = Server.MapPath("~/App_Data/" + filename);
        //            FileUpload1.PostedFile.SaveAs(path);

        //            DataTable dt = Helper.GetSheetDataAsDataTable(path, "sales_visit_date"); //GetWorksheetAsDataTable(excelWorksheet);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //}

        //public string ConnectionExcel(string FileName, string Header)
        //{
        //    OleDbConnectionStringBuilder Builder = new OleDbConnectionStringBuilder();
        //    if (Path.GetExtension(FileName).ToUpper() == ".XLS")
        //    {
        //        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0
        //        Builder.Provider = "Microsoft.Jet.OLEDB.4.0";
        //        Builder.Add("Extended Properties", string.Format("Excel 8.0;IMEX=1;HDR={0};", Header));
        //    }
        //    else
        //    {
        //        Builder.Provider = "Microsoft.ACE.OLEDB.12.0";
        //        Builder.Add("Extended Properties", string.Format("Excel 8.0;IMEX=1;HDR={0};", Header));
        //    }

        //    Builder.DataSource = FileName;

        //    return Builder.ConnectionString;
        //}
    }
}
