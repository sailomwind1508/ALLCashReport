using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Page1 : System.Web.UI.Page
{
    CultureInfo culture = new CultureInfo("en-US", false);

    //QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2();
    QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2();
    public List<tbl_Customer> CustomerList = new List<tbl_Customer>();
    public List<tbl_Questionnaire> QuestionnaireList = new List<tbl_Questionnaire>();
    public List<tbl_QuestionnaireDetails> QuestionnaireDetailsList = new List<tbl_QuestionnaireDetails>();
    public List<tbl_CustImage> CustImageList = new List<tbl_CustImage>();
    public List<QADataModel> QAList = new List<QADataModel>();
    public List<tbl_CustMaster> CustMasterList = new List<tbl_CustMaster>();

    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string conString17 = ConfigurationManager.ConnectionStrings["QUESTIONNAIRE_TESTEntities2"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        var requestTarget = this.Request["__EVENTTARGET"];
        string customerID = "";
        customerID = this.Request.QueryString["CustoemerID"];
        if (!string.IsNullOrEmpty(customerID))
        ViewState["QA_CustomerID"] = customerID;
        else
            ViewState["QA_CustomerID"] = null;

        if (!IsPostBack)
        {
            InitPage();
        }

        PrepareQAData();

        if (this.Request["__EVENTARGUMENT"] != null)
        {
            try
            {
                var paramList = this.Request["__EVENTARGUMENT"];
                var splData = paramList.Split(',').ToList();
                if (!string.IsNullOrEmpty(splData[0]))
                {
                    QAList = new List<QADataModel>();

                    string _assessorName = "";
                    string _custID = "";
                    string _remark = "";
                    foreach (var item in splData)
                    {
                        int len = item.ToString().Split('|')[2].Length;
                        var rdoGroup = item.ToString().Split('|')[2].Substring(4, len - 4).ToString();
                        var rdoID = item.ToString().Split('|')[0].ToString();
                        var latlong = item.ToString().Split('|')[1].ToString();
                        var text = item.ToString().Split('|')[3].ToString();

                        if (rdoID == "AssessorName")
                            _assessorName = text;
                        if (rdoID.Contains("CustID"))
                            _custID = text;
                        if (rdoID == "remark")
                            _remark = text;
                    }

                    if (splData.Count == 3)
                    {
                        var latlong = splData[0].ToString().Split('|')[1].ToString();

                        var qa = new QADataModel();

                        qa.CustomerID = _custID;
                        qa.AssessorName = _assessorName;
                        qa.QuestionnaireID = null;
                        qa.QuestionnaireDetailsID = null;

                        if (!string.IsNullOrEmpty(latlong))
                        {
                            qa.Latitude = latlong.Split(':')[0];
                            qa.Longitude = latlong.Split(':')[1];
                        }

                        qa.Text = null;
                        qa.Length = 0;
                        qa.Remark = _remark;

                        QAList.Add(qa);
                    }
                    else
                    {
                        foreach (var item in splData)
                        {
                            int len = item.ToString().Split('|')[2].Length;
                            var rdoGroup = item.ToString().Split('|')[2].Substring(4, len - 4).ToString();
                            var rdoID = item.ToString().Split('|')[0].ToString();
                            var latlong = item.ToString().Split('|')[1].ToString();
                            var text = item.ToString().Split('|')[3].ToString();

                            var qa = new QADataModel();
                            if (rdoID != "AssessorName" && !rdoID.Contains("CustID") && rdoID != "remark")
                            {
                                qa.CustomerID = _custID;
                                qa.AssessorName = _assessorName;
                                qa.QuestionnaireID = rdoGroup;
                                qa.QuestionnaireDetailsID = rdoID;

                                if (!string.IsNullOrEmpty(latlong))
                                {
                                    qa.Latitude = latlong.Split(':')[0];
                                    qa.Longitude = latlong.Split(':')[1];
                                }

                                qa.Text = text;
                                qa.Length = len;
                                qa.Remark = _remark;

                                var chkRdoTxt = QAList.FirstOrDefault(x => x.QuestionnaireID == rdoGroup && x.QuestionnaireDetailsID == rdoID);
                                if (chkRdoTxt != null)
                                {
                                    QAList.Remove(chkRdoTxt);
                                }

                                QAList.Add(qa);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string message = "เกิดข้อผิดพลาด กรุณาลองใหม่อีกครั้ง!!! : (Page_Load)" + ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
            }
        }

        if (requestTarget != null && !string.IsNullOrEmpty(requestTarget))
        {
            if (requestTarget == "upload")
                UploadData();
        }
    }

    private void CreateDiv(HtmlGenericControl div, string divId, string _class, string _role = "", string innerHtml = "")
    {
        HtmlGenericControl _div = new HtmlGenericControl("div");
        _div.Attributes.Add("id", divId);
        if (!string.IsNullOrEmpty(_role))
        {
            _div.Attributes.Add("role", _role);
        }
        div.Controls.Add(_div);
    }

    private void PrepareQAData()
    {
        CustomerList = new List<tbl_Customer>();
        QuestionnaireList = new List<tbl_Questionnaire>();
        QuestionnaireDetailsList = new List<tbl_QuestionnaireDetails>();

        CustomerList = db.tbl_Customer.ToList();
        QuestionnaireList = db.tbl_Questionnaire.Where(x => x.QAType == ddlCustType.SelectedValue).ToList();
        foreach (var item in QuestionnaireList)
        {
            var qaDT = db.tbl_QuestionnaireDetails.Where(x => x.QuestionnaireID == item.QuestionnaireID).ToList();
            QuestionnaireDetailsList.AddRange(qaDT);
        }
    }

    protected void ddlCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        PrepareQAData();

        string custType = ddlCustType.SelectedValue;

        if (custType == "WS")
        {
            div_ws.Visible = true;
            div_allc.Visible = false;
        }
        else if (custType == "ALLC")
        {
            div_ws.Visible = false;
            div_allc.Visible = true;
        }
    }

    private void InitPage()
    {
        try
        {
            Dictionary<string, string> custTypeList = new Dictionary<string, string>();
            custTypeList.Add("WS", "ลูกค้า WS");
            custTypeList.Add("ALLC", "ลูกค้า ALLC");
            ddlCustType.DataSource = custTypeList;
            ddlCustType.DataTextField = "value";
            ddlCustType.DataValueField = "key";
            ddlCustType.DataBind();

            DataTable _dt = new DataTable();
            _dt = ExecuteProcedureToTable(conString, "proc_qa_province_get_all", CommandType.StoredProcedure, null);

            DataRow _sRow = _dt.NewRow();
            _sRow["BranchID"] = "-1";
            _sRow["BranchName"] = "---เลือกศูนย์---";
            _dt.Rows.InsertAt(_sRow, 0);

            if (_dt != null && _dt.Rows.Count > 0)
            {
                ddlProvince.DataSource = _dt;
                ddlProvince.DataTextField = "BranchName";
                ddlProvince.DataValueField = "BranchID";
                ddlProvince.DataBind();
            }

            if (ViewState["QA_CustomerID"] != null && !string.IsNullOrEmpty(ViewState["QA_CustomerID"].ToString()))
            {
                string _custID = ViewState["QA_CustomerID"].ToString();
                if (_custID.Length > 7) //ALLC
                {
                    PrepareQAData();

                    ddlCustType.SelectedValue = "ALLC";

                    div_ws.Visible = false;
                    div_allc.Visible = true;

                    divLink.Visible = false;

                    DataTable _cust_dt = new DataTable();
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("CustomerID", _custID);
                    _cust_dt = ExecuteProcedureToTable(conString, "proc_qa_customer_get_all", CommandType.StoredProcedure, p);

                    if (_cust_dt != null && _cust_dt.Rows.Count > 0)
                    {
                        ddlCustomer.DataSource = _cust_dt;
                        ddlCustomer.DataTextField = "CustName";
                        ddlCustomer.DataValueField = "CustomerID";
                        ddlCustomer.DataBind();
                    }
                }
                else //WS
                {
                    ddlCustType.SelectedValue = "WS";
                    div_ws.Visible = true;
                    div_allc.Visible = false;

                    DataTable _cust_dt = new DataTable();
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("CustomerID", _custID);
                    _cust_dt = ExecuteProcedureToTable(conString, "proc_qa_customer_get_all", CommandType.StoredProcedure, p);

                    if (_cust_dt != null && _cust_dt.Rows.Count > 0)
                    {
                        CustID.Text = _cust_dt.Rows[0]["CustomerID"].ToString();
                        lblCustWSName.Text = _cust_dt.Rows[0]["CustName"].ToString();
                    }
                    else
                        lblCustWSName.Text = "";
                }                
                //string _customerID = ViewState["QA_CustomerID"].ToString();
                //Dictionary<string, object> t_prmt = new Dictionary<string, object>();
                //t_prmt.Add("CustomerID", _customerID);

                //DataTable dt_c = new DataTable();

                //string query_c = "";
                //query_c = "SELECT CustomerID, CustomerID + ':' + CustName AS 'CustName' FROM dbo.tblCustomerAM2 WHERE CustomerID = @CustomerID";
                //dt_c = ExecuteProcedureToTable(conString, query_c, CommandType.Text, t_prmt);

                //if (dt_c != null && dt_c.Rows.Count > 0)
                //{
                //    CustID.Text = dt_c.Rows[0]["CustName"].ToString();
                //}
                //else
                //{
                //    query_c = "SELECT CustomerID, CustomerID + ':' + CustName AS 'CustName' FROM dbo.tblCustomerWS WHERE CustomerID = @CustomerID";
                //    dt_c = ExecuteProcedureToTable(conString, query_c, CommandType.Text, t_prmt);

                //    if (dt_c != null && dt_c.Rows.Count > 0)
                //    {
                //        CustID.Text = dt_c.Rows[0]["CustName"].ToString();
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            string message = "เกิดข้อผิดพลาด กรุณาลองใหม่อีกครั้ง!!! : " + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
        }
    }

    protected void btnSearchCust_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProvince.SelectedValue != "-1")
            BindVanID();
        else
        {
            BindVanID(true);
            BindSaleArea(true);
            BindCustomer(true);
        }
    }

    protected void ddlVanID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProvince.SelectedValue != "-1" && ddlVanID.SelectedValue != "-1")
            BindSaleArea();
        else
        {
            BindSaleArea(true);
            BindCustomer(true);
        }
    }

    protected void ddlSalAreaID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProvince.SelectedValue != "-1" && ddlVanID.SelectedValue != "-1" && ddlSalAreaID.SelectedValue != "-1")
            BindCustomer();
        else
            BindCustomer(true);
    }

    private void BindVanID(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlProvince.SelectedValue);
        _dt = ExecuteProcedureToTable(conString, "proc_qa_van_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["van_id"] = "-1";
        _sRow["VanName"] = "---เลือกแวน---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlVanID.DataSource = _dt;
            ddlVanID.DataTextField = "VanName";
            ddlVanID.DataValueField = "van_id";
            ddlVanID.DataBind();
        }
    }

    private void BindSaleArea(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlProvince.SelectedValue);
        p.Add("van_id", ddlVanID.SelectedValue);
        _dt = ExecuteProcedureToTable(conString, "proc_qa_salearea_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["SalAreaID"] = "-1";
        _sRow["SalAreaName"] = "---เลือกตลาด---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlSalAreaID.DataSource = _dt;
            ddlSalAreaID.DataTextField = "SalAreaName";
            ddlSalAreaID.DataValueField = "SalAreaID";
            ddlSalAreaID.DataBind();
        }
    }

    private void BindCustomer(bool isClear = false)
    {
        DataTable _dt = new DataTable();
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("BranchID", ddlProvince.SelectedValue);
        p.Add("van_id", ddlVanID.SelectedValue);
        p.Add("SalAreaID", ddlSalAreaID.SelectedValue);
        _dt = ExecuteProcedureToTable(conString, "proc_qa_customer_get", CommandType.StoredProcedure, p);

        if (isClear)
            _dt.Clear();

        DataRow _sRow = _dt.NewRow();
        _sRow["CustomerID"] = "-1";
        _sRow["CustName"] = "---เลือกร้านค้า---";
        _dt.Rows.InsertAt(_sRow, 0);

        if (_dt != null && _dt.Rows.Count > 0)
        {
            ddlCustomer.DataSource = _dt;
            ddlCustomer.DataTextField = "CustName";
            ddlCustomer.DataValueField = "CustomerID";
            ddlCustomer.DataBind();
        }
    }

    private void UploadData()
    {
        int result = 0;
        try
        {
            string customerID = SaveCustomer();

            if (!string.IsNullOrEmpty(customerID))
            {
                result = 1;

                if (!chkNotActivate.Checked)
                {
                    int ret = SaveCustQA(customerID);

                    if (ret == 1)
                    {
                        result = 1;

                        try
                        {
                            List<int> ListImageID = new List<int>();
                            ListImageID.AddRange(UploadImage(customerID));

                            if (ListImageID.Any(x => x == 1))
                                result = 1;
                            else if (ListImageID.All(x => x == -1))
                            {
                                string message = "ไม่สามารถส่งรูปได้ กรุณาลองใหม่อีกครั้ง!!!";
                                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = "ไม่สามารถส่งรูปได้ กรุณาลองใหม่อีกครั้ง!!! : (UploadData)" + ex.Message;
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
                        }
                        
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!! : " + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);

            result = 0;
        }

        if (result == 1)
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('ส่งข้อมูลเรียบร้อยแล้ว!!!')", true);
        else
        {
            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!!";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
        }
    }

    private void Search()
    {
        try
        {
            string _custID = "";
            if (ddlCustType.SelectedValue == "WS")
                _custID = CustID.Text;
            else
                _custID = ddlCustomer.SelectedValue;

            var custList = db.tbl_Customer.Where(x => x.CustomerID == _custID).ToList();
            if (custList.Count > 0)
            {
                var _c = custList.First();
                CustID.Text = _c.CustomerID;
                latlong.Text = _c.Latitude + ":" + _c.Longitude;
                AssessorName.Text = _c.AssessorName;

                DataTable _cust_dt = new DataTable();
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("CustomerID", _c.CustomerID);
                _cust_dt = ExecuteProcedureToTable(conString, "proc_qa_customer_get_all", CommandType.StoredProcedure, p);

                if (_cust_dt != null && _cust_dt.Rows.Count > 0)
                {
                    lblCustWSName.Text = _cust_dt.Rows[0]["CustName"].ToString();
                }
                else
                    lblCustWSName.Text = "";
            }
            else
            {
                latlong.Text = "";
                AssessorName.Text = "";
                lblCustWSName.Text = "";
                string message = "ไม่พบข้อมูลร้านค้าในระบบ กรุณาลองใหม่อีกครั้ง!!!";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            string message = "เกิดข้อผิดพลาด กรุณาลองใหม่อีกครั้ง!!! : " + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
        }

    }

    private int SaveCustQA(string customerID)
    {
        List<int> retList = new List<int>();
        int ret = -1;
        try
        {
            var cDate = DateTime.Now;

            List<tbl_CustQA> qaList = new List<tbl_CustQA>();
            var qaDTList = QuestionnaireDetailsList;
            string _customerID = customerID;

            using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
            {
                var _tbl_CustQAList = db.tbl_CustQA.ToList();
                if (_tbl_CustQAList.Any(x => x.CustomerID == _customerID))
                {
                    var custQAList = _tbl_CustQAList.Where(x => x.CustomerID == _customerID).ToList();
                    foreach (tbl_CustQA item in custQAList)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                        db.tbl_CustQA.Remove(item);
                        db.SaveChanges();
                    }
                }
            }

            foreach (var item in qaDTList)
            {
                var _q = QuestionnaireList.FirstOrDefault(x => x.QuestionnaireID.Value.ToString() == item.QuestionnaireID.Value.ToString());
                if (_q != null)
                {
                    var inputQA = QAList.FirstOrDefault(x => x.QuestionnaireDetailsID == item.QuestionnaireDetailsID.ToString());
                    if (inputQA != null)
                    {
                        tbl_CustQA q = new tbl_CustQA();
                        q.CustomerID = _customerID;
                        q.QuestionnaireID = item.QuestionnaireID;
                        q.QuestionnaireDetailsID = item.QuestionnaireDetailsID;

                        if (_q.Pattern == "radio")
                        {
                            q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "multi-radio")
                        {
                            q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "multi-checkbox")
                        {
                            q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "checkbox")
                        {
                            q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "text")
                        {
                            q.Text = inputQA.Text;
                        }
                        else if (_q.Pattern == "radio-text")
                        {
                            q.Text = inputQA.Text;
                            if (!string.IsNullOrEmpty(inputQA.Text))
                                q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "checkbox-text")
                        {
                            q.Text = inputQA.Text;
                            if (!string.IsNullOrEmpty(inputQA.Text))
                                q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "text-area")
                        {
                            q.Text = inputQA.Text;
                        }

                        q.Score = 0;
                        q.Remark = "";
                        q.UpdateDate = cDate;

                        qaList.Add(q);
                    }
                }
            }

            using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
            {
                foreach (var qa in qaList)
                {
                    db.tbl_CustQA.Attach(qa);
                    db.tbl_CustQA.Add(qa);
                    retList.Add(db.SaveChanges());
                }
            }
        }
        catch (Exception ex)
        {
            ret = -1;
            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!! : (SaveCustQA)" + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
        }

        ret = retList.All(x => x != -1) ? 1 : -1;

        return ret;
    }

    private string SaveCustomer()
    {
        string ret = "";
        try
        {
            var cDate = DateTime.Now;
            string _custID = "";
            if (ddlCustType.SelectedValue == "WS")
                _custID = CustID.Text;
            else
                _custID = ddlCustomer.SelectedValue;

            var item = QAList[0];

            CustomerList = db.tbl_Customer.ToList();
            var cust = CustomerList.FirstOrDefault(x => x.CustomerID == _custID && x.CustomerType == ddlCustType.SelectedValue);
            if (cust != null)
            {
                cust.CustomerID = _custID;
                cust.CustomerType = ddlCustType.SelectedValue;
                cust.AssessorName = AssessorName.Text;
                cust.Latitude = item.Latitude;
                cust.Longitude = item.Longitude;
                cust.Remark = item.Remark;
                cust.UpdateDate = cDate;

                using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
                {
                    db.Entry(cust).State = EntityState.Modified;

                    int _ret = -1;
                    _ret = db.SaveChanges();
                    if (_ret == 1)
                        ret = _custID;
                }
            }
            else
            {
                var newCust = new tbl_Customer();
                newCust.CustomerID = _custID;
                newCust.CustomerType = ddlCustType.SelectedValue;
                newCust.AssessorName = AssessorName.Text;
                newCust.Latitude = item.Latitude;
                newCust.Longitude = item.Longitude;
                newCust.Remark = item.Remark;
                newCust.UpdateDate = cDate;
                using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
                {
                    db.tbl_Customer.Attach(newCust);
                    db.tbl_Customer.Add(newCust);

                    int _ret = -1;
                    _ret = db.SaveChanges();
                    if (_ret == 1)
                        ret = _custID;
                }
            }
        }
        catch (Exception ex)
        {
            ret = "";
            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!! : (SaveCustomer)" + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
        }
        return ret;
    }

    private List<int> UploadImage(string customerID)
    {
        List<int> results = new List<int>();
        try
        {
            if (FileUpload1.HasFile || FileUpload2.HasFile || FileUpload3.HasFile || FileUpload4.HasFile)
            {
                using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
                {
                    var _tbl_CustImageList = db.tbl_CustImage.ToList();
                    if (_tbl_CustImageList.Any(x => x.CustomerID == customerID))
                    {
                        var custImageList = _tbl_CustImageList.Where(x => x.CustomerID == customerID).ToList();
                        foreach (tbl_CustImage item in custImageList)
                        {
                            db.Entry(item).State = EntityState.Deleted;
                            db.tbl_CustImage.Remove(item);
                            db.SaveChanges();
                        }
                    }
                }
            }

            int ret1 = UploadImg(FileUpload1, customerID, 1);
            results.Add(ret1);

            int ret2 = UploadImg(FileUpload2, customerID, 2);
            results.Add(ret2);

            int ret3 = UploadImg(FileUpload3, customerID, 3);
            results.Add(ret3);

            int ret4 = UploadImg(FileUpload4, customerID, 4);
            results.Add(ret4);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return results;
    }

    private int UploadImg(FileUpload file, string customerID, int picNum)
    {
        int ret = -1;
        try
        {
            string path = "";

            if (file.HasFile)
            {
                var _cDate = DateTime.Now.ToString("yyyyMMdd", culture);
                string imgName = customerID + _cDate + picNum.ToString() + ".jpg"; //file.FileName.Split('.')[0];

                string _path = AppDomain.CurrentDomain.BaseDirectory + "CustomerImages";
                string filename = Path.GetFileName(file.FileName);
                file.SaveAs(Path.Combine(_path, imgName));// filename));
                path = _path + "\\" + imgName; // file.FileName;

                //try
                //{
                //    string url = Path.Combine(_path, imgName);// "http://center2.blogdns.net:6996/QuestionnaireTestApp/" + imgName;//bdccb931-1599-4eba-817d-6b3968ab90ee.jpg";
                //    WebClient webClient = new WebClient();
                //    string clintPath = Server.MapPath("Images"); // Create a folder named 'Images' in your root directory
                //    string fileName = Path.GetFileName(url);
                //    webClient.DownloadFile(url, clintPath + "\\" + fileName);
                //}
                //catch (Exception ex)
                //{

                //}
                

                //if (file.PostedFile != null)
                //{
                //    // Check the extension of image  
                //    string extension = Path.GetExtension(file.FileName);
                //    if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg")
                //    {
                //        var cDate = DateTime.Now.ToString("yyyyMMdd", culture);
                //        string imgName = customerID + cDate + picNum.ToString(); //file.FileName.Split('.')[0];

                //        //Path to store uploaded files on server - make sure your paths are unique
                //        string filePath = "~/CustomerImages/" + imgName;

                //        string thumbPath = "~/CustomerImages/" + imgName + "_thumb";

                //        if (File.Exists(Server.MapPath(filePath + ".jpg")))
                //            System.IO.File.Delete(Server.MapPath(filePath + ".jpg"));

                //        // Check file size (mustn't be 0)
                //        HttpPostedFile myFile = file.PostedFile;
                //        int nFileLen = myFile.ContentLength;

                //        // Read file into a data stream
                //        byte[] myData = new Byte[nFileLen];
                //        myFile.InputStream.Read(myData, 0, nFileLen);
                //        myFile.InputStream.Dispose();

                //        // Save the stream to disk as temporary file. 
                //        // make sure the path is unique!
                //        System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(filePath + "_temp.jpg"), System.IO.FileMode.Create);
                //        newFile.Write(myData, 0, myData.Length);

                //        // run ALL the image optimisations you want here..... 
                //        // make sure your paths are unique
                //        // you can use these booleans later 
                //        // if you need the results for your own labels or so.
                //        // don't call the function after the file has been closed.
                //        bool success = ResizeImageAndUpload(newFile, filePath, 768, 1024);
                //        path = Server.MapPath(filePath + ".jpg");
                //        // tidy up and delete the temp file.
                //        newFile.Close();

                //        // don't delete if you want to keep original files on server 
                //        // (in this example its for a real estate website
                //        // the company might want the large originals 
                //        // for a printing module later.
                //        System.IO.File.Delete(Server.MapPath(filePath + "_temp.jpg"));
                //    }

                //    //if (file.PostedFile.ContentLength > 0)//Check if image is greater than 5MB
                //    //{
                //    //    path = string.Concat(Server.MapPath("~/CustomerImages/" + file.FileName));
                //    //    file.SaveAs(path);
                //    //}
                //}

                if (!string.IsNullOrEmpty(path))
                {
                    var cDate = DateTime.Now;
                    using (QUESTIONNAIRE_TESTEntities2 db = new QUESTIONNAIRE_TESTEntities2())
                    {
                        var img = new tbl_CustImage();
                        img.CustomerID = customerID;
                        img.CustImagePath = path;
                        img.UpdateDate = cDate;

                        db.tbl_CustImage.Attach(img);
                        db.tbl_CustImage.Add(img);
                        ret = db.SaveChanges();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return ret;
    }

    public bool ResizeImageAndUpload(System.IO.FileStream newFile, string folderPathAndFilenameNoExtension, double maxHeight, double maxWidth)
    {
        try
        {
            // Declare variable for the conversion
            float ratio;

            // Create variable to hold the image
            System.Drawing.Image thisImage = System.Drawing.Image.FromStream(newFile);

            // Get height and width of current image
            int width = (int)thisImage.Width;
            int height = (int)thisImage.Height;

            // Ratio and conversion for new size
            if (width > maxWidth)
            {
                ratio = (float)width / (float)maxWidth;
                width = (int)(width / ratio);
                height = (int)(height / ratio);
            }

            // Ratio and conversion for new size
            if (height > maxHeight)
            {
                ratio = (float)height / (float)maxHeight;
                height = (int)(height / ratio);
                width = (int)(width / ratio);
            }

            // Create "blank" image for drawing new image
            Bitmap outImage = new Bitmap(width, height);
            Graphics outGraphics = Graphics.FromImage(outImage);
            SolidBrush sb = new SolidBrush(System.Drawing.Color.White);

            // Fill "blank" with new sized image
            outGraphics.FillRectangle(sb, 0, 0, outImage.Width, outImage.Height);
            outGraphics.DrawImage(thisImage, 0, 0, outImage.Width, outImage.Height);
            sb.Dispose();
            outGraphics.Dispose();
            thisImage.Dispose();

            // Save new image as jpg
            outImage.Save(Server.MapPath(folderPathAndFilenameNoExtension + ".jpg"),
            System.Drawing.Imaging.ImageFormat.Jpeg);
            outImage.Dispose();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void BindDropdownList(DropDownList ddl, DataTable objs, string text, string value)
    {
        ddl.Items.Clear();
        ddl.DataSource = objs;
        ddl.DataTextField = text;
        ddl.DataValueField = value;
        ddl.DataBind();
    }

    private int ExecuteProcedureOBJ(string conString, string cmdText, Dictionary<string, object> parameters = null)
    {
        int imageID = -1;
        try
        {
            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand(cmdText, cn);

                cmd.CommandType = System.Data.CommandType.Text;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                    }
                }

                cmd.CommandTimeout = 0;

                imageID = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
        }
        catch (Exception ex)
        {
            imageID = -1;
            throw ex;
        }

        return imageID;
    }

    private DataTable ExecuteProcedureToTable(string conString, string procedureName, CommandType type, Dictionary<string, object> parameters = null)
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(conString))
            {
                cn.Open();

                SqlCommand cmd = null;

                cmd = new SqlCommand(procedureName, cn);

                cmd.CommandType = type;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter("@" + item.Key, item.Value));
                    }
                }

                cmd.CommandTimeout = 0;

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

            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!! : " + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
            return null;
        }
    }

}