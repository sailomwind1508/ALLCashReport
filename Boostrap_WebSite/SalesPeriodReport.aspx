<%@ Page Title="Sales Period Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesPeriodReport.aspx.cs" Inherits="SalesPeriodReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.9/css/dataTables.bootstrap.min.css" />
    <%-- <link type="text/css" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />--%>
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/responsive/1.0.7/css/responsive.bootstrap.min.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/responsive/1.0.7/js/dataTables.responsive.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=GridView1]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": true,
                "sPaginationType": "full_numbers"
            });
        });
    </script>
    <style>
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

        .line1_button {
            position: absolute;
            top: 150px;
            left: 46%;
            width: 30%;
            height: 30px;
            text-align: center;
        }

        .line2_footer {
            position: absolute;
            top: 200px;
            left: 13%;
            width: 87%;
        }

        .header-center {
            text-align: center;
            white-space: nowrap !important;
        }

        .imgWidth {
            Width: 30px;
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
    </style>
    <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/online-shopping.png" Width="50px" />
            <asp:Label ID="Label4" runat="server" Text="Sales Yearly Report" CssClass="text-white"></asp:Label>
        </div>

    <div class="line1_button">

        <asp:LinkButton ID="linkExportExcel" runat="server" OnClick="linkExportExcel_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />Export Excel
        </asp:LinkButton>

    </div>
    <div class="line2_footer">
        <div style="width: 100%">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" class="table table-striped"
                Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                 DataKeyNames="รหัสร้านค้า" EmptyDataText="No records Found">
                <Columns>
                </Columns>
                <PagerStyle CssClass="pagination-ys" />
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" CssClass="header-center" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" CssClass="rowStyle" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>

