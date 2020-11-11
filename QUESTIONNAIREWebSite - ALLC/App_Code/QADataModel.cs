using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for QAData
/// </summary>
public class QADataModel
{
    public string CustomerID { get; set; }
    public string AssessorName { get; set; }
    public string QuestionnaireID { get; set; }
    public string QuestionnaireDetailsID { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Text { get; set; }
    public int Length { get; set; }
    public string Remark { get; set; }

    public int Rank { get; set; }
    public QADataModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}