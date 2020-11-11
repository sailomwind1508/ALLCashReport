<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransportationCost.aspx.cs" Inherits="TransportationCost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>



    <script type="text/javascript">
        $(function () {
            //$('[id*=grdTransportation]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            //    "responsive": true,
            //    "sPaginationType": "full_numbers",
            //    "pageLength": 10

            //});

            var pageMode = $('[id*=hfCurrentPage]').val();
            if (pageMode != null && pageMode != undefined && pageMode != "") {
                if (pageMode == "TB1") {
                    $('#tabs').tabs({ active: 0 });
                }
                else if (pageMode == "TB2") {
                    $('#tabs').tabs({ active: 1 });
                }
                else if (pageMode == "TB3") {
                    $('#tabs').tabs({ active: 2 });
                }
                else if (pageMode == "TB4") {
                    $('#tabs').tabs({ active: 3 });
                }
            }
        });

        var validate_save = true;
        function validate_save_trns() {
            var txtTranDate = $("#<%=txtTranDate.ClientID %>").val();
            var txtLicence = $("#<%=txtLicence.ClientID %>").val();
            var txtQuantity = $("#<%=txtQuantity.ClientID %>").val();
            var txtTotalQuantity = $("#<%=txtTotalQuantity.ClientID %>").val();
            var txtAmount = $("#<%=txtAmount.ClientID %>").val();

            if (txtTranDate == undefined || txtTranDate == "" || txtTranDate == null) {
                validate_save = false;
                alert("กรุณากรอก วันที่ขึ้นของ!!!");
                $("#<%=txtTranDate.ClientID %>").focus();
            }
            else if (txtLicence == undefined || txtLicence == "" || txtLicence == null) {
                validate_save = false;
                alert("กรุณากรอก ทะเบียนรถ!!!");
                $("#<%=txtLicence.ClientID %>").focus();
            }
            else if (txtQuantity == undefined || txtQuantity == "" || txtQuantity == null) {
                validate_save = false;
                alert("กรุณากรอก จำนวนหีบขนม!!");
                $("#<%=txtQuantity.ClientID %>").focus();
            }
            else if (txtTotalQuantity == undefined || txtTotalQuantity == "" || txtTotalQuantity == null) {
                validate_save = false;
                alert("กรุณากรอก จำนวนหีบทั้งหมด!!");
                $("#<%=txtTotalQuantity.ClientID %>").focus();
            }
            else if (txtAmount == undefined || txtAmount == "" || txtAmount == null) {
                validate_save = false;
                alert("กรุณากรอก จำนวนเงิน!!!");
                $("#<%=txtAmount.ClientID %>").focus();
            }
        }

        function alert_confirm_save() {
            validate_save = true;

            validate_save_trns();

            if (validate_save == true) {
                $('#dialog-confirm-save').dialog({
                    resizable: false,
                    height: 200,
                    width: 400,
                    modal: true,
                    buttons: {
                        "Save": function () {
                            __doPostBack("save", "");
                            $(this).dialog("close");
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }

        function alert_confirm_remove(obj) {

            $('#dialog-confirm-remove').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    "Remove": function () {
                        __doPostBack("remove", obj);
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function closeDialog() {
            try {
                setTimeout(function () {
                    //location.reload();
                    $('#wait_dialog').hide();
                    $('#wait_dialog').dialog("close");
                    $('#wait_dialog').dialog("destroy");
                }, 10000);
            } catch (e) {
                alert(e);
            }
        }

        function ActiveTab(obj) {
            if (obj == 1) {
                $('[id*=hfCurrentPage]').val("TB1");
            }
            else if (obj == 2) {
                $('[id*=hfCurrentPage]').val("TB2");
            }
            else if (obj == 3) {
                $('[id*=hfCurrentPage]').val("TB3");
            }
            else if (obj == 4) {
                $('[id*=hfCurrentPage]').val("TB4");
            }
        }

        function SaveResult(obj) {

            $('#dialog-message-save').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            ActiveTab(obj)
        }

        function ValidateSave() {

            $('#dialog-validate-save').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function ShowImportFile() {

            $('#dialog-import-file').dialog({
                resizable: false,
                height: 300,
                width: 650,
                modal: true,
                buttons: {}
            });
        }

        function ShowResultUpload() {
            $('#dialog-message-upload').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        //window.location.href = "TransportationCost.aspx";
                    }
                }
            });

            ActiveTab(4)
        }

        function ImportSalesTarget() {

            var path = $("#txtImportFile").val();
            alert(path);
            __doPostBack("import_target", path);
        }

    </script>

    <style type="text/css">
        .header {
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 170px;
            width: 70%;
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
            <asp:Image ID="Image11" runat="server" ImageUrl="~/img/delivery-truck.png" Width="50px" />
            <asp:Label ID="Label7" runat="server" Text="Transportation Cost" CssClass="text-white"></asp:Label>
        </div>
    </div>

    <div class="line4_footer">
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
                    font-family: Calibri;
                }
            </style>
            <div id="tabs" style="width: 120%; height: 100%; font-size: 14px;">
                <ul>
                    <li><a href="#tabs-1"><asp:Image ID="Image18" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;รายงาน</a></li>
                    <li><a href="#tabs-2"><asp:Image ID="Image8" runat="server" ImageUrl="~/img/delivery.png" Width="23px" Height="25px" />&nbsp;รายละเอียดการขนส่ง</a></li>
                    <li><a href="#tabs-3"><asp:Image ID="Image9" runat="server" ImageUrl="~/img/add-file.png" Width="23px" Height="25px" />&nbsp;เพิ่ม/แก้ไขรายละเอียดการขนส่ง</a></li>
                    <li><a href="#tabs-4"><asp:Image ID="Image10" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" />&nbsp;อัตราค่าขนส่ง-นำเข้า Excel</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td colspan="5">
                                    <table style="width: 600px;">
                                        <tr>
                                            <td style="width: 100px; text-align: left">รูปแบบรายงาน : 
                                            </td>
                                            <td style="width: 180px; text-align: left">
                                                <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 100px; text-align: left">สาขา : </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSalesAreaR" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnExportExcel" runat="server" Visible="false" OnClick="btnExportExcel_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SOA">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export Excel
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; text-align: left">วันที่เริ่มต้น : 
                                            </td>
                                            <td style="width: 180px; text-align: left">
                                                <asp:TextBox ID="txtTranDate1" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtTranDate1" runat="server" ErrorMessage="*" ControlToValidate="txtTranDate1" ValidationGroup="SR1" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </td>
                                            <td style="width: 100px; text-align: left">วันที่สิ้นสุด : </td>
                                            <td style="width: 180px; text-align: left">
                                                <asp:TextBox ID="txtTranDate2" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtTranDate2" runat="server" ErrorMessage="*" ControlToValidate="txtTranDate2" ValidationGroup="SR1" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </td>
                                            <td style="width: 180px; text-align: left">

                                                <asp:LinkButton ID="btnSR1" runat="server" OnClick="btnSR1_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SR1">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                                </asp:LinkButton>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:UpdatePanel ID="UpdatePanel1"
                                        runat="server">
                                        <ContentTemplate>
                                            <div style="width: 100%; font-size: 14px;">

                                                <asp:GridView ID="grdTransReport1" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                    HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="15" AllowPaging="true" OnRowDataBound="grdTransReport1_RowDataBound"
                                                    CellPadding="4" DataKeyNames="ลำดับ" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdTransReport1_Sorting" OnPageIndexChanging="grdTransReport1_PageIndexChanging"
                                                    OnRowCreated="grdTransReport1_RowCreated">
                                                    <Columns>
                                                        <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" SortExpression="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="เดือน/ปี" HeaderText="เดือน/ปี" SortExpression="เดือน/ปี" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="วันที่" HeaderText="วันที่" SortExpression="วันที่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" SortExpression="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="สาขา" HeaderText="สาขา" SortExpression="สาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="Supplier" HeaderText="Supplier" SortExpression="Supplier" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="ประเภทรถ" HeaderText="ประเภทรถ" SortExpression="ประเภทรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="จำนวนรถ" HeaderText="จำนวนรถ" SortExpression="จำนวนรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="อัตราค่าขนส่ง" HeaderText="อัตราค่าขนส่ง" SortExpression="อัตราค่าขนส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนเงินค่าขนส่ง" HeaderText="จำนวนเงินค่าขนส่ง" SortExpression="จำนวนเงินค่าขนส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบขนมจาก admin" HeaderText="จำนวนหีบขนมจาก admin" SortExpression="จำนวนหีบขนมจาก admin" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบขนม" HeaderText="จำนวนหีบขนม" SortExpression="จำนวนหีบขนม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบทั้งหมด" HeaderText="จำนวนหีบทั้งหมด" SortExpression="จำนวนหีบทั้งหมด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="ค่าขนส่งต่อหีบขนม" HeaderText="ค่าขนส่งต่อหีบขนม" SortExpression="ค่าขนส่งต่อหีบขนม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                        <asp:BoundField DataField="ค่าขนส่งต่อหีบทั้งหมด" HeaderText="ค่าขนส่งต่อหีบทั้งหมด" SortExpression="ค่าขนส่งต่อหีบทั้งหมด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                    </Columns>
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>

                                                <asp:GridView ID="grdTransReport2" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                    HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="80" AllowPaging="true"
                                                    CellPadding="4" DataKeyNames="ลำดับ" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="false" OnSorting="grdTransReport2_Sorting" OnPageIndexChanging="grdTransReport2_PageIndexChanging" OnRowDataBound="grdTransReport2_RowDataBound"
                                                    OnRowCreated="grdTransReport2_RowCreated">
                                                    <Columns>
                                                        <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" SortExpression="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="วันที่" HeaderText="วันที่" SortExpression="วันที่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" SortExpression="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="สาขา" HeaderText="สาขา" SortExpression="สาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="รหัสสินค้า" HeaderText="รหัสสินค้า" SortExpression="รหัสสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="สินค้า" HeaderText="สินค้า" SortExpression="สินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="Supplier" HeaderText="Supplier" SortExpression="Supplier" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="ประเภทรถ" HeaderText="ประเภทรถ" SortExpression="ประเภทรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="จำนวนรถ" HeaderText="จำนวนรถ" SortExpression="จำนวนรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="อัตราค่าขนส่ง" HeaderText="อัตราค่าขนส่ง" SortExpression="อัตราค่าขนส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบขนม" HeaderText="จำนวนหีบขนม" SortExpression="จำนวนหีบขนม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบทั้งหมด" HeaderText="จำนวนหีบทั้งหมด" SortExpression="จำนวนหีบทั้งหมด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="ค่าขนส่งต่อ sku" HeaderText="ค่าขนส่งต่อ sku" SortExpression="ค่าขนส่งต่อ sku" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                    </Columns>
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>

                                                <asp:GridView ID="grdTransReport3" runat="server" Visible="false" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                    HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="80" AllowPaging="true" OnRowDataBound="grdTransReport3_RowDataBound"
                                                    CellPadding="4" DataKeyNames="ลำดับ" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdTransReport3_Sorting" OnPageIndexChanging="grdTransReport3_PageIndexChanging"
                                                    OnRowCreated="grdTransReport3_RowCreated">
                                                    <Columns>
                                                        <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" SortExpression="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="วันที่" HeaderText="วันที่" SortExpression="วันที่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" SortExpression="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="สาขา" HeaderText="สาขา" SortExpression="สาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="Supplier" HeaderText="Supplier" SortExpression="Supplier" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="ประเภทรถ" HeaderText="ประเภทรถ" SortExpression="ประเภทรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="อัตราค่าขนส่ง" HeaderText="อัตราค่าขนส่ง" SortExpression="อัตราค่าขนส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="จำนวนหีบขนมจาก admin" HeaderText="จำนวนหีบขนมจาก admin" SortExpression="จำนวนหีบขนมจาก admin" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="จำนวนหีบขนม" HeaderText="จำนวนหีบขนม" SortExpression="จำนวนหีบขนม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="สถานะ" HeaderText="สถานะ" SortExpression="สถานะ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    </Columns>
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-2">
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td style="text-align: left">
                                    <asp:Label ID="Label13" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                
                                    <asp:DropDownList ID="ddlSalesAreaDT" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesAreaDT_SelectedIndexChanged"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                   <%-- <asp:Label ID="Label15" runat="server" Text="วันที่ขึ้นของ : "></asp:Label>&nbsp;
                                
                                    <asp:TextBox ID="txtTranDateDT" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" OnClientClick="return RegisDateDT();" ></asp:TextBox>
                                    --%>
                                    <asp:Label ID="Label15" runat="server" Text="วันที่ขึ้นของ(จาก) : "></asp:Label>&nbsp;
                                
                                    <asp:TextBox ID="txtTranDateDT_F" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" OnClientClick="return RegisDateDT();" ></asp:TextBox>
                                    
                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label6" runat="server" Text="วันที่ขึ้นของ(ถึง) : "></asp:Label>&nbsp;
                                
                                    <asp:TextBox ID="txtTranDateDT_T" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" OnClientClick="return RegisDateDT();" ></asp:TextBox>

                                    
                                
                            </tr>
                           
                            <tr style="white-space: nowrap;">
                                <td style="text-align: left">
                                    
                                    <asp:Label ID="Label9" runat="server" Text="ทะเบียนรถ : "></asp:Label>&nbsp;
                              
                                    <asp:TextBox ID="txtLicenceDT" runat="server" Width="150px" MaxLength="50" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label12" runat="server" Text="ประเภทรถ : "></asp:Label>&nbsp;
                               
                                    <asp:DropDownList ID="ddlCarTypeDT" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>


                                    <asp:Label ID="Label10" runat="server" Text="เลขที่เอกสาร : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlDocNoDT" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="rdoPOTR" runat="server" Text="เลขที่ทั้งหมด" Checked="true" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="rdoPOTR_CheckedChanged" GroupName="rdoDocType" />
                                    <asp:RadioButton ID="rdoPO" runat="server" Text="เลขที่ PO" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="rdoPO_CheckedChanged" GroupName="rdoDocType" />
                                    <asp:RadioButton ID="rdoTR" runat="server" Text="เลขที่ TR" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="rdoTR_CheckedChanged" GroupName="rdoDocType" />
                                    
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnSearchDT" runat="server" OnClick="btnSearchDT_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SearchDT">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                    </asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnExportDTExcel" runat="server" Visible="false" OnClick="btnExportDTExcel_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SearchDT">
                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export Excel
                                    </asp:LinkButton>
                                </td>
                            </tr>

                            <tr style="white-space: nowrap;">
                                <td colspan="6">
                                    <div style="width: 100%; font-size: 14px;">

                                                <asp:GridView ID="grdTransportationDT" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                    HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="15" AllowPaging="true"
                                                    CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true"
                                                    OnPageIndexChanging="grdTransportationDT_PageIndexChanging" OnRowCreated="grdTransportationDT_RowCreated"
                                                    OnSorting="grdTransportationDT_Sorting" OnRowDataBound="grdTransportationDT_RowDataBound" OnRowCommand="grdTransportationDT_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="ROW_NO" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="SalesAreaCode" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="SalesArea" HeaderText="สาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="TranDate" HeaderText="วันที่ขึ้นของ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <asp:BoundField DataField="Licence" HeaderText="ทะเบียนรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="Supplier" HeaderText="Supplier" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="DocNo" HeaderText="เลขที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="CarType" HeaderText="ประเภทรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="Quantity" HeaderText="จำนวนหีบขนม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                        <asp:BoundField DataField="TotalQuantity" HeaderText="จำนวนหีบทั้งหมด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                        <asp:BoundField DataField="Amount" HeaderText="จำนวนเงิน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="Remark" HeaderText="หมายเหตุ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" 
                                                            CommandName="imgEdit" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                            HeaderText="แก้ไข"
                                                            ImageUrl="~/img/document.png" />
                                                        <asp:TemplateField HeaderText="ลบ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <a id="btnRemove" onclick="return alert_confirm_remove(<%# Eval("PK").ToString()%>);" href="#" title="" style="width: 80px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/x-button.png" Width="23px" Height="20px" />
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- 
                                                <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="imgRemove" DataTextField="PK" ControlStyle-CssClass="imgWidthDel"
                                                    HeaderText="ลบ"
                                                    ImageUrl="~/img/x-button.png" />--%>
                                                    </Columns>

                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="tabs-3">
                    <div>
                        <table style="width: 80%">
                            <tr style="height: 30px; white-space: nowrap;">

                                <td style="width: 30%; text-align: right;">รหัสสาขา : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:DropDownList ID="ddlSalesAreaCode_E" runat="server" Height="30px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesAreaCode_E_SelectedIndexChanged"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr style="height: 30px; white-space: nowrap;">
                                <td style="width: 30%; text-align: right;">วันที่ขึ้นของ : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtTranDate" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtTranDate" runat="server" ErrorMessage="*" ControlToValidate="txtTranDate" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr style="height: 30px; white-space: nowrap;">
                                <td style="width: 30%; text-align: right;">ทะเบียนรถ : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtLicence" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtLicence" runat="server" ErrorMessage="*" ControlToValidate="txtLicence" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: right;"></td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:CheckBox ID="chkDocNoCon" runat="server" Checked="false" Text="กรอกข้อมูลเอง" /></td>
                            </tr>
                            <tr style="white-space: nowrap; height: 30px;">
                                <td style="white-space: nowrap; width: 30%; text-align: right;">เลขที่เอกสาร : </td>
                                <td style="width: 5%;"></td>
                                <td style="white-space: nowrap; width: 50%;">
                                    <table>

                                        <tr>
                                            <td>
                                                <div id="showChkDocNo" visible="true">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDocNo" runat="server" Height="30px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdoPO_E" runat="server" Text="เลขที่ PO" Checked="true" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="rdoPO_E_CheckedChanged" GroupName="rdoDocTypeE" />
                                                                <asp:RadioButton ID="rdoTR_E" runat="server" Text="เลขที่ TR" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="rdoTR_E_CheckedChanged" GroupName="rdoDocTypeE" />
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td>ค้นหาตามวันที่ : &nbsp;
                                                                <asp:TextBox ID="txtFilterDate" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30" AutoPostBack="true" OnTextChanged="txtTranDate_TextChanged"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                                <div id="showTxtDocNo" visible="false">
                                                    <asp:TextBox ID="txtDocNo_E" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">ประเภทรถ : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:DropDownList ID="ddlCarType_E" runat="server" Height="30px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">Supplier : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:DropDownList ID="ddlSupplier_E" runat="server" Height="30px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtSupplier" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="150"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">จำนวนหีบขนม : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtQuantity" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="8" Style="text-align: right;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtQuantity" runat="server" ErrorMessage="*" ControlToValidate="txtQuantity" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">จำนวนหีบทั้งหมด : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtTotalQuantity" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="8" Style="text-align: right;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtTotalQuantity" runat="server" ErrorMessage="*" ControlToValidate="txtTotalQuantity" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">จำนวนเงิน : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtAmount" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="20" Style="text-align: right;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ErrorMessage="*" ControlToValidate="txtAmount" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td style="width: 30%; text-align: right;">หมายเหตุ : </td>
                                <td style="width: 5%;"></td>
                                <td style="width: 50%;">
                                    <asp:TextBox ID="txtRemark" runat="server" Width="230px" ViewStateMode="Enabled" EnableViewState="true" Height="56px" TextMode="MultiLine" MaxLength="255"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                    <asp:HiddenField ID="hidPK" runat="server" />
                                </td>
                            </tr>
                            <tr style="height: 30px; white-space: nowrap; text-align: center;">
                                <td></td>
                                <td></td>
                                <td style="white-space: nowrap;">
                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <a id="btnSave" onclick="return alert_confirm_save();" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                                    <asp:Image ID="imgSave" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" Height="25px" />&nbsp;บันทึก
                                                </a>
                                                <%--<asp:LinkButton ID="btnSave" runat="server"  OnClick="btnSave_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SaveTranp">
                                                    <asp:Image ID="imgSave" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />บันทึก
                                                </asp:LinkButton>--%>
                                            </td>
                                            <td>
                                                <%--<a id="btnClear" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/img/x-button.png" Width="23px" Height="25px" />&nbsp;ยกเลิก
                                                </a>--%>
                                                <asp:LinkButton ID="btnClear" runat="server" OnClick="btnClear_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                                    <asp:Image ID="imgClear" runat="server" ImageUrl="~/img/x-button.png" Width="23px" />&nbsp;ยกเลิก
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
                <div id="tabs-4">
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="6"></td>
                            </tr>

                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label3" runat="server" Text="สาขา : "></asp:Label>&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlSalesArea" runat="server" Height="25px" Width="230px" class="dropdown-divider"></asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label4" runat="server" Text="เดือน : "></asp:Label>&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label5" runat="server" Text="ปี : "></asp:Label>&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label1" runat="server" Text="ประเภทรถ : "></asp:Label>&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlCarType" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label2" runat="server" Text="Suppiler : "></asp:Label>&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlSuppiler" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Search">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    
                                    <button type="button" class="btn btn-primary btn-user btn-block excelButtom" data-toggle="modal" data-target="#myModal">
                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />Import Excel
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">

                                    <asp:UpdatePanel ID="UpdatePanel4"
                                        runat="server">
                                        <ContentTemplate>
                                            <div style="width: 100%; font-size: 14px;">

                                                <asp:GridView ID="grdTransportation" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                    HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows"
                                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="15" AllowPaging="true"
                                                    CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true"
                                                    OnPageIndexChanging="grdTransportation_PageIndexChanging" OnRowCreated="grdTransportation_RowCreated"
                                                    OnSorting="grdTransportation_Sorting" OnRowDataBound="grdTransportation_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ROW_NO" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="SalesAreaCode" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="SalesArea" HeaderText="สาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <asp:BoundField DataField="Month" HeaderText="เดือน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="Year" HeaderText="ปี" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="CarType" HeaderText="ประเภทรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                        <asp:BoundField DataField="ShippingCost" HeaderText="ค่าขนส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                        <asp:BoundField DataField="Supplier" HeaderText="Suppiler" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                        <%--<asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="imgEdit" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                    HeaderText="แก้ไข"
                                                    ImageUrl="~/img/document.png" />
                                                <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="imgRemove" DataTextField="PK" ControlStyle-CssClass="imgWidthDel"
                                                    HeaderText="ลบ"
                                                    ImageUrl="~/img/x-button.png" />--%>
                                                    </Columns>

                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfCurrentPage" runat="server" />
    <div id="dialog-message-save" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Save successful!
        </p>

    </div>
    <div id="dialog-confirm-remove" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Do you want to remove?
        </p>

    </div>
    <div id="dialog-confirm-save" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Do you want to save?
        </p>

    </div>

    <div id="dialog-validate-save" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            กรุณากรอกข้อมูลให้เรียบร้อยก่อนบันทึกข้อมูล
        </p>
    </div>
    <div id="dialog-message-upload" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            <asp:Label ID="lblUploadResult" runat="server" Text=""></asp:Label>

        </p>
    </div>

    <div class="card-body">
        <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Import Excel File</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <label>Choose excel</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">เลือกข้อมูลที่ต้องการนำเข้า : </td>
                                        </tr>
                                        <tr style="white-space: nowrap">
                                            <td>
                                                <asp:CheckBox ID="chkTransportation" runat="server" Text="sheet Transportation" Checked="true" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkTransportationDT" runat="server" Text="sheet Transportation_Details" />
                                            </td>

                                        </tr>

                                    </table>
                                    <br />
                                    <label>Choose excel file</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />

                                        </div>

                                        <div class="input-group-append">
                                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-outline-primary" Text="Upload" OnClick="btnUpload_Click" />
                                        </div>
                                    </div>
                                    <br />
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $("#<%=txtTranDate1.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTranDate2.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTranDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

       <%-- $("#<%=txtTranDateDT.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });--%>

        $("#<%=txtTranDateDT_F.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTranDateDT_T.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtFilterDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        function checkfile(sender) {
            var validExts = new Array(".xlsx", ".xls");
            var fileExt = sender.value;
            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
            if (validExts.indexOf(fileExt) < 0) {
                alert("Invalid file selected, valid files are of " +
                    validExts.toString() + " types.");
                return false;
            }
            else return true;
        }

        $(function () {
            validateShowEditDocNo();

            $("#ContentPlaceHolder1_txtQuantity").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d]/g, ''));
            });

            $("#ContentPlaceHolder1_txtTotalQuantity").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d]/g, ''));
            });

            $("#ContentPlaceHolder1_txtAmount").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d.]/g, ''));
            });

            $("#<%=chkDocNoCon.ClientID %>").click(function () {
                validateShowEditDocNo();
            });

        });

        function validateShowEditDocNo() {
            if ($("#<%=chkDocNoCon.ClientID %>").is(':checked')) {
                showTxt();
            }
            else {
                showChk();
            }
        }

        function showTxt() {
            $("#showChkDocNo").hide();
            $("#showTxtDocNo").show();
        }

        function showChk() {
            $("#showChkDocNo").show();
            $("#showTxtDocNo").hide();
        }

    </script>
</asp:Content>

