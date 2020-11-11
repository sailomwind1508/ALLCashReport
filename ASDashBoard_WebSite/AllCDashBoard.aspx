<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllCDashBoard.aspx.cs" Inherits="View_AllCDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function () {

        });


    </script>
    <style>
        .header-center {
            text-align: center;
            font-size: 11px;
            height: 50px;
            font-weight: bold;
        }

        .rowStyle {
            width: auto;
            font-size: 9px;
            font-family: Tahoma;
            height: 20px;
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

    <div class="">

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
                            <h6 class="m-0 font-weight-bold" style="color: black; display: none;">Period : </h6>
                        </td>
                        <td style="white-space: nowrap; display: none;">
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
                <br />
                <div class="table-responsive">
                    <asp:GridView ID="grdDash1" runat="server" AutoGenerateColumns="true" class="" Width="100%" CellSpacing="0"
                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdDash1_RowDataBound">
                        <Columns>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                        <HeaderStyle BackColor="#FFFF66" Font-Bold="True" ForeColor="Black" CssClass="header-center" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" ForeColor="Black" CssClass="rowStyle" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                        <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

