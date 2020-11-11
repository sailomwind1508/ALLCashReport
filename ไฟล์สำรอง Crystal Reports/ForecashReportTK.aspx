<%@ Page Title="Forecast Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ForecashReportTK.aspx.cs" Inherits="ForecashReportTK" %>

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
            width: 30%;
            height: 30px;
            text-align: left;
        }


        .line4_footer {
            position: absolute;
            top: 200px;
            left: 12%;
            width: 70%;
            
        }


        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
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
                width: 30%;
                height: 30px;
                text-align: left;
            }


            .line4_footer {
                position: absolute;
                top: 200px;
                left: 20%;
                width: 70%;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Forecast Report"></asp:Label>
    </div>
    <div class="line1_left">
        <asp:Label ID="Label1" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="150px" class="dropdown-divider"></asp:DropDownList>
    </div>
    <div class="line1_center">
        <asp:Label ID="Label3" runat="server" Text="กลุ่มสินค้า : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlProductType" runat="server" Height="25px" Width="150px" class="dropdown-divider"></asp:DropDownList>
    </div>
    <div class="line1_right">
            <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
        </asp:LinkButton>
    </div>

    <div class="line4_footer" style="border: 1px solid #ccc;">
        
        <CR:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true" toolpanelview="None" hasrefreshbutton="True" OnLoad="CrystalReportViewer1_Load" hastogglegrouptreebutton="False" haspagenavigationbuttons="False" width="100%" />
    </div>

</asp:Content>