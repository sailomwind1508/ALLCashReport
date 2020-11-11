<%@ Page Title="Xena&SAP Discount Comparison Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="XenaSapDiscountComparison.aspx.cs" Inherits="XenaSapDiscountComparison" %>

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
    </script>

    <style type="text/css">
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }
    </style>

    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/icons8-sap-480.png" Width="50px" />
            <asp:Label ID="Label4" runat="server" Text="Xena & SAP Discount Comparison Report" CssClass="text-white"></asp:Label>
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
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Xena & SAP Discount Comparison</a>

                        </li>
                    </ul>
                    <div id="tabs-1" style="width: 100%;">
                        <table>
                            <tr>
                                <td>

                                    <asp:Label ID="Label1" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="150px" class="dropdown-divider"></asp:DropDownList>
                                    &nbsp;&nbsp;

                                    <asp:Label ID="Label5" runat="server" Text="Start Date : "></asp:Label>
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;

                                    <asp:Label ID="Label6" runat="server" Text="End Date : "></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;

                                    <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Report">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                                    </asp:LinkButton>
                                </td>

                            </tr>

                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="position: absolute;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" ss HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />

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

