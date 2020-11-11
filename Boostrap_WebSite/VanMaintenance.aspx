<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VanMaintenance.aspx.cs" Inherits="VanMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {

            var hidHis = $('[id*=hidHistory]').val();

            if (hidHis == "1") {
                $("#history").show();
            }
            else {
                $("#history").hide();
            }

            var pageMode = $('[id*=hfCurrentPage]').val();
            if (pageMode != null && pageMode != undefined && pageMode != "") {
                if (pageMode == "TB1") {
                    $('#tabs').tabs({ active: 0 });
                }
                if (pageMode == "TB2") {
                    $('#tabs').tabs({ active: 1 });
                }
                if (pageMode == "TB3") {
                    $('#tabs').tabs({ active: 2 });
                }
                if (pageMode == "TB4") {
                    $('#tabs').tabs({ active: 3 });
                }
            }

            $("[id*=ContentPlaceHolder1_grdEditWH_txtChestQty]").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d.]/g, ''));
            });

            $("[id*=ContentPlaceHolder1_grdEditPO_txtOrderQty]").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d.]/g, ''));
            });
        });


        var validate_save = true;
        function validate_save_trns() {
            var _txtTransferDate = $("#<%=txtTransferDate.ClientID %>").val();

            if (_txtTransferDate == undefined || _txtTransferDate == "" || _txtTransferDate == null) {
                validate_save = false;
                alert("กรุณากรอก วันที่!!!");
                $("#<%=txtTransferDate.ClientID %>").focus();
            }
        }

        function showHistory(obj) {

            if (obj.checked) {

                $("#history").show();
                $('[id*=hidHistory]').val("1");
            }
            else {
                $("#history").hide();
                $('[id*=hidHistory]').val("0");
            }
        }

        function alert_confirm_save() {
            validate_save = true;

            validate_save_trns();

            if (validate_save == true) {
                $('#dialog-confirm-send').dialog({
                    resizable: false,
                    height: 200,
                    width: 400,
                    modal: true,
                    buttons: {
                        ส่ง: function () {
                            __doPostBack("upload", "");
                            $(this).dialog("close");
                        },
                        ยกเลิก: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }

        function getGridViewData(_grid) {

            var grdData = "";
            var grid = _grid;

            $("#" + _grid.id + " tr").each(function () {
                $this = $(this);

                var qty = "";
                var pk = "";

                if ($this.find('input[type=text]').val() != undefined) {
                    qty = $this.find('input[type=text]').val();
                }

                if ($this.find('input[type=text]').val() != undefined) {
                    pk = $this.find('input[type=hidden]').val();
                }

                if (pk != undefined && pk != "") {
                    grdData += pk + "|" + qty + ",";
                }

            });

            return grdData;
        }

        function closeDialog() {
            try {
                setTimeout(function () {
                    $('#wait_dialog').hide();
                    $('#wait_dialog').dialog("close");
                    $('#wait_dialog').dialog("destroy");
                }, 1000);
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

        function VerifyExportExcel(obj) {

            $('#dialog-message-verify-doc').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            ActiveTab(obj)
        }

        function ExcelResult(obj) {

            $('#dialog-message-excel').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            ActiveTab(obj)
        }

        function ShowDT(obj) {

            var dlg = $("#dialog-oil-dt"); // Get the dialog container.

            var width = $(window).width();
            var height = $(window).height();

            // Provide some space between the window edges.
            width = width - 10;
            height = height - 10; // iframe height will need to be even less to account for space taken up by dialog title bar, buttons, etc.

            // Set the iframe height.
            $(dlg.children("iframe").get(0)).css("height", height + "px");

            dlg.dialog({
                resizable: true,
                height: height,
                width: width,
                modal: true
            });

            ActiveTab(obj);
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

        function ShowErrSave(obj) {

            var dlg = $("#dialog-edit-po"); // Get the dialog container.

            var width = $(window).width();
            var height = $(window).height();

            // Provide some space between the window edges.
            width = width - 50;
            height = height - 150; // iframe height will need to be even less to account for space taken up by dialog title bar, buttons, etc.

            // Set the iframe height.
            $(dlg.children("iframe").get(0)).css("height", height + "px");

            dlg.dialog({
                resizable: true,
                height: height,
                width: width,
                modal: true
            });


            $('#dialog-message-upload').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        //window.location.href = "TransportationCost.aspx";
                    }
                }
            });

            ActiveTab(obj)
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

            ActiveTab(1);
        }

        function ImportSalesTarget() {

            var path = $("#txtImportFile").val();
            alert(path);
            __doPostBack("import_target", path);
        }

    </script>

   <style type="text/css">
        .pointer {
            cursor: pointer;
        }

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
            white-space: nowrap;
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

    <div class="card" style="width: 100%;">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image11" runat="server" ImageUrl="~/img/maintenance.png" Width="50px" />
            <asp:Label ID="Label14" runat="server" Text="รายงานค่าซ่อมบำรุงรถ" CssClass="text-white"></asp:Label>
        </div>
    </div>
    <div style="width: 80%; position: absolute;">

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
        <div id="tabs" style="width: 100%; font-family: Verdana,Arial,sans-serif; font-size: 14px;">
            <ul>
                <li><a href="#tabs-1">
                    <asp:Image ID="Image16" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" />&nbsp;ส่งข้อมูล Excel</a></li>
                <li><a href="#tabs-2">
                    <asp:Image ID="Image19" runat="server" ImageUrl="~/img/paper-plane.png" Width="23px" Height="25px" />&nbsp;สถานะการส่ง</a></li>
                <li><a href="#tabs-3">
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/img/document.png" Width="23px" Height="25px" />&nbsp;ข้อมูลค่าซ่อมบำรุง</a></li>
                <li><a href="#tabs-4">
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/img/top-three.png" Width="23px" Height="25px" />&nbsp;Ranking Report</a></li>
            </ul>

            <div id="tabs-1" style="width: 100%">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Import Excel</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" /><label>เลือกไฟล์ Excel ค่าซ่อมบำรุง</label>

                                    <table>
                                        <tr style="white-space: nowrap;">
                                            <td style="text-align: right">
                                                <asp:Label ID="Label2" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                              
                                                    <asp:DropDownList ID="ddlBranch" runat="server" Height="25px" Width="230px" class="dropdown-divider" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                                &nbsp;&nbsp;

                                                    <asp:Label ID="Label1" runat="server" Text="วันที่ส่ง : "></asp:Label>&nbsp;
                                               
                                                    <asp:TextBox ID="txtTransferDate" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>

                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" Height="50px" />

                                        </div>
                                    </div>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <a id="btnSend" onclick="return alert_confirm_save();" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                            <asp:Image ID="imgSave" runat="server" ImageUrl="~/img/paper-plane.png" Width="23px" Height="25px" />&nbsp;Send
                        </a>
                    </div>

                </div>
            </div>

            <div id="tabs-2">
                <div style="width: 100%;" runat="server" id="tb2">
                    <div class="modal-content">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td style="width: 250px; text-align: left">
                                    <div class="modal-header">

                                        <table>
                                            <tr style="white-space: nowrap;">
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label10" runat="server" Text="สถานะการส่ง : " />&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlDocStatus" runat="server" Height="25px" Width="120px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                        <asp:Label ID="Label4" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlBranch_R" runat="server" Height="25px" Width="230px" class="dropdown-divider" OnSelectedIndexChanged="ddlBranch_R_SelectedIndexChanged" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                    
                                                        <asp:Label ID="Label6" runat="server" Text="วันที่ส่งเอกสาร : "></asp:Label>&nbsp;
                                                    
                                                        <asp:TextBox ID="txtTransferDate_R" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnSearchStatus" runat="server" OnClick="btnSearchStatus_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Search">
                                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                        </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                            
                                                        <asp:LinkButton ID="btnExportExcel" runat="server" Visible="false" OnClick="btnExportExcel_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Search">
                                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                                        </asp:LinkButton>

                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                        </table>

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="width: 100%">
                                    <div class="modal-body" style="width: 100%; font-size: 14px;">

                                        <asp:GridView ID="grdStatus" runat="server" AutoGenerateColumns="true" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="50" AllowPaging="true" OnRowCommand="grdStatus_RowCommand"
                                            CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdStatus_Sorting" OnPageIndexChanging="grdStatus_PageIndexChanging" OnRowDataBound="grdStatus_RowDataBound">
                                            <Columns>

                                                <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="report" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                    HeaderText="รายละเอียด"
                                                    ImageUrl="~/img/search.png" />


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
            </div>

            <div id="tabs-3">
                <div style="width: 100%;" runat="server" id="tb3">
                    <div class="modal-content">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td>
                                    <div class="modal-header">

                                        <table>
                                            <tr style="white-space: nowrap;">
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label7" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                   
                                                        <asp:DropDownList ID="ddlBranch_DT" runat="server" Height="25px" Width="230px" class="dropdown-divider" OnSelectedIndexChanged="ddlBranch_DT_SelectedIndexChanged" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                        <asp:Label ID="Label8" runat="server" Text="แวน : "></asp:Label>&nbsp;
                                                   
                                                        <asp:DropDownList ID="ddlVan_DT" runat="server" Height="25px" Width="120px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkHistory" runat="server" Text="ดูประวัติการส่ง" onclick="return showHistory(this)" ViewStateMode="Enabled" EnableViewState="true" CssClass="pointer" />

                                                    &nbsp;&nbsp;
                                                    
                                                        <asp:LinkButton ID="btnSearchDT" runat="server" OnClick="btnSearchDT_Click" OnClientClick="return showHistory(this)" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SearchDT">
                                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                        </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                           
                                                        <asp:LinkButton ID="btnExcel_DT" runat="server" Visible="false" OnClick="btnExcel_DT_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SearchDT">
                                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                                        </asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="history" style="display: none;">

                                                        <asp:HiddenField ID="hidHistory" runat="server" />
                                                        <asp:Label ID="Label9" runat="server" Text="วันที่ส่ง(จาก) : "></asp:Label>&nbsp;
                                                    
                                                            <asp:TextBox ID="txtTransferDateF_DT" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                            <asp:Label ID="Label13" runat="server" Text="วันที่ส่ง(ถึง) : "></asp:Label>&nbsp;
                                                    
                                                            <asp:TextBox ID="txtTransferDateT_DT" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </td>

                            </tr>
                            <tr>
                                <td colspan="6" style="width: 100%">
                                    <div class="modal-body" style="width: 100%; font-size: 14px;">

                                        <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" CssClass="" PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="50" AllowPaging="true"
                                            CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCommand="grdDetails_RowCommand"
                                             OnSorting="grdDetails_Sorting" OnRowDataBound="grdDetails_RowDataBound" OnPageIndexChanging="grdDetails_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="วันที่ส่ง" HeaderText="วันที่ส่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="วันที่" HeaderText="วันที่ซ่อม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="รหัสศูนย์" HeaderText="รหัสศูนย์" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="ชื่อศูนย์" HeaderText="ชื่อศูนย์" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="แวน" HeaderText="แวน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="ทะเบียนรถ" HeaderText="ทะเบียนรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="Supplier" HeaderText="Supplier" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="รายละเอียดการตรวจเช็ค" HeaderText="รายละเอียดการตรวจเช็ค" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="หมายเหตุอื่น" HeaderText="หมายเหตุอื่น" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                               
                                                <asp:BoundField DataField="มูลค่าสินค้าหรือบริการ" HeaderText="มูลค่าสินค้าหรือบริการ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="จำนวนเงินภาษีมูลค่าเพิ่ม" HeaderText="จำนวนเงินภาษีมูลค่าเพิ่ม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="จำนวนเงินรวม" HeaderText="จำนวนเงินรวม" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                
                                                <asp:BoundField DataField="ไมล์ เริ่มต้น(จากการเข้าตรวจครั้งก่อน)" HeaderText="ไมล์ เริ่มต้น(จากการเข้าตรวจครั้งก่อน)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                <asp:BoundField DataField="ไมล์ สิ้นสุด(ณ วันที่นำรถเช้าศูนย์)" HeaderText="ไมล์ สิ้นสุด(ณ วันที่นำรถเช้าศูนย์)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                <asp:BoundField DataField="จำนวนไมล์ใช้งาน" HeaderText="จำนวนไมล์ใช้งาน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                           
                                                <asp:BoundField DataField="แก้ไขโดย" HeaderText="แก้ไขโดย" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="วันที่แก้ไขล่าสุด" HeaderText="วันที่แก้ไขล่าสุด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
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
            </div>

            <div id="tabs-4">
                <div style="width: 100%;" runat="server" id="tb4">
                    <div class="modal-content">
                        <table style="width: 100%;">
                            <tr style="white-space: nowrap;">
                                <td style="width: 250px; text-align: left">
                                    <div class="modal-header">

                                        <table>
                                            <tr style="white-space: nowrap;">
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label3" runat="server" Text="Ranking Type : " />&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlRankingType" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlRankingType_SelectedIndexChanged"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                        <asp:Label ID="Label5" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlBranch_Rank" runat="server" Height="25px" Width="260px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label11" runat="server" Text="วันที่ซ่อมบำรุง(จาก) : "></asp:Label>&nbsp;
                                                    
                                                        <asp:TextBox ID="txtDateFrom" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                    &nbsp;&nbsp;
                                                        <asp:Label ID="Label12" runat="server" Text="วันที่ซ่อมบำรุง(ถึง) : "></asp:Label>&nbsp;
                                                    
                                                        <asp:TextBox ID="txtDateTo" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>

                                                    &nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnSearch_Rank" runat="server" OnClick="btnSearch_Rank_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Search">
                                                            <asp:Image ID="Image8" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                        </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                            
                                                        <asp:LinkButton ID="btnExport_Rank" runat="server" Visible="false" OnClick="btnExport_Rank_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="Search">
                                                            <asp:Image ID="Image9" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                                        </asp:LinkButton>

                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                        </table>

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="width: 100%">
                                    <div class="modal-body" style="width: 100%; font-size: 14px;">

                                        <asp:GridView ID="grdRanking" runat="server" AutoGenerateColumns="true" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="100" AllowPaging="true" OnRowCommand="grdRanking_RowCommand"
                                            CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdRanking_Sorting" OnPageIndexChanging="grdRanking_PageIndexChanging" OnRowDataBound="grdRanking_RowDataBound">
                                            <Columns>


                                                <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                    CommandName="details" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                    HeaderText="รายละเอียด"
                                                    ImageUrl="~/img/search.png" />
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
            </div>
        </div>

    </div>

    <div id="dialog-oil-dt" title="รายละเอียด" style="display: none; font-size: 14px;" class="save-dialog-message">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right;">
                    <asp:LinkButton ID="btnExportSubDT" runat="server" Visible="false" OnClick="btnExportSubDT_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SearchDT">
                        <asp:Image ID="Image10" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                    </asp:LinkButton>

                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%; font-size: 14px;">
                        <asp:GridView ID="grdSubDT" runat="server" AutoGenerateColumns="true" PagerStyle-CssClass="pager" AllowSorting="true" OnSorting="grdSubDT_Sorting"
                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" DataKeyNames="ลำดับ" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="grdSubDT_RowDataBound">
                            <Columns></Columns>
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <asp:HiddenField ID="hfCurrentPage" runat="server" />
    <div id="dialog-message-save" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            บันทึกข้อมูลเรียบร้อยแล้ว!
        </p>

    </div>
    <div id="dialog-message-remove" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            ลบข้อมูลเรียบร้อยแล้ว!
        </p>

    </div>

    <div id="dialog-message-verify-doc" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p style="color: red;">
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            ไม่สามารถออกรายงานได้ เนื่องจากสถานะเอกสารยังไม่ได้รับการตรวจสอบ!
        </p>

    </div>

    <div id="dialog-message-excel" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            ออกรายงานเรียบร้อยแล้ว!
        </p>

    </div>

    <div id="dialog-confirm-send" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            ต้องการส่งข้อมูลใช่หรือไม่?
        </p>

    </div>

    <div id="dialog-confirm-save" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>

            ต้องการบันทึกข้อมูลใช่หรือไม่?
        </p>
        <br />
        <p style="color: red; font-weight: bold;">***ตรวจสอบสถานะเอกสารก่อนบันทึกทุกครั้ง***</p>

    </div>

    <div id="dialog-confirm-remove" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>

            ต้องการลบข้อมูลใช่หรือไม่?
        </p>
    </div>

    <div id="dialog-validate-save" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            กรุณากรอกข้อมูลให้เรียบร้อยก่อนบันทึกข้อมูล
        </p>
    </div>

    <div id="dialog-message-upload" title="ข้อความ" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>

            <asp:Label ID="lblUploadResult" runat="server" Text=""></asp:Label>

        </p>
    </div>

    <script type="text/javascript">

        $("#<%=txtTransferDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTransferDate_R.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTransferDateF_DT.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtTransferDateT_DT.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateFrom.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDateTo.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

    </script>

</asp:Content>
