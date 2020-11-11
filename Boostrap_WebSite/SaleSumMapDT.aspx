<%@ Page Title="Sales Monthly Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SaleSumMapDT.aspx.cs" Inherits="SaleSumMapDT" Async="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <style type="text/css">
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .header-center {
            text-align: center;
            white-space: pre-wrap !important;
            width: 80px;
            font-size: 12px;
        }

        .imgWidthEdit {
            Width: 20px;
            display: block;
            margin-left: auto;
            margin-right: auto;
        }

        .imgWidthDel {
            Width: 20px;
            display: block;
            margin-left: auto;
            margin-right: auto;
        }

        .imgWidth {
            Width: 26px;
            display: block;
            margin-left: auto;
            margin-right: auto;
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

        .mydatagrid {
            width: 80%;
            border: solid 2px black;
            min-width: 80%;
        }

        .header_grid {
            background-color: #646464;
            font-family: Arial;
            color: White;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 16px;
            border-color: cornflowerblue;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            min-height: 25px;
            border: none 0px transparent;
            border-color: cornflowerblue;
        }

            .rows:hover {
                background-color: blanchedalmond;
                font-family: Arial;
                color: blue;
                text-align: left;
            }

        .selectedrow {
            background-color: #ff8000;
            font-family: Arial;
            color: #fff;
            font-weight: bold;
        }

        .mydatagrid a /** FOR THE PAGING ICONS **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: #fff;
            text-decoration: none;
            font-weight: bold;
        }

            .mydatagrid a:hover /** FOR THE PAGING ICONS HOVER STYLES**/ {
                background-color: cornflowerblue;
                color: #fff;
            }

        .mydatagrid span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            background-color: cornflowerblue;
            color: #fff;
            padding: 5px 5px 5px 5px;
        }

        .mydatagrid td {
            padding: 5px;
        }

        .mydatagrid th {
            padding: 5px;
        }

        .mydatagrid tr:nth-child(even) {
            background-color: #ffffff;
        }

        .mydatagrid tr:nth-child(odd) {
            background-color: #C2D69B;
        }

        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="card">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/placeholder.png" Width="50px" />
            <asp:Label ID="Label8" runat="server" Text="Sales Monthly Report" CssClass="text-white"></asp:Label>
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
                            <asp:Image ID="Image18" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Sales Monthly</a></li>
                    </ul>
                    <div id="tabs-1" style="width: 100%;">

                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>

                                            <asp:Label ID="Label1" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>

                                            &nbsp;&nbsp;
                            <asp:Label ID="Label3" runat="server" Text="เลือกแวน : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlVan" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVan_SelectedIndexChanged"></asp:DropDownList>

                                            &nbsp;&nbsp;
                            <asp:Label ID="Label4" runat="server" Text="เลือกตลาด : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlSaleArea" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlSaleArea_SelectedIndexChanged"></asp:DropDownList>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Start Date : "></asp:Label>&nbsp;
                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                    <asp:Label ID="Label6" runat="server" Text="End Date : "></asp:Label>&nbsp;
                    <asp:TextBox ID="txtEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                                    &nbsp;&nbsp;
                    <asp:Label ID="Label7" runat="server" Text="เลือกรูปแบบรายงาน : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                    &nbsp;&nbsp;
                    <asp:LinkButton ID="linkExportReport" runat="server" OnClick="btnDetails_Click" Visible="false">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/report.png" Width="23px" />Export Report
                    </asp:LinkButton>

                                    &nbsp;&nbsp;
                    <asp:LinkButton ID="linkAllLocation" runat="server" OnClick="LinkButton1_Click" Visible="false">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/placeholder.png" Width="23px" />Show All Location
                    </asp:LinkButton>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" OnClientClick="return setResponsiveTable()" Style="width: 120px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Report">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                                    </asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>

                                    <div style="width: 100%">

                                        <asp:GridView ID="grdCustomerList" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="100"
                                            CellPadding="4" DataKeyNames="CustomerID" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowPaging="true" OnPageIndexChanging="grdCustomerList_PageIndexChanging" OnRowCommand="grdCustomerList_RowCommand">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="imgLocation" DataTextField="CustomerID" ControlStyle-CssClass="imgWidth"
                                                    HeaderText="Map"
                                                    ImageUrl="~/img/placeholder.png" />


                                                <asp:BoundField DataField="BranchName" HeaderText="ศูนย์" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="VAN_ID" HeaderText="แวน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="SalAreaName" HeaderText="ตลาด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="CustName" HeaderText="ชื่อร้านค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="Telephone" HeaderText="เบอร์โทร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="AddressNo" HeaderText="ที่อยู่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="District" HeaderText="ตำบล" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="Area" HeaderText="อำเภอ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="Province" HeaderText="จังหวัด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />


                                                <asp:BoundField DataField="GAmount" HeaderText="G Amount" DataFormatString="{0, 0:N2}" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="rigthRow" />
                                                <asp:BoundField DataField="NAmount" HeaderText="N Amount" DataFormatString="{0, 0:N2}" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="rigthRow" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" DataFormatString="{0, 0:N2}" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="rigthRow" />


                                                <asp:BoundField DataField="Latitude" HeaderText="Latitude" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="Longitude" HeaderText="Longitude" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="ShopTypeName" HeaderText="ประเภทร้านค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="FlagDel" HeaderText="Status" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                            </Columns>

                                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>

                                </td>
                            </tr>
                        </table>


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

                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>

