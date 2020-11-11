﻿<%@ Page Title="Actual Sales Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ActualSalesQtyByChannel.aspx.cs" Inherits="ActualSalesQtyByChannel" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">

    <script type="text/javascript">

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
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 35%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 55%;
            width: 20%;
            height: 30px;
            text-align: left;
        }

        .line1-2_right {
            position: absolute;
            top: 150px;
            left: 75%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line4_footer {
            position: absolute;
            top: 200px;
            left: 12%;
           
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
            .header {
                position: absolute;
                top: 100px;
                left: 10%;
                width: 100%;
                height: 50px;
                font-size: x-large;
                font-weight: 700;
                text-align: center;
            }

            .line1_left {
                position: absolute;
                top: 150px;
                left: 15%;
                width: 25%;
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

            .line1-2_right {
                position: absolute;
                top: 150px;
                left: 80%;
                width: 25%;
                height: 30px;
                text-align: left;
                white-space: nowrap;
            }

            .line4_footer {
                position: absolute;
                top: 200px;
                left: 20%;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Actual Sales All Channel Report"></asp:Label>
    </div>
    <div class="line1_left">
        <asp:Label ID="Label1" runat="server" Text="Product Line : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlProductLine" runat="server" Height="25px" Width="150px" class="dropdown-divider"></asp:DropDownList>
    </div>
    <div class="line1_center">
        <asp:Label ID="Label5" runat="server" Text="Start Date : "></asp:Label>
        <asp:TextBox ID="txtStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

    </div>
    <div class="line1_right">
        <asp:Label ID="Label6" runat="server" Text="End Date : "></asp:Label>
        <asp:TextBox ID="txtEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

    </div>

    <div class="line1-2_right">
         <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
        </asp:LinkButton>
    </div>

    <div class="line4_footer" style="border: 1px solid #ccc;">
        
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
    </div>

    <script type="text/javascript">

        $("#<%=txtStartDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        }); //.datepicker('option', 'regional', 'th');

        $("#<%=txtEndDate.ClientID %>").datepicker({//$(".txtEndDateClass").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        }); //.datepicker('option', 'dateFormat', 'dd/mm/yy');

    </script>
    <div></div>
</asp:Content>

