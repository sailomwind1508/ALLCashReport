<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllCashReport_V2.aspx.cs" Inherits="AllCashReport_V2" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("#ContentPlaceHolder1_txtPrevStartDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');
            $("#ContentPlaceHolder1_txtPrevEndDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');
            $("#ContentPlaceHolder1_txtCurStartDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');
            $("#ContentPlaceHolder1_txtCurEndDate").datepicker({
                changeMonth: true,
                changeYear: true
            }).datepicker('option', 'dateFormat', 'dd/mm/yy');

            var passwordControl = $("#ContentPlaceHolder1_txtPrevStartDate");
            passwordControl.setAttribute("autocomplete", "off");
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

        .line1_preleft {
            position: absolute;
            top: 150px;
            left: 0;
            width: 10%;
            height: 30px;
            min-width: 375px;
        }

        .line1_left {
            position: absolute;
            top: 150px;
            left: 10%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 35%;
            width: 25%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 60%;
            width: 30%;
            height: 30px;
            text-align: left;
        }

        .line1_endright {
            position: absolute;
            top: 150px;
            left: 55%;
            width: 10%;
            height: 30px;
        }

        .line2_preleft {
            position: absolute;
            top: 180px;
            left: 0;
            width: 10%;
            height: 30px;
        }

        .line2_left {
            position: absolute;
            top: 180px;
            left: 10%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line2_center {
            position: absolute;
            top: 180px;
            left: 35%;
            width: 25%;
            height: 30px;
            text-align: center;
        }

        .line2_right {
            position: absolute;
            top: 180px;
            left: 60%;
            width: 30%;
            height: 30px;
            text-align: left;
        }

        .line2_endright {
            position: absolute;
            top: 180px;
            left: 55%;
            width: 10%;
            height: 30px;
        }

        .line3_btn {
            position: absolute;
            top: 210px;
            left: 0px;
            width: 100%;
            height: 30px;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 250px;
            left: 0px;
            width: 100%;
            height: 600px;
            text-align: center;
        }

        @media screen and (max-width: 1080px) { /* when the width is less than 30em, make both of the widths 100% */
            .line1_left {
                position: absolute;
                top: 150px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_center {
                position: absolute;
                top: 180px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_right {
                position: absolute;
                top: 210px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line2_left {
                position: absolute;
                top: 240px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line2_center {
                position: absolute;
                top: 270px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line2_right {
                position: absolute;
                top: 300px;
                left: 10%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line3_btn {
                position: absolute;
                top: 330px;
                left: 0px;
                width: 100%;
                height: 30px;
                text-align: center;
            }

            .line4_footer {
                position: absolute;
                top: 380px;
                left: 0px;
                width: 100%;
                height: 600px;
                text-align: center;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label5" runat="server" Text="บริษัท ออลมาร์เก็ตติ้ง จำกัด"></asp:Label>
    </div>
    <div class="line1_preleft"></div>
    <div class="line1_left">
        <asp:Label ID="Label1" runat="server" Text="Previous Start Date : "></asp:Label>
        <asp:TextBox ID="txtPrevStartDate" runat="server" Width="100px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtPrevStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line1_center">
        <asp:Label ID="Label2" runat="server" Text="Previous End Date : "></asp:Label>
        <asp:TextBox ID="txtPrevEndDate" runat="server" Width="100px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtPrevEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line1_right">
        <asp:Label ID="Label3" runat="server" Text="ภูมิภาค : "></asp:Label><asp:DropDownList ID="ddlArea" runat="server" Height="23px" Width="149px"></asp:DropDownList>
        <asp:Label ID="Label8" runat="server" Text="Amt Size : "></asp:Label><asp:DropDownList ID="ddlFilter" runat="server" Height="19px" Width="150px"></asp:DropDownList>
    </div>
    <div class="line1_endright"></div>

    <div class="line2_preleft"></div>
    <div class="line2_left">
        <asp:Label ID="Label6" runat="server" Text="Current Start Date : "></asp:Label>
        <asp:TextBox ID="txtCurStartDate" runat="server" Width="100px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtCurStartDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div class="line2_center">
        <asp:Label ID="Label7" runat="server" Text="Current End Date : "></asp:Label>
        <asp:TextBox ID="txtCurEndDate" runat="server" Width="100px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtCurEndDate" ValidationGroup="Report" ForeColor="Red"></asp:RequiredFieldValidator>

    </div>
    <div class="line2_right">
        <asp:Label ID="Label4" runat="server" Text="ศูนย์ : "></asp:Label><asp:DropDownList ID="ddlProvince" runat="server" Height="19px" Width="150px"></asp:DropDownList>
        <asp:Label ID="Label9" runat="server" Text="Amt Type : "></asp:Label>
        <asp:DropDownList ID="ddlAmt" runat="server" Height="19px" Width="150px"></asp:DropDownList>
    </div>
    <div class="line2_endright"></div>
    <div class="line3_btn">

        <asp:Button ID="btnReport" runat="server" Height="33px" OnClick="btnReport_Click" Style="font-weight: 700" Text="แสดงรายงาน" Width="96px" ValidationGroup="Report" Class="button" />


    </div>
    <div class="line4_footer">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" />
    </div>

    <div id="dialog-message" title="Warning" style="display: none">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Please enter date time for filter report!
        </p>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

