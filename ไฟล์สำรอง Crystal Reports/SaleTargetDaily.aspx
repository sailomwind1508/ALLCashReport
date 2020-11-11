<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SaleTargetDaily.aspx.cs" Inherits="SaleTargetDaily" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
                    location.reload();
                }, 10000);
            } catch (e) {
                alert(e);
            }
        }

        function activeTab(obj) {
            $('#tabs').tabs({ active: obj });
        }

        function showEmp() {
            $('#div_emp').show();
            $('#div_cust').hide();
            $('#div_custSum').hide();
            $('#div_plastic').hide();
        }

        function showCust() {
            $('#div_emp').hide();
            $('#div_cust').show();
            $('#div_custSum').hide();
            $('#div_plastic').hide();
        }

        function showCustSum() {
            $('#div_emp').hide();
            $('#div_cust').hide();
            $('#div_custSum').show();
            $('#div_plastic').hide();
        }

        function showPlastic() {
            $('#div_emp').hide();
            $('#div_cust').hide();
            $('#div_custSum').hide();
            $('#div_plastic').show();
        }

        function hidePlastic() {
            $('#div_emp').hide();
            $('#div_cust').hide();
            $('#div_custSum').hide();
            $('#div_plastic').hide();
        }

        function hideAll() {
            $('#div_emp').hide();
            $('#div_cust').hide();
            $('#div_custSum').hide();
            $('#div_plastic').hide();
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
            position: absolute;
            top: 100px;
            left: 0px;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 150px;
            left: 13%;
            width: 70%;
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

            .line4_footer {
                position: absolute;
                top: 150px;
                left: 18%;
                width: 70%;
            }
        }

        .header-center {
            text-align: center;
            white-space: nowrap !important;
        }

        .imgWidth {
            Width: 26px;
            display: block;
            margin-left: auto;
            margin-right: auto;
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

        .excelButtom {
            width: 170px;
            font-family: Verdana,Arial,sans-serif;
            font-size: 1em;
            color: white;
            white-space: nowrap;
        }

        .fileUpploadFont {
            color: black;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="รายงานเป้าหมายของแผนก WS"></asp:Label>
    </div>
    <div class="line4_footer">
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
                    font-family: Calibri;
                }
            </style>
            <div id="tabs" style="width: 120%; height: 100%; font-size: 14px;">
                <ul>
                    <li><a href="#tabs-1">รายงานเป้าหมายเทียบยอดจดของแผนก WS</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="text-align: left">
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="รูปแบบรายงาน : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="301px" class="dropdown-divider" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_emp" style="width: 100%;display: none">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="วันที่ : "></asp:Label>
                                                <asp:TextBox ID="txtDocDate" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDocDate" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
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
                    </div>
                    <div id="div_cust" style="width: 100%;display: none">

                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="วันที่ : "></asp:Label>
                                                <asp:TextBox ID="txtDocDate2" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDocDate2" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate2" ForeColor="Red" ValidationGroup="Report2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="พนักงาน : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlEmployee" runat="server" Height="25px" Width="228px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport2" runat="server" OnClick="btnReport2_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report2">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton></td>
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
                    <div id="div_custSum" style="width: 100%;display: none">

                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="วันที่ : "></asp:Label>
                                                <asp:TextBox ID="txtDocDate3" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDocDate3" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate3" ForeColor="Red" ValidationGroup="Report3"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="พนักงาน : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlEmployee2" runat="server" Height="25px" Width="228px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnReport3" runat="server" OnClick="btnReport3_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report3">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <CR:CrystalReportViewer ID="CrystalReportViewer3" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer3_Load" OnUnload="CrystalReportViewer3_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_plastic" style="width: 100%;display: none">

                        <table style="width: 100%;">
                            <tr style="white-space: nowrap">
                                <td style="white-space: nowrap; text-align: left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="วันที่ : "></asp:Label>
                                                <asp:TextBox ID="txtDocDateP" runat="server" Height="25px" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvDocDateP" runat="server" ErrorMessage="*" ControlToValidate="txtDocDateP" ForeColor="Red" ValidationGroup="Report4"></asp:RequiredFieldValidator>
                                            </td>
                                           
                                            <td>
                                                <asp:LinkButton ID="btnReport4" runat="server" OnClick="btnReport4_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report4">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <CR:CrystalReportViewer ID="CrystalReportViewer4" runat="server" AutoDataBind="true" ToolPanelView="None"
                                        HasRefreshButton="True" OnLoad="CrystalReportViewer4_Load" OnUnload="CrystalReportViewer4_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />

                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">

        $("#<%=txtDocDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDocDate2.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDocDate3.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

         $("#<%=txtDocDateP.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

    </script>
</asp:Content>
