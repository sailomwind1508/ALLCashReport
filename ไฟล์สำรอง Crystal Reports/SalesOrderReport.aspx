<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesOrderReport.aspx.cs" Inherits="SalesOrderReport" %>

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

        .line1 {
            position: absolute;
            top: 150px;
            left: 20%;
            width: 65%;
            height: 30px;
            text-align: center;
        }

        .line2_footer {
            position: absolute;
            top: 200px;
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

            .line1 {
                position: absolute;
                top: 150px;
                left: 18%;
                width: 65%;
                height: 30px;
                text-align: center;
            }

            .line2_footer {
                position: absolute;
                top: 200px;
                left: 18%;
                width: 70%;
            }
        }
    </style>

    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Sales Order Report"></asp:Label>
    </div>

    <div class="line1">

        <table border="0" style="width: 1198px">
            <tr style="text-align: center">
                <td></td>
                <td style="text-align: left; width: 180px;white-space:nowrap">
                    <asp:Label ID="Label5" runat="server" Text="Doc Date From : "></asp:Label>
                </td>
                <td style="text-align: left; width: 200px;white-space:nowrap">
                    <asp:TextBox ID="txtDocDateFrom" runat="server" Width="111px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDocDateFrom" runat="server" ErrorMessage="*" ControlToValidate="txtDocDateFrom" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: left; width: 180px;white-space:nowrap">
                    <asp:Label ID="Label3" runat="server" Text="Doc Date To : "></asp:Label>
                </td>
                <td style="text-align: left; width: 200px;white-space:nowrap">
                    <asp:TextBox ID="txtDocDateTo" runat="server" Width="111px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDocDateTo" runat="server" ErrorMessage="*" ControlToValidate="txtDocDateTo" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                </td>
                <td style="text-align: right; width: 350px; white-space: nowrap;">
                    <asp:Label ID="Label1" runat="server" Text="สินค้า : "></asp:Label>&nbsp;
                    <asp:DropDownList ID="ddlProduct" runat="server" Height="25px" Width="501px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                </td>
                <td style="text-align: right; width: 98px; white-space: nowrap;">
                    <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
                    </asp:LinkButton></td>
            </tr>
        </table>
    </div>


    <div class="line2_footer" style="font-size: 14px;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
    </div>

    <div id="stock-dialog-alert-msg" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>ข้อมูล stock ล่าสุดจะสามารถตรวจสอบได้ หลังเวลา 13.00 น.</p>
    </div>

    <script type="text/javascript">

        $("#<%=txtDocDateFrom.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $("#<%=txtDocDateTo.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
    </script>
    <div></div>
</asp:Content>

