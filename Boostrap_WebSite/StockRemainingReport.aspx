<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockRemainingReport.aspx.cs" Inherits="StockRemainingReport" %>

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
    </script>

    <style type="text/css">
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/warehouse.png" Width="50px" />
            <asp:Label ID="Label4" runat="server" Text="Stock Remaining Report" CssClass="text-white"></asp:Label>
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
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Stock Remaining</a></li>
                    </ul>
                    <div id="tabs-1" style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label1" runat="server" Text="สาขา : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlSalesArea" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Report">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                                                </asp:LinkButton>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" OnUnload="CrystalReportViewer1_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

