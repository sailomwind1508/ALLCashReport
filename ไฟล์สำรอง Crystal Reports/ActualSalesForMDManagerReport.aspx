<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ActualSalesForMDManagerReport.aspx.cs" Inherits="ActualSalesForMDManagerReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">

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
            position: absolute;
            top: 100px;
            left: 0px;
            width: 95%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line1 {
            position: absolute;
            top: 150px;
            left: 28%;
        }

        .line4_footer {
            position: absolute;
            top: 200px;
            left: 18%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
            .header {
                position: absolute;
                top: 100px;
                left: 10%;
                width: 95%;
                height: 50px;
                font-size: x-large;
                font-weight: 700;
                text-align: center;
            }

            .line1 {
                position: absolute;
                top: 150px;
                left: 28%;
            }

            .line4_footer {
                position: absolute;
                top: 200px;
                left: 18%;
            }
        }
    </style>
    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Actual Sales All Channel Report"></asp:Label>
    </div>
    <div class="line1">


        <table border="0" style="width: 800px">
            <tr style="text-align: center; white-space: nowrap;">
                <td></td>
                <td style="text-align: right; width: 80px">
                    <asp:Label ID="Label3" runat="server" Text="Start Date : "></asp:Label>
                </td>
                <td style="text-align: left; width: 150px">

                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right; width: 80px">
                    <asp:Label ID="Label6" runat="server" Text="End Date : "></asp:Label>
                </td>
                <td style="text-align: left; width: 150px">

                    <asp:TextBox ID="txtEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                </td>
                <td style="width: 100px; white-space: nowrap;"></td>
                <td style="text-align: right; width: 200px; white-space: nowrap;">
                    <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/report.png" Width="23px" />Show Report
                    </asp:LinkButton>
                </td>

            </tr>
        </table>
    </div>

    <div class="line4_footer" style="border: 1px solid #ccc;">

        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
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
