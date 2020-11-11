<%@ Page Title="Sales Monthly Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SaleSumMapDT.aspx.cs" Inherits="SaleSumMapDT" Async="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/1.10.9/css/dataTables.bootstrap.min.css" />
    <link type="text/css" rel="stylesheet" href="https://cdn.datatables.net/responsive/1.0.7/css/responsive.bootstrap.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/responsive/1.0.7/js/dataTables.responsive.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.9/js/dataTables.bootstrap.min.js"></script>
<%--    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            $('[id*=grdCustomerList]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": true,
                "sPaginationType": "full_numbers"
            });
        });

        function setResponsiveTable() {


        }

    </script>

    <style type="text/css">
        .auto-style1 {
            width: 79px;
            height: 82px;
        }

        .auto-style2 {
            font-size: x-large;
        }

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

        .line1_left {
            position: absolute;
            top: 150px;
            left: 10%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 35%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 55%;
            width: 20%;
            height: 30px;
            text-align: left;
        }

        .line1-2_right {
            position: absolute;
            top: 150px;
            left: 75%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line2_left {
            position: absolute;
            top: 195px;
            left: 10%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line2_center {
            position: absolute;
            top: 195px;
            left: 35%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line2_right {
            position: absolute;
            top: 195px;
            left: 55%;
            width: 20%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line2-2_right {
            position: absolute;
            top: 195px;
            left: 75%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line4_footer {
            position: absolute;
            top: 230px;
            left: 13%;
            width: 100%;
        }

        .line5_footer {
            position: absolute;
            top: 1000px;
            left: 15%;
            width: 100%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
        .header {
            position: absolute;
            top: 100px;
            left: 10%;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line1_left {
            position: absolute;
            top: 150px;
            left: 13%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line1_center {
            position: absolute;
            top: 150px;
            left: 38%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line1_right {
            position: absolute;
            top: 150px;
            left: 58%;
            width: 20%;
            height: 30px;
            text-align: left;
        }

        .line1-2_right {
            position: absolute;
            top: 150px;
            left: 78%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line2_left {
            position: absolute;
            top: 195px;
            left: 13%;
            width: 25%;
            height: 30px;
            text-align: right;
        }

        .line2_center {
            position: absolute;
            top: 195px;
            left: 38%;
            width: 20%;
            height: 30px;
            text-align: center;
        }

        .line2_right {
            position: absolute;
            top: 195px;
            left: 58%;
            width: 20%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line2-2_right {
            position: absolute;
            top: 195px;
            left: 82%;
            width: 25%;
            height: 30px;
            text-align: left;
            white-space: nowrap;
        }

        .line4_footer {
            position: absolute;
            top: 230px;
            left: 20%;
            width: 100%;
        }

        .line5_footer {
            position: absolute;
            top: 1000px;
            left: 18%;
            width: 100%;
        }
        }
    </style>
    <style type="text/css">
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
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>

            <div class="header">
                <asp:Label ID="Label2" runat="server" Text="Sales Monthly Report"></asp:Label>
            </div>
            <div class="line1_left">
                <asp:Label ID="Label1" runat="server" Text="เลือกสาขา : "></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="line1_center">
                <asp:Label ID="Label3" runat="server" Text="เลือกแวน : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlVan" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVan_SelectedIndexChanged"></asp:DropDownList>

            </div>
            <div class="line1_right">
                <asp:Label ID="Label4" runat="server" Text="เลือกตลาด : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlSaleArea" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlSaleArea_SelectedIndexChanged"></asp:DropDownList>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="line1-2_right">
        <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" OnClientClick="return setResponsiveTable()" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
        </asp:LinkButton>

        <%--<asp:Button ID="btnReport" runat="server" class="btn btn-primary btn-user btn-block" Text="Search" Width="150px" ValidationGroup="Report" OnClick="btnReport_Click" OnClientClick="return setResponsiveTable()" />--%>
    </div>
    <div class="line2_left">
        <asp:Label ID="Label5" runat="server" Text="Start Date : "></asp:Label>
        <asp:TextBox ID="txtStartDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

    </div>
    <div class="line2_center">
        <asp:Label ID="Label6" runat="server" Text="End Date : "></asp:Label>
        <asp:TextBox ID="txtEndDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

    </div>

    <div class="line2_right">
        <asp:Label ID="Label7" runat="server" Text="เลือกรูปแบบรายงาน : "></asp:Label>&nbsp;
        <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
    </div>
    <div class="line2-2_right">

        <asp:LinkButton ID="linkExportReport" runat="server" OnClick="btnDetails_Click" Visible="false">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/report.png" Width="23px" />Export Report
        </asp:LinkButton>

        <asp:LinkButton ID="linkAllLocation" runat="server" OnClick="LinkButton1_Click" Visible="false">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/placeholder.png" Width="23px" />Show All Location
        </asp:LinkButton>
    </div>

    <div class="line4_footer">
        <div style="width: 85%">
            <asp:GridView ID="grdCustomerList" runat="server" AutoGenerateColumns="false" class="table table-striped"
                BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                CellPadding="4" DataKeyNames="CustomerID" EmptyDataText="No records Found" Width="100%" OnRowCommand="grdCustomerList_RowCommand">
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
                    <%--<asp:TemplateField HeaderText="Map" SortExpression="CustomerID" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgBtnMarkerItem" runat="server" CommandArgument='<%#Eval("CustomerID") %>' CommandName="imgLocation" ImageUrl="~/img/placeholder.png" Width="30px" />

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ศูนย์" SortExpression="BranchName" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("BranchName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="แวน" SortExpression="VAN_ID" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblVAN_ID" runat="server" Text='<%# Bind("VAN_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ตลาด" SortExpression="SalAreaName" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblSalAreaName" runat="server" Text='<%# Bind("SalAreaName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ชื่อร้านค้า" SortExpression="CustName" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblCustName" runat="server" Text='<%# Bind("CustName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="เบอร์โทร" SortExpression="Telephone" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblTelephone" runat="server" Text='<%# Bind("Telephone") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ที่อยู่" SortExpression="AddressNo" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblAddressNo" runat="server" Text='<%# Bind("AddressNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ตำบล" SortExpression="District" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblDistrict" runat="server" Text='<%# Bind("District") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="อำเภอ" SortExpression="Area" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblArea" runat="server" Text='<%# Bind("Area") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="จังหวัด" SortExpression="Province" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblProvince" runat="server" Text='<%# Bind("Province") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ประเภทร้านค้า" SortExpression="ShopTypeName" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:Label ID="lblShopTypeName" runat="server" Text='<%# Bind("ShopTypeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Latitude" SortExpression="Latitude" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblLatitude" runat="server" Text='<%# Bind("Latitude") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Longitude" SortExpression="Longitude" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblLongitude" runat="server" Text='<%# Bind("Longitude") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="G Amt." SortExpression="GAmount" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <asp:Label ID="lblGAmount" runat="server" Text='<%# String.Format("{0, 0:N2}",DataBinder.Eval (Container.DataItem, "GAmount")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="N Amt." SortExpression="NAmount" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <asp:Label ID="lblNAmount" runat="server" Text='<%# String.Format("{0, 0:N2}",DataBinder.Eval (Container.DataItem, "NAmount")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right"/>
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# String.Format("{0, 0:N2}",DataBinder.Eval (Container.DataItem, "Quantity")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" SortExpression="FlagDel" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <asp:Label ID="lblFlagDel" runat="server" Text='<%# Bind("FlagDel") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                </Columns>

                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />


                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
        </div>
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

