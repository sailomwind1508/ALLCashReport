<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Culture="en-US"%>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


    <script type="text/javascript" src="/crystalreportviewers13/js/crviewer/crv.js"></script>
    <%--<script type="text/javascript" src="/aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/crv.js"></script>--%>

    <script>
        $(function () {
            $("#txtStartDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');;
            $("#txtEndDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');;
            //$("#txtStartDate").datepicker();
            //$("#txtEndDate").datepicker();
        });

        function showValidateMsg() {
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        //function showWait() {
        //    $("#dialog-message2").dialog({
        //        modal: true,
        //        buttons: {
        //            Ok: function () {
        //                $(this).dialog("close");
        //            }
        //        }
        //    });
        //}

        //function hideWait() {
        //    $("#dialog-message2").hide();
        //}

        function setHourglass() {
            document.body.style.cursor = 'wait';
        }

        function removeHourglass() {
            document.body.style.cursor = 'default';
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 79px;
            height: 82px;
        }
        .auto-style2 {
            font-size: x-large;
        }
    </style>
</head>
<body onbeforeunload="setHourglass();" onunload="removeHourglass();">
    <form id="form1" runat="server" onsubmit="return setHourglass();" style="font-family:Tahoma">


        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td align="center" style="width: 100%; height: 100%">

                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width: 15%; height: 100%"></td>
                            <td align="left" style="width: 15%; height: 100%">
                                <div align="left">
                                    <img alt="" class="auto-style1" src="UfoodsImage.jpg" />
                                </div>
                            </td>
                            <td style="width: 35%; height: 100%">
                                <div align="center">
                                    <asp:Label ID="Label5" runat="server" Text="รายงาน All Cash Report" Style="font-weight: 700; text-align: center;" CssClass="auto-style2"></asp:Label>
                                </div>
                            </td>
                            <td style="width: 25%; height: 100%"></td>
                            
                        </tr>
                    </table>


                    <div>
                        <br />
                    </div>
                    <div>
                        <asp:Label ID="Label1" runat="server" Text="Start Date : "></asp:Label><asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                        &nbsp;
                        <asp:Label ID="Label2" runat="server" Text="End Date : "></asp:Label><asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        &nbsp;
                        <asp:Label ID="Label3" runat="server" Text="ภูมิภาค : "></asp:Label><asp:DropDownList ID="ddlArea" runat="server" Height="23px" Width="149px"></asp:DropDownList>
                        &nbsp;
                        <asp:Label ID="Label4" runat="server" Text="ศูนย์ : "></asp:Label><asp:DropDownList ID="ddlProvince" runat="server" Height="19px" Width="150px"></asp:DropDownList>
                        &nbsp;
                        <asp:Button ID="btnReport" runat="server" Height="33px" OnClick="btnReport_Click" Style="font-weight: 700" Text="แสดงรายงาน" Width="96px" ValidationGroup="Report"/>
                        <br />
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please input Start Date!" ControlToValidate="txtStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
                        &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please input End Date!" ControlToValidate="txtEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
                    

                    </div>
                    <div>
                        <br />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 100%; height: 100%">
                    
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ReportSourceID="CrystalReportSource1" />
                   
                    
                    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                        <Report FileName="CrystalReport_Test.rpt">
                        </Report>
                    </CR:CrystalReportSource>
                   
                    
                </td>
            </tr>
            
        </table>

        </div>
        <div id="dialog-message" title="Warning" style="display: none">
            <p>
                <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
                Please enter date time for filter report!
            </p>

        </div>
        <%--<div id="dialog-message2" title="Message" style="display:none">
            <p>
                <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
                Please wait...
            </p>
          
        </div>--%>
    </form>
</body>
</html>
