<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RefreshTransferStockReport.aspx.cs" Inherits="RefreshTransferStockReport" %>

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

        function showRefreshAlertMsg() {
            $('[id*=refresh-dialog-alert-msg]').dialog({
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

        function RefreshPage() {
            location.reload(true);
        }

        function showRefreshErrAlertMsg() {
            $('[id*=refresh-err-dialog-alert-msg]').dialog({
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

        .line1_button {
            position: absolute;
            top: 150px;
            left: 46%;
            width: 30%;
            height: 30px;
            text-align: center;
        }

        .line2_footer {
            position: absolute;
            top: 200px;
            left: 18%;
            width: 87%;
        }

        .header-center {
            text-align: center;
            white-space: nowrap !important;
        }

        .imgWidth {
            Width: 30px;
        }

        .rowStyle {
            white-space: nowrap;
        }

        .centerRow {
            height: 18px;
            white-space: nowrap !important;
            text-align: center;
            vertical-align: top;
        }

        .leftRow {
            height: 18px;
            white-space: nowrap !important;
            text-align: left;
            vertical-align: top;
        }

        .rigthRow {
            height: 18px;
            white-space: nowrap !important;
            text-align: right;
            vertical-align: top;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Refresh Transfer Stock"></asp:Label>
    </div>
    <div class="line1_button">

        <asp:LinkButton ID="linkRefreshTransferStock" runat="server" OnClick="linkRefreshTransferStock_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/refresh.png" Width="23px" />Refresh Stock
        </asp:LinkButton>

    </div>
    <div class="line2_footer">
        <div style="width: 100%">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" ss HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />

        </div>
    </div>
    <div id="refresh-dialog-alert-msg" title="Information" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Refresh ข้อมูล stock เรียบร้อยแล้ว!</p>
    </div>
     <div id="refresh-err-dialog-alert-msg" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>เกิดข้อผิดพลาดบางอย่าง กรุณา manualrun sql job ด้วยตนเอง!</p>
    </div>
</asp:Content>

