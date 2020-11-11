<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ForecashReportTK.aspx.cs" Inherits="ForecashReportTK" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            width: 35%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 45%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 65%;
            width: 45%;
            height: 30px;
            text-align: left;
        }

        .line3_btn {
            position: absolute;
            top: 210px;
            left: 0px;
            width: 100%;
            height: 30px;
            text-align: center;
        }

        .line4_footer-b {
            position: absolute;
            top: 200px;
            left: 5%;
            width: 7%;
            height: 600px;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 200px;
            left: 12%;
            width: 70%;
            height: 600px;
            text-align: center;
        }

        .line4_footer-a {
            position: absolute;
            top: 200px;
            left: 82%;
            width: 10%;
            height: 600px;
            text-align: center;
        }

        @media screen and (max-width: 1080px) { /* when the width is less than 30em, make both of the widths 100% */
            .line1_left {
                position: absolute;
                top: 150px;
                left: 15%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_center {
                position: absolute;
                top: 180px;
                left: 15%;
                width: 100%;
                height: 30px;
                text-align: left;
            }

            .line1_right {
                position: absolute;
                top: 210px;
                left: 15%;
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


            .line4_footer-b {
                position: absolute;
                top: 250px;
                left: 10%;
                width: 10%;
                height: 600px;
                text-align: center;
            }

            .line4_footer {
                position: absolute;
                top: 250px;
                left: 20%;
                width: 70%;
                height: 600px;
                text-align: center;
            }

            .line4_footer-a {
                position: absolute;
                top: 250px;
                left: 90%;
                width: 10%;
                height: 600px;
                text-align: center;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="บริษัท ขายสะดวก จำกัด"></asp:Label>
    </div>
    <div class="line1_left">
        <asp:Label ID="Label1" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlBranch" runat="server" Height="23px" Width="149px"></asp:DropDownList>
    </div>
    <div class="line1_center">
        <asp:Label ID="Label3" runat="server" Text="กลุ่มสินค้า : "></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlProductType" runat="server" Height="23px" Width="149px"></asp:DropDownList>
    </div>
    <div class="line1_right">
        <asp:Button ID="btnReport" runat="server" Height="33px" OnClick="btnReport_Click" Style="font-weight: 700" Text="แสดงรายงาน" Width="96px" ValidationGroup="Report" Class="button" /></div>
    <div class="line4_footer-b"></div>
    <div class="line4_footer">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
    </div>
    <div class="line4_footer-a"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

