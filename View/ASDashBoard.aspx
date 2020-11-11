 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/View/ASDashBoard.aspx.cs" Inherits="ASDashBoard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css"/>
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet"/>

    <!-- Custom styles for this template-->
    <link href="../css/sb-admin-2.min.css" rel="stylesheet"/>

    <script type="text/javascript">
        $(function () {
            $('[id*=grdDash1]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": false,
                "sPaginationType": "full_numbers",
                "pageLength": 10

            });
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
            white-space: normal;
            width: 50px;
            font-size: 14px;
            font-family: Tahoma;
            text-align: left;
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
</head>
<body style="width: 100%;">
    <form id="form1" runat="server">
        <!-- Page Wrapper -->
        <div id="wrapper">

            <!-- Sidebar -->
            <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
                <!-- Sidebar - Brand -->
                <a class="sidebar-brand d-flex align-items-center justify-content-center" href="https://www.unitedfoods.co.th/">
                    <div class="sidebar-brand-icon">
                        <img id="logo" src="../img/United-food2222_1.jpg" />
                    </div>
                    <div class="sidebar-brand-text" style="white-space:nowrap;width:400px;">&nbsp;UNITED FOODS</div>
                </a>
                <!-- Divider -->
                <hr class="sidebar-divider my-0">
                <!-- Nav Item - Dashboard -->
                <li class="nav-item" text-align: left; left: 10px;">
                    <iframe src="http://www.pttor.com/oilprice-board.aspx" width="200" height="420" scrolling="no" frameborder="0"></iframe>
                    <%--<iframe src="http://namchiang.com/ncgp2-1.swf" style="font-size:12px" width="200" height="380" frameborder="0" marginheight="0" marginwidth="0" scrolling="no"></iframe>--%>
                    <iframe src='http://www.namchiang.com/GoldPriceHistory/GoldPrice2015.html' width='200' height='150' frameborder='0' scrolling='no'></iframe>
                    <%--<iframe src="http://www.tmd.go.th/daily_forecast_forweb.php" width="200" height="260" scrolling="no" frameborder="0"></iframe>--%>
                    <iframe onload="iFrameHeight(this)" id="blockrandom-93" name="" 
                        src="https://docs.google.com/spreadsheets/d/e/2PACX-1vSdiWEirzRNYezEaHaFA5iF9Td7QlkBEm1eR8ZjkXGfc2-uFO89jHKNJQH5uT_cXhBUV2A6I10tcjtU/pubhtml?widget=true&amp;headers=false"
                        width="225" height="800" scrolling="auto" frameborder="1" title="ราคาเม็ดพลาสติกในตลาด" class="wrapper">ไม่มี ไอเฟรม (iFrame)</iframe>
                </li>
                <!-- Divider -->
                <hr class="sidebar-divider">
                <!-- Heading -->
            </ul>
            <!-- End of Sidebar -->

            <!-- Content Wrapper -->
            <div id="content-wrapper" class="d-flex flex-column">

                <!-- Main Content -->
                <div id="content">

                    <!-- Topbar -->
                    <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

                        <!-- Sidebar Toggle (Topbar) -->
                        <button id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
                            <i class="fa fa-bars"></i>
                        </button>

                        <!-- Topbar Search -->
                        <form class="d-none d-sm-inline-block form-inline mr-auto ml-md-3 my-2 my-md-0 mw-100 navbar-search">
                            <div class="input-group">
                                <input type="text" class="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2">
                                <div class="input-group-append">
                                    <button class="btn btn-primary" type="button" onclick="location.href='http://google.co.th';">
                                        <i class="fas fa-search fa-sm"></i>
                                    </button>
                                </div>
                            </div>
                        </form>

                        <!-- Topbar Navbar -->
                        <ul class="navbar-nav ml-auto">

                            <!-- Nav Item - Search Dropdown (Visible Only XS) -->
                            <li class="nav-item dropdown no-arrow d-sm-none">
                                <a class="nav-link dropdown-toggle" href="#" id="searchDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="fas fa-search fa-fw"></i>
                                </a>
                                <!-- Dropdown - Messages -->
                                <div class="dropdown-menu dropdown-menu-right p-3 shadow animated--grow-in" aria-labelledby="searchDropdown">
                                    <form class="form-inline mr-auto w-100 navbar-search">
                                        <div class="input-group">
                                            <input type="text" class="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2">
                                            <div class="input-group-append">
                                                <button class="btn btn-primary" type="button">
                                                    <i class="fas fa-search fa-sm"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </li>

                            <div class="topbar-divider d-none d-sm-block"></div>

                            <!-- Nav Item - User Information -->
                            <li class="nav-item dropdown no-arrow"></li>

                        </ul>

                    </nav>
                    <!-- End of Topbar -->

                    <!-- Begin Page Content -->
                    <div class="container-fluid">
                       <h1 class="h3 mb-2 text-gray-800">Actual Sales Report</h1>
                        <div class="card shadow mb-4">
                            <div class="card-header py-3">
                                <table>
                                    
                                    <tr>
                                        <td>

                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnGAmt" runat="server" OnClick="btnGAmt_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                                <asp:Image ID="Image3" runat="server" ImageUrl="~/img/receive-amount.png" Width="23px" />Gross Amt.
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnNAmt" runat="server" OnClick="btnNAmt_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/receive-amount.png" Width="23px" />Net Amt.
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnQty" runat="server" OnClick="btnQty_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/receive-amount.png" Width="23px" />Quantity
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                   
                                </table>
                            </div>
                            <div class="card-body">
                                <h6 class="m-0 font-weight-bold text-primary">
                                                <asp:Literal ID="lblTitle" runat="server"></asp:Literal></h6>
                                <div class="table-responsive">

                                  <%--  <asp:GridView ID="grdDash1" runat="server" AutoGenerateColumns="true" class="table table-bordered" Width="100%" CellSpacing="0"
                                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true">--%>
                                    <asp:GridView ID="grdDash1" runat="server" AutoGenerateColumns="true" class="" Width="100%" CellSpacing="0"
                                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                                        <HeaderStyle BackColor="#BD220D" Font-Bold="True" ForeColor="#FFFFFF" CssClass="header-center" Width="50px" />
                                        <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                                        <RowStyle BackColor="White" ForeColor="Blue" CssClass="" />
                                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                        <SortedDescendingHeaderStyle BackColor="#002876" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="card shadow mb-4">
                            <div class="card-header py-3">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <h6 class="m-0 font-weight-bold text-primary">
                                                <asp:Literal ID="lblTitle2" runat="server"></asp:Literal></h6>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <asp:GridView ID="grdDash2" runat="server" AutoGenerateColumns="true" class="" Width="50%" CellSpacing="0"
                                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                                        <HeaderStyle BackColor="#BD220D" Font-Bold="True" ForeColor="#FFFFFF" CssClass="header-center" Width="80px" />
                                        <PagerStyle BackColor="#99CCCC" ForeColor="#1F438F" HorizontalAlign="Left" />
                                        <RowStyle BackColor="White" ForeColor="Blue" CssClass="" />
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

                    <!-- /.container-fluid -->
                </div>
                <!-- End of Main Content -->

                <div>
                    <!-- Footer -->
                    <footer class="sticky-footer bg-white">
                        <div class="container my-auto">
                            <div class="copyright text-center my-auto">
                                <span>Copyright &copy; UNITED FOODS PUBLIC COMPANY LIMITED.</span>
                            </div>
                        </div>
                    </footer>
                    <!-- End of Footer -->
                </div>
            </div>
            <!-- End of Content Wrapper -->

        </div>
        <!-- End of Page Wrapper -->

        <!-- Scroll to Top Button-->
        <a class="scroll-to-top rounded" href="#page-top">
            <i class="fas fa-angle-up"></i>
        </a>
    </form>
</body>
</html>
