<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransferStockBySKU.aspx.cs" Inherits="TransferStockBySKU" %>

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
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/warehouse.png" Width="50px" />
            <asp:Label ID="Label6" runat="server" Text="Transfer Stock Daily By SKU Report" CssClass="text-white"></asp:Label>
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
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Transfer Stock Daily By SKU</a></li>
                    </ul>
                    <div id="tabs-1" style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td>
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="Label3" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="194px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                                            &nbsp;&nbsp;
                                    <asp:Label ID="Label4" runat="server" Text="เลือกแวน : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlVan" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Doc. Date : "></asp:Label>
                                    <asp:TextBox ID="txtDocDate" runat="server" Width="111px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDocDate" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="เลือกสินค้า : "></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlProduct" runat="server" Height="25px" Width="501px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
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
