<%@ Page Title="ALL CASH Target by Area" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllCashReport_V2.aspx.cs" Inherits="AllCashReport_V2" Debug="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script type="text/javascript">


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

        .auto-style3 {
            width: 245px;
            text-align: right;
        }

        .auto-style4 {
            width: 245px;
            text-align: right;
        }

        .header {
            position: absolute;
            top: 100px;
            left: 0px;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line1_left {
            position: absolute;
            top: 150px;
            left: 10%;
            width: 20%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 30%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 50%;
            width: 20%;
            height: 30px;
            text-align: left;
        }

        .line1_right2 {
            position: absolute;
            top: 150px;
            left: 70%;
            width: 30%;
            height: 30px;
            text-align: left;
        }

        .line2_left {
            position: absolute;
            top: 190px;
            left: 10%;
            width: 20%;
            height: 30px;
            text-align: right;
        }

        .line2_center {
            position: absolute;
            top: 190px;
            left: 30%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line2_right {
            position: absolute;
            top: 190px;
            left: 50%;
            width: 20%;
            height: 30px;
            text-align: left;
        }

        .line2_right2 {
            position: absolute;
            top: 190px;
            left: 70%;
            width: 30%;
            height: 30px;
            text-align: left;
        }

        .line3_btn {
            position: absolute;
            top: 230px;
            left: 50%;
            width: 50%;
            height: 30px;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 280px;
            left: 12%;
            width: 80%;
            height: 600px;
            text-align: center;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
            .header {
                position: absolute;
                top: 100px;
                left: 8%;
                width: 100%;
                height: 50px;
                font-size: x-large;
                font-weight: 700;
                text-align: center;
            }

            .line1_left {
                position: absolute;
                top: 150px;
                left: 20%;
                width: 20%;
                height: 30px;
                text-align: right;
            }

            .line1_center {
                position: absolute;
                top: 150px;
                left: 40%;
                width: 20%;
                height: 30px;
                text-align: center;
            }

            .line1_right {
                position: absolute;
                top: 150px;
                left: 60%;
                width: 20%;
                height: 30px;
                text-align: left;
            }

            .line1_right2 {
                position: absolute;
                top: 150px;
                left: 80%;
                width: 30%;
                height: 30px;
                text-align: left;
            }

            .line2_left {
                position: absolute;
                top: 190px;
                left: 20%;
                width: 20%;
                height: 30px;
                text-align: right;
            }

            .line2_center {
                position: absolute;
                top: 190px;
                left: 40%;
                width: 20%;
                height: 30px;
                text-align: center;
            }

            .line2_right {
                position: absolute;
                top: 190px;
                left: 60%;
                width: 20%;
                height: 30px;
                text-align: left;
            }

            .line2_right2 {
                position: absolute;
                top: 190px;
                left: 80%;
                width: 30%;
                height: 30px;
                text-align: left;
            }

            .line3_btn {
                position: absolute;
                top: 230px;
                left: 50%;
                width: 50%;
                height: 30px;
                text-align: center;
            }

            .line4_footer {
                position: absolute;
                top: 280px;
                left: 18%;
                width: 80%;
                height: 600px;
                text-align: center;
            }
        }
    </style>

    <div class="header">

        <asp:Label ID="Label5" runat="server" Text="All Cash Target By Area Report"></asp:Label>
    </div>

    <div class="line1_left">
        <asp:Label ID="Label1" runat="server" Text="Previous Start Date : "></asp:Label>
        <asp:TextBox ID="txtPrevStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtPrevStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line1_center">
        <asp:Label ID="Label2" runat="server" Text="Previous End Date : "></asp:Label>
        <asp:TextBox ID="txtPrevEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtPrevEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line1_right">
        <asp:Label ID="Label3" runat="server" Text="ภูมิภาค : "></asp:Label>
        <asp:DropDownList ID="ddlArea" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>

    </div>
    <div class="line1_right2">
        <asp:Label ID="Label8" runat="server" Text="Amt Size : "></asp:Label>
        <asp:DropDownList ID="ddlFilter" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>
    </div>



    <div class="line2_left">
        <asp:Label ID="Label6" runat="server" Text="Current Start Date : "></asp:Label>
        <asp:TextBox ID="txtCurStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtCurStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line2_center">
        <asp:Label ID="Label7" runat="server" Text="Current End Date : "></asp:Label>
        <asp:TextBox ID="txtCurEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtCurEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

    </div>
    <div class="line2_right">
        <asp:Label ID="Label4" runat="server" Text="ศูนย์ : "></asp:Label>
        <asp:DropDownList ID="ddlProvince" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>

    </div>
    <div class="line2_right2">
        <asp:Label ID="Label9" runat="server" Text="Amt Type : "></asp:Label>
        <asp:DropDownList ID="ddlAmt" runat="server" Height="25px" Width="150px" class=".dropdown-divider"></asp:DropDownList>

    </div>

    <div class="line3_btn">
        <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
        </asp:LinkButton>

        <%--  <asp:Button ID="btnReport" runat="server" class="btn btn-primary btn-user btn-block" Text="Show Report" Width="150px" ValidationGroup="Report" OnClick="btnReport_Click" />--%>
    </div>
    <div class="line4_footer" style="border: 1px solid #ccc;">

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

