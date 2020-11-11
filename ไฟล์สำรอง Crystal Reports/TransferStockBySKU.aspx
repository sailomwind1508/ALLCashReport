﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransferStockBySKU.aspx.cs" Inherits="TransferStockBySKU" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">

        function setHourglass() {
            document.body.style.cursor = 'wait';
        }

        function removeHourglass() {
            document.body.style.cursor = 'default';
        }

        function showStockAlertMsg() {
            $('[id*=stock-dialog-alert-msg]').dialog({
                resizable: false,
                height: 250,
                width: 450,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>

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


        .line2_left {
            position: absolute;
            top: 180px;
            left: 24%;
            width: 45%;
            height: 30px;
            text-align: left;
        }

        .line2_right {
            position: absolute;
            top: 190px;
            left: 60%;
            width: 20%;
            height: 30px;
            text-align: left;
        }


        .line4_footer {
            position: absolute;
            top: 240px;
            left: 20%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
            .header {
                position: absolute;
                top: 100px;
                left: 6%;
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
                width: 25%;
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
                width: 20%;
                height: 30px;
                text-align: left;
            }


            .line2_left {
                position: absolute;
                top: 180px;
                left: 20%;
                width: 45%;
                height: 30px;
                text-align: left;
            }

            .line2_right {
                position: absolute;
                top: 190px;
                left: 70%;
                width: 20%;
                height: 30px;
                text-align: left;
            }


            .line4_footer {
                position: absolute;
                top: 240px;
                left: 20%;
            }
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div class="header">
                <asp:Label ID="Label2" runat="server" Text="Transfer Stock Daily By SKU Report"></asp:Label>
            </div>

            <div class="line1_left">
                <asp:Label ID="Label3" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="194px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="line1_center">
                <asp:Label ID="Label4" runat="server" Text="เลือกแวน : "></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlVan" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="line1_right">
        <asp:Label ID="Label5" runat="server" Text="Doc. Date : "></asp:Label>
        <asp:TextBox ID="txtDocDate" runat="server" Width="111px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDocDate" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

    </div>
    <div class="line2_left">
        <asp:Label ID="Label1" runat="server" Text="เลือกสินค้า : "></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlProduct" runat="server" Height="25px" Width="501px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

    </div>
    <div class="line2_center">
    </div>
    <div class="line2_right">
        <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
        </asp:LinkButton>

    </div>

    <div class="line4_footer" style="border: 1px solid #ccc;">

        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
    </div>


    <div id="stock-dialog-alert-msg" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>ข้อมูล stock ล่าสุดจะสามารถตรวจสอบได้ หลังเวลา 13.00 น.</p>
    </div>

    <script type="text/javascript">

        $("#<%=txtDocDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
    </script>
    <div></div>
</asp:Content>
