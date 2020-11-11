using Newtonsoft.Json;
using Report_ClassLibrary;
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

    QUESTIONNAIRE_ALLCEntities db = new QUESTIONNAIRE_ALLCEntities();
    public List<tbl_Customer> CustomerList = new List<tbl_Customer>();
    public List<tbl_Questionnaire> QuestionnaireList = new List<tbl_Questionnaire>();
    public List<tbl_QuestionnaireDetails> QuestionnaireDetailsList = new List<tbl_QuestionnaireDetails>();
    public List<tbl_CustImage> CustImageList = new List<tbl_CustImage>();
    public List<QADataModel> QAList = new List<QADataModel>();
    public List<tbl_CustMaster> CustMasterList = new List<tbl_CustMaster>();

    string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string conString12 = ConfigurationManager.ConnectionStrings["QUESTIONNAIRE_ALLCEntities"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        var requestTarget = this.Request["__EVENTTARGET"];
        string customerID = "";
        customerID = this.Request.QueryString["CustomerID"];
        if (!string.IsNullOrEmpty(customerID))
            ViewState["QA_CustomerID"] = customerID;
        else
            ViewState["QA_CustomerID"] = null;

        //var verifyLogin = Session["QALogin"];
        //if (verifyLogin == null)
        //{
        //    Session.Clear();
        //    Session.RemoveAll();
        //    Session.Abandon();

        //    Session["QALogin"] = null;

        //    Response.Redirect("~/Login.aspx");
        //}

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

                    List<QADataModel> rankList = new List<QADataModel>();
                    foreach (var item in splData)
                    {
                        var qa = new QADataModel();
                        var rdoID = item.ToString().Split('|')[0].ToString();
                        var latlong = item.ToString().Split('|')[1].ToString();
                        var text = item.ToString().Split('|')[3].ToString();

                        if (rdoID.Contains("rank"))
                        {
                            qa.QuestionnaireID = rdoID.Substring(4, rdoID.Length - 4).ToString();
                            qa.Rank = Convert.ToInt32(text);
                            rankList.Add(qa);
                        }
                    }

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

                            if (rankList.Any(x => x.QuestionnaireID == qa.QuestionnaireID))
                            {
                                var _rankData = rankList.First(x => x.QuestionnaireID == qa.QuestionnaireID);
                                qa.Rank = _rankData.Rank;
                            }

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
            qaDT.ForEach(x => 
            {
                if (item.Pattern == "checkbox-text")
                {
                    if (x.Question != "อื่น ๆ ")
                    {
                        x.Question = x.Question + " => อันดับที่: ";
                    }
                }
                
            
            });
            QuestionnaireDetailsList.AddRange(qaDT);
        }
    }

    protected void ddlCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        PrepareQAData();
    }

    private void InitPage()
    {
        try
        {
            Dictionary<string, string> custTypeList = new Dictionary<string, string>();
            //custTypeList.Add("WS", "ลูกค้า WS");
            custTypeList.Add("ALLC", "ลูกค้า ALLC");
            ddlCustType.DataSource = custTypeList;
            ddlCustType.DataTextField = "value";
            ddlCustType.DataValueField = "key";
            ddlCustType.DataBind();

            if (ViewState["QA_CustomerID"] != null && !string.IsNullOrEmpty(ViewState["QA_CustomerID"].ToString()))
            {
                string _custID = ViewState["QA_CustomerID"].ToString();
                if (_custID.Length > 7) //ALLC
                {
                    PrepareQAData();

                    ddlCustType.SelectedValue = "ALLC";

                    DataTable _cust_dt = new DataTable();
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    p.Add("CustomerID", _custID);
                    _cust_dt = Helper.ExecuteProcedureToTable(conString, "proc_qa_customer_get_all", CommandType.StoredProcedure, p);

                    if (_cust_dt != null && _cust_dt.Rows.Count > 0)
                    {
                        ddlCustomer.DataSource = _cust_dt;
                        ddlCustomer.DataTextField = "CustName";
                        ddlCustomer.DataValueField = "CustomerID";
                        ddlCustomer.DataBind();
                    }

                    DataTable user = new DataTable();
                    Dictionary<string, object> p2 = new Dictionary<string, object>();
                    p2.Add("CustomerID", _custID);
                    user = Helper.ExecuteProcedureToTable(conString, "proc_qa_customer_get_user", CommandType.StoredProcedure, p2);
                    if (user != null && user.Rows.Count > 0)
                    {
                        AssessorName.Text = user.Rows[0]["VAN_ID"].ToString();
                    }
                }
                
            }
            else
            {
                string _custID = "";
                if (ViewState["QA_CustomerID"] != null)
                {
                    ViewState["QA_CustomerID"].ToString();
                }

                string message = "ไม่พบข้อมูลร้านค้านี้ในระบบ : " + _custID;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
            }
        }
        catch (Exception ex)
        {
            string message = "เกิดข้อผิดพลาด กรุณาลองใหม่อีกครั้ง!!! : " + ex.Message;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('" + message + "')", true);
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
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowSuccess('ส่งข้อมูลเรียบร้อยแล้ว!!!')", true);
            //ddlVanID.SelectedIndex = 0;
        }
        else
        {
            string message = "บันทึกไม่สำเร็จ กรุณาลองใหม่อีกครั้ง!!!";
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

            using (QUESTIONNAIRE_ALLCEntities db = new QUESTIONNAIRE_ALLCEntities())
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
                        q.Score = 0;

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
                        else if (_q.Pattern == "number")
                        {
                            q.Text = inputQA.Text;
                        }
                        else if (_q.Pattern == "text-score")
                        {
                            q.Text = inputQA.Text;
                            q.Score = inputQA.Rank;
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
                            q.SelectFlag = true;
                        }
                        else if (_q.Pattern == "text-area")
                        {
                            q.Text = inputQA.Text;
                        }

                        q.Remark = "";
                        q.UpdateDate = cDate;

                        if (!string.IsNullOrEmpty(q.CustomerID))
                            qaList.Add(q);
                    }
                }
            }

            using (QUESTIONNAIRE_ALLCEntities db = new QUESTIONNAIRE_ALLCEntities())
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
            string _custID = ddlCustomer.SelectedValue;

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

                using (QUESTIONNAIRE_ALLCEntities db = new QUESTIONNAIRE_ALLCEntities())
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
                using (QUESTIONNAIRE_ALLCEntities db = new QUESTIONNAIRE_ALLCEntities())
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


    protected void rdoVanSearch_CheckedChanged(object sender, EventArgs e)
    {
        //divLink.Visible = true;
        //div_ws.Visible = false;
    }

    protected void rdoMSearch_CheckedChanged(object sender, EventArgs e)
    {
        //divLink.Visible = false;
        //div_ws.Visible = true;
    }
}