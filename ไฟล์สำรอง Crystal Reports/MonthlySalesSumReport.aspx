<%@ Page Title="รายงานสรุปยอดขายรายเดือนศูนย์จัดจำหน่ายตามร้านค้า" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MonthlySalesSumReport.aspx.cs" Inherits="MonthlySalesSumReport" Async="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">

    <style type="text/css">
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

        .line2_left {
            position: absolute;
            top: 195px;
            left: 10%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line2_center {
            position: absolute;
            top: 195px;
            left: 35%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line2_right {
            position: absolute;
            top: 195px;
            left: 55%;
            width: 20%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line2-2_right {
            position: absolute;
            top: 195px;
            left: 75%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line4_footer {
            position: absolute;
            top: 150px;
            left: 15%;
            width: 70%;
        }

        .line5_footer {
            position: absolute;
            top: 1300px;
            left: 15%;
            width: 70%;
        }

        @media screen and (max-width: 1080px) { /* when the width is less than 30em, make both of the widths 100% */
            .line1_left {
                position: absolute;
                top: 150px;
                left: 25%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_center {
                position: absolute;
                top: 180px;
                left: 25%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_right {
                position: absolute;
                top: 210px;
                left: 25%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line4_footer {
                position: absolute;
                top: 250px;
                left: 35%;
                width: 65%;
                text-align: center;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="รายงานสรุปยอดขายรายเดือนศูนย์จัดจำหน่ายตามร้านค้า"></asp:Label>
    </div>
    <div class="line4_footer" style="border: 1px solid #ccc;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" HasToggleGroupTreeButton="False" Width="100%" OnLoad="CrystalReportViewer1_Load" />

    </div>

    <div></div>
</asp:Content>

