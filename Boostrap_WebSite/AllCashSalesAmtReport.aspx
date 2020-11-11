<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllCashSalesAmtReport.aspx.cs" Inherits="AllCashSalesAmtReport" %>

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

        function activeTab(obj) {
            $('#tabs').tabs({ active: obj });
        }

        function showR1() {
            $('#div_r1').show();
            $('#div_r2').hide();
            $('#div_r3').hide();
        }

        function showR2() {
            $('#div_r1').hide();
            $('#div_r2').show();
            $('#div_r3').hide();
        }

        function showR3() {
            $('#div_r1').hide();
            $('#div_r2').hide();
            $('#div_r3').show();
        }

        function hideAll() {
            $('#div_r1').hide();
            $('#div_r2').hide();
            $('#div_r3').hide();
        }

        function showValidateMsg() {
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

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

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="card">

        <div class="card-header header bg-gradient-primary">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/img/online-shopping.png" Width="50px" />
                <asp:Label ID="Label2" runat="server" Text="รายงานจำนวนขายสินค้า All Cash" CssClass="text-white"></asp:Label>
        </div>

    </div>

    <div class="card" style="position: absolute; width: 80%;">
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
                    font-family: Calibri;
                }
            </style>
            <div id="tabs" style="width: 100%; height: 100%; font-size: 14px;">
                <ul>
                    <li><a href="#tabs-1">รายงานจำนวนขายสินค้า all cash</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="text-align: left">
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="รูปแบบรายงาน : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="350px" class="dropdown-divider" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_r1" style="width: 100%; display: none">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="วันที่เริ่มต้น : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateFrom" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateFrom" runat="server" ErrorMessage="*" ControlToValidate="txtDateFrom" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="วันที่สิ้นสุด : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateTo" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateTo" runat="server" ErrorMessage="*" ControlToValidate="txtDateTo" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Report">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" OnUnload="CrystalReportViewer1_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="div_r2" style="width: 100%; display: none">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="วันที่เริ่มต้น : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateFrom2" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateFrom2" runat="server" ErrorMessage="*" ControlToValidate="txtDateFrom2" ForeColor="Red" ValidationGroup="Report2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="วันที่สิ้นสุด : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateTo2" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateTo2" runat="server" ErrorMessage="*" ControlToValidate="txtDateTo2" ForeColor="Red" ValidationGroup="Report2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport2" runat="server" OnClick="btnReport2_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report2">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer2_Load" OnUnload="CrystalReportViewer2_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div id="div_r3" style="width: 100%; display: none">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="วันที่เริ่มต้น : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateFrom3" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateFrom3" runat="server" ErrorMessage="*" ControlToValidate="txtDateFrom3" ForeColor="Red" ValidationGroup="Report3"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="วันที่สิ้นสุด : "></asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDateTo3" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDateTo3" runat="server" ErrorMessage="*" ControlToValidate="txtDateTo3" ForeColor="Red" ValidationGroup="Report3"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport3" runat="server" OnClick="btnReport3_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report3">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <CR:CrystalReportViewer ID="CrystalReportViewer3" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer3_Load" OnUnload="CrystalReportViewer3_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
    </div>

    <asp:HiddenField ID="hfCurrentPage" runat="server" />

    <script type="text/javascript">

        $("#<%=txtDateFrom.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateTo.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateFrom2.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateTo2.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateFrom3.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateTo3.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

    </script>
</asp:Content>

