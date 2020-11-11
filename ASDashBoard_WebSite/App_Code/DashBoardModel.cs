using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DashBoardModel
/// </summary>
public class DashBoardModel
{
    public DateTime CDate { get; set; }
    public string Order { get; set; }
    public string Type { get; set; }
    public decimal Price { get; set; }
    public string Remark { get; set; }

    public DashBoardModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}