<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RefreshTransferStockReport.aspx.cs" Inherits="RefreshTransferStockReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="card" style="position: absolute; width: 80%;">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/refresh.png" Width="50px" />
            <asp:Label ID="Label4" runat="server" Text="Refresh Transfer Stock" CssClass="text-white"></asp:Label>
        </div>


        <div class="card-body">
            <div style="width: 100%; font-size: 14px; color: black;">
                <div class="modal-content">
                    <table>
                        <tr>
                            <td>
                                <div class="modal-header">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="linkRefreshTransferStock" runat="server" OnClick="linkRefreshTransferStock_Click" Style="width: 150px;" class="btn btn-primary">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/refresh.png" Width="23px" />Refresh Stock
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="modal-body" style="font-size: 14px;">

                                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" ss HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />

                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>



    <div id="refresh-dialog-alert-msg" title="Information" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Refresh ข้อมูล stock เรียบร้อยแล้ว!</p>
    </div>
    <div id="refresh-err-dialog-alert-msg" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>เกิดข้อผิดพลาดบางอย่าง กรุณา manualrun sql job ด้วยตนเอง!</p>
    </div>
</asp:Content>

