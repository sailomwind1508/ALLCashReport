<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="test4.aspx.cs" Inherits="View_test4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js"></script>
    <link href="../jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">

    <script type="text/javascript">
        $(function () {

        });


    </script>

    <style>
        .header-center {
            text-align: center;
            white-space: nowrap !important;
            column-width: 80px;
            font-size: 11px;
            height: 50px;
            font-weight: bold;
        }

        .rowStyle {
            white-space: nowrap;
            column-width: 80px;
            width: auto;
            font-size: 9px;
            font-family: Tahoma;
            height: 20px;
        }

        .rowStyle2 {
            white-space: nowrap;
            column-width: 80px;
            width: auto;
            font-size: 9px;
            font-family: Tahoma;
            height: 20px;
        }

        th {
            border: 1px solid black;
            width: 100px;
            overflow: hidden;
            white-space: nowrap !important;
        }

        .header {
            position: absolute;
            top: 10px;
            left: 50%;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: right;
        }

        .div_left {
            position: absolute;
            top: 60px;
            left: 0%;
            width: 25%;
            height: 50px;
            text-align: left;
        }

        .div_center {
            position: absolute;
            top: 60px;
            left: 20%;
            height: 50px;
            text-align: left;
        }

        .excelButtom {
            width: 170px;
            font-family: Verdana,Arial,sans-serif;
            font-size: 1em;
            color: white;
            white-space: nowrap;
        }
    </style>


    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <table>

                <tr style="white-space: nowrap;">
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Value Type : </h6>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlValueType" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Month : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Year : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Period : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlPeriod" runat="server"></asp:DropDownList>
                    </td>
                    <td>&nbsp;&nbsp;</td>
                    <td>

                        <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                        </asp:LinkButton>
                    </td>
                </tr>

            </table>
        </div>
        <div class="card-body">
            <h6 class="m-0 font-weight-bold" style="color: black;">
                <asp:Literal ID="lblTitle" runat="server"></asp:Literal></h6>
            <div class="table-responsive">
                <asp:GridView ID="grdDash1" runat="server" AutoGenerateColumns="true" class="" Width="100%" CellSpacing="0"
                    BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                    EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdDash1_RowDataBound">
                    <Columns>
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                    <HeaderStyle BackColor="#C6EFCE" Font-Bold="True" ForeColor="Black" CssClass="header-center" />
                    <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" ForeColor="Black" CssClass="rowStyle" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView>
            </div>
       
            <h6 class="m-0 font-weight-bold" style="color: black;">
                <asp:Literal ID="lblDaily2" runat="server"></asp:Literal></h6>
            <div class="table-responsive">
                <asp:GridView ID="grdData2" runat="server" AutoGenerateColumns="true" class="" Width="100%" CellSpacing="0"
                    BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                    EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdData2_RowDataBound">
                    <Columns>
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                    <HeaderStyle BackColor="#FFCCFF" Font-Bold="True" ForeColor="Black" CssClass="header-center" Width="50px" />
                    <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" ForeColor="Black" CssClass="rowStyle2" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView>
            </div>

            <h6 class="m-0 font-weight-bold" style="color: black;">
                <asp:Literal ID="lblDaily1" runat="server"></asp:Literal></h6>
            <div class="table-responsive">
                <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="true" class="" Width="100%" CellSpacing="0"
                    BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                    EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdData_RowDataBound">
                    <Columns>
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                    <HeaderStyle BackColor="#9BC2E6" Font-Bold="True" ForeColor="Black" CssClass="header-center" Width="50px" />
                    <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" ForeColor="Black" CssClass="rowStyle2" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView>
            </div>
        
            
        </div>
        <br />
        <div id="divHDaily1" runat="server" class="card-header py-3">
            <table>

                <tr style="white-space: nowrap;">

                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Month : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlDailyMonth" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Year : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlDailyYear" runat="server"></asp:DropDownList>
                    </td>

                    <td>
                        <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; background-color: brown; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                        </asp:LinkButton>
                    </td>
                </tr>

            </table>
        </div>
        <br />
        <div id="divHDaily2" runat="server" class="card-header py-3">
            <table>

                <tr style="white-space: nowrap;">

                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Month : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlDailyMonth2" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: black;">Year : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlDailyYear2" runat="server"></asp:DropDownList>
                    </td>

                    <td>
                        <asp:LinkButton ID="btnSearch2" runat="server" OnClick="btnSearch2_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; background-color: brown; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                        </asp:LinkButton>
                    </td>
                </tr>

            </table>
        </div>

    </div>

</asp:Content>