<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesPromotionReport.aspx.cs" Inherits="SalesPromotionReport" %>

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

        .line_footer {
            position: absolute;
            top: 170px;
            width: 80%;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/flash-sale.png" Width="50px" />
            <asp:Label ID="Label5" runat="server" Text="Actual Sales By Promotion Bill Type Report" CssClass="text-white"></asp:Label>
        </div>

    </div>
    <div class="line_footer">
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
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Promotion Bill Type</a></li>
                </ul>
                <div id="tabs-1" style="width: 100%;">
                    <table style="width: 100%;">
                        <tr style="white-space: nowrap">
                            <td style="white-space: nowrap; text-align: left">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                             <asp:Label ID="Label4" runat="server" Text="ประเภทรายงาน : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="180px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;

                                            <asp:Label ID="Label3" runat="server" Text="ปี : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlYear" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                              
                                                &nbsp;&nbsp;
                                                <asp:Label ID="Label1" runat="server" Text="เดือน : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlMonth" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                                <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary">
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


</asp:Content>


