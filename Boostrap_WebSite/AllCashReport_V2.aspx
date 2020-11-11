<%@ Page Title="ALL CASH Target by Area" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllCashReport_V2.aspx.cs" Inherits="AllCashReport_V2" Debug="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">

        function closeDialog() {
            try {
                setTimeout(function () {
                    //location.reload();
                    $('#wait_dialog').hide();
                    $('#wait_dialog').dialog("close");
                    $('#wait_dialog').dialog("destroy");
                }, 10000);
            } catch (e) {
                alert(e);
            }
        }

        function setHourglass() {
            document.body.style.cursor = 'wait';
        }

        function removeHourglass() {
            document.body.style.cursor = 'default';
        }
    </script>

    <style type="text/css">
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }
    </style>
    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/top-three.png" Width="50px" />
            <asp:Label ID="Label10" runat="server" Text="All Cash Target By Area Report" CssClass="text-white"></asp:Label>
        </div>

        <div class="card-body">

            <div style="width: 100%;">
                <script type="text/javascript">  
                    $(function () {
                        $("#tabs").tabs();
                        $("#MyAccordion").accordion();
                    });
                </script>
                <style type="text/css">
                    #ParentDIV {
                        width: 50%;
                        height: 100%;
                        font-size: 12px;
                    }
                </style>
                <div id="tabs" style="width: 100%; font-family: Verdana,Arial,sans-serif; font-size: 14px;">
                    <ul>
                        <li><a href="#tabs-1">
                            <asp:Image ID="Image18" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;All Cash Target By Area</a></li>
                    </ul>
                    <div id="tabs-1" style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td>
                                    <asp:Label ID="Label1" runat="server" Width="130px" Text="Previous Start Date"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtPrevStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtPrevStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label2" runat="server" Width="130px" Text="Previous End Date"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtPrevEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtPrevEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label3" runat="server" Width="80px" Text="ภูมิภาค"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlArea" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label8" runat="server" Width="80px" Text="Amt Size"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlFilter" runat="server" Height="25px" Width="110px" class=".dropdown-divider"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="white-space: nowrap;">
                                <td>
                                    <asp:Label ID="Label6" runat="server" Width="130px" Text="Current Start Date"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtCurStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtCurStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label7" runat="server" Width="130px" Text="Current End Date"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtCurEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtCurEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label4" runat="server" Width="80px" Text="ศูนย์"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlProvince" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>

                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label9" runat="server" Width="80px" Text="Amt Type"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlAmt" runat="server" Height="25px" Width="110px" class=".dropdown-divider"></asp:DropDownList>

                                    <br />
                                    <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Report">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                                    </asp:LinkButton>

                                </td>
                            </tr>

                        </table>
                    </div>

                </div>

            </div>

        </div>
    </div>
    <div style="position: absolute;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" />
    </div>

    <div id="dialog-message" title="Warning" style="display: none">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Please enter date time for filter report!
        </p>

    </div>

    <script type="text/javascript">
        $("#ContentPlaceHolder1_txtPrevStartDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });//.datepicker('option', 'dateFormat', 'dd/mm/yy');
        $("#ContentPlaceHolder1_txtPrevEndDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });//.datepicker('option', 'dateFormat', 'dd/mm/yy');
        $("#ContentPlaceHolder1_txtCurStartDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });//.datepicker('option', 'dateFormat', 'dd/mm/yy');
        $("#ContentPlaceHolder1_txtCurEndDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });//.datepicker('option', 'dateFormat', 'dd/mm/yy');
    </script>

</asp:Content>

