<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebUserControl_DashBoard.ascx.cs" Inherits="WebUserControl_DashBoard" %>
<script type="text/javascript">
    $(function () {

    });


</script>
<style>
    .header-center {
        text-align: center;
        white-space: nowrap !important;
        width: 80px;
        font-size: 14px;
        height: 50px;
    }

    .rowStyle {
        white-space: nowrap;
        width: 30px;
        font-size: 15px;
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

<div class="container-fluid">
    <h1 class="m-0 font-weight-bold" style="color: red;">Actual Sales Report</h1>
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <table>

                <tr style="white-space: nowrap;">
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: red;">Value Type : </h6>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlValueType" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: red;">Month : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: red;">Year : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <h6 class="m-0 font-weight-bold" style="color: red;">Period : </h6>
                    </td>
                    <td style="white-space: nowrap;">
                        <asp:DropDownList ID="ddlPeriod" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; background-color: brown; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/img/receive-amount.png" Width="23px" />Refresh
                        </asp:LinkButton>
                    </td>
                </tr>

            </table>
        </div>
        <div class="card-body">
            <h6 class="m-0 font-weight-bold" style="color: red;">
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
                    <HeaderStyle BackColor="#BD220D" Font-Bold="True" ForeColor="#FFFFFF" CssClass="header-center" Width="50px" />
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
