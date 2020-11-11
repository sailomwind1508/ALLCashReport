using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserLogin
/// </summary>
public class UserLogin
{
    public  string UserName { get; set; }
    public string Password { get; set; }
    public string BranchID { get; set; }

    public string FullName { get; set; }

    public string PhoneNo { get; set; }

    public UserLogin()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}