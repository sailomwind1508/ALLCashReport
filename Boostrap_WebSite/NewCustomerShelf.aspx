<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NewCustomerShelf.aspx.cs" Inherits="NewCustomerShelf" %>

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
            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/shelf.png" Width="50px" />
            <asp:Label ID="Label2" runat="server" Text="Customer Shelf Report" CssClass="text-white"></asp:Label>
        </div>
    </div>
    <div class="line_footer">
        <div style="width: 100%; position: absolute;">
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

                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Customer Shelf</a></li>
                    <%--<li><a href="#tabs-1">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;New Customer Shelf</a></li>
                    <li><a href="#tabs-2">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Customer Shelf</a></li>--%>
                </ul>
                <%--<div id="tabs-1" style="width: 100%;">
                    <table style="width: 100%;">
                        <tr style="white-space: nowrap">
                            <td style="white-space: nowrap; text-align: left">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label3" runat="server" Text="ปี : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlYear" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                              
                                                &nbsp;&nbsp;
                                                <asp:Label ID="Label1" runat="server" Text="เดือน : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlMonth" runat="server" Height="25px" Width="120px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                                <asp:Label ID="Label4" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>

                                            &nbsp;&nbsp;
                                                <asp:Label ID="Label5" runat="server" Text="แวน : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlVan" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

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

                                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None"
                                    HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" OnUnload="CrystalReportViewer1_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />

                            </td>
                        </tr>
                    </table>


                </div>--%>
                <div id="tabs-1" style="width: 100%;">
                    <table style="width: 100%;">
                        <tr style="white-space: nowrap">
                            <td style="white-space: nowrap; text-align: left">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label10" runat="server" Text="ประเภทรายงาน : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlCustType" runat="server" Height="25px" Width="120px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                            <asp:Label ID="Label6" runat="server" Text="ปี : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlYear_2" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                              
                                                &nbsp;&nbsp;
                                                <asp:Label ID="Label7" runat="server" Text="เดือน : "></asp:Label>&nbsp;

                                                <asp:DropDownList ID="ddlMonth_2" runat="server" Height="25px" Width="120px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                                <asp:Label ID="Label8" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlBranch_2" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>

                                            &nbsp;&nbsp;
                                                <asp:Label ID="Label9" runat="server" Text="แวน : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlVan_2" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>


                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="btnSearch_2" runat="server" OnClick="btnSearch_2_Click" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary">
                                                <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                                            </asp:LinkButton>
                                            &nbsp;&nbsp;
                                            
                                            <asp:LinkButton ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Search">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                            </asp:LinkButton>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>

                                <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" AutoDataBind="true" ToolPanelView="None"
                                    HasRefreshButton="True" OnLoad="CrystalReportViewer2_Load" OnUnload="CrystalReportViewer2_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />

                            </td>
                        </tr>
                    </table>


                </div>
            </div>
        </div>
    </div>




</asp:Content>
