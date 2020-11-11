<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BranchStockReport.aspx.cs" Inherits="BranchStockReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
        $(function () {
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
            var txtDocDate = $("#<%=txtDocDate.ClientID %>").val();
            var txtPONo = $("#<%=txtPONo.ClientID %>").val();
            var upWH = $("#<%=FileUpload1.ClientID %>").val();
            var upPO = $("#<%=FileUpload2.ClientID %>").val();

            if (txtDocDate == undefined || txtDocDate == "" || txtDocDate == null) {
                validate_save = false;
                alert("กรุณากรอก วันที่!!!");
                $("#<%=txtDocDate.ClientID %>").focus();
            }
            else if (txtPONo == undefined || txtPONo == "" || txtPONo == null) {
                validate_save = false;
                alert("กรุณากรอก เลขที่ PO!!!");
                $("#<%=txtPONo.ClientID %>").focus();
            }
            else if ((upWH == undefined || upWH == "" || upWH == null) && (upPO == undefined || upPO == "" || upPO == null)) {
                validate_save = false;
                alert("กรุณาเลือก excel!!!");
            }

            if (txtPONo != undefined || txtPONo != "" || txtPONo != null) {
                if (txtPONo.length != 7) {
                    validate_save = false;
                    alert("เลขที่ PO ไม่ถูกต้อง !!!");
                    $("#<%=txtPONo.ClientID %>").focus();
                }
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

        function removePO(pk) {
            $('#dialog-confirm-remove').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    ยืนยัน: function () {
                        __doPostBack("removePO", pk);
                        $(this).dialog("close");
                    },
                    ยกเลิก: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function savePO() {
            var ddlPO = $("#<%=ddlPODocStatus.ClientID%>");
            var selPOStatus = ddlPO.val();
            var chk = $("#<%=chkAdminApprove.ClientID%>");
            var txtRefSAP = $("#<%=txtRefSAPPO.ClientID%>");
            var refSAPPO = "";
            refSAPPO = txtRefSAP.val();

            if (selPOStatus == "-1") {
                alert("กรุณาเลือก สถานะเอกสาร!!!");
                return;
            }

            if (chk.is(":checked")) {
                selPOStatus = "5";
            }

            var grdData = getGridViewData(document.getElementById("<%=grdEditPO.ClientID%>"));

            if (validate_save == true) {
                $('#dialog-confirm-save').dialog({
                    resizable: false,
                    height: 300,
                    width: 400,
                    modal: true,
                    buttons: {
                        บันทึก: function () {
                            __doPostBack("savePO", grdData + "!" + selPOStatus + "!" + refSAPPO);
                            $(this).dialog("close");
                        },
                        ยกเลิก: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }

        function saveWH() {

            var ddlwh = $("#<%=ddlWHDocStatus.ClientID%>");
            var selWHStatus = ddlwh.val();

            if (selWHStatus == "-1") {
                alert("กรุณาเลือก สถานะเอกสาร!!!");
                return;
            }

            var grdData = getGridViewData(document.getElementById("<%=grdEditWH.ClientID%>"));

            $('#dialog-confirm-save').dialog({
                resizable: false,
                height: 300,
                width: 400,
                modal: true,
                buttons: {
                    บันทึก: function () {
                        __doPostBack("saveWH", grdData + "!" + selWHStatus);
                        $(this).dialog("close");
                    },
                    ยกเลิก: function () {
                        $(this).dialog("close");
                    }
                }
            });
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

        function RemoveResult(obj) {

            $('#dialog-message-remove').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            ActiveTab(obj)
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

        function ShowPrint(obj) {
            var dlg = $("#dialog-report-po"); // Get the dialog container.

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

            ActiveTab(obj)
        }

        function ShowPO(obj) {

            var dlg = $("#dialog-edit-po"); // Get the dialog container.

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

            ActiveTab(obj)
        }

        function ShowSavePO(obj) {

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


        function ShowWH(obj) {

            var dlg = $("#dialog-edit-wh"); // Get the dialog container.

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

    <%--    <div class="header">
        <asp:Label ID="lblPageName" runat="server" Text="Branch Stock Report"></asp:Label>
    </div>--%>

    <div class="card" style="width: 100%;">
        <div class="card-header header bg-gradient-primary">
            <asp:Image ID="Image20" runat="server" ImageUrl="~/img/warehouse.png" Width="50px" />
            <asp:Label ID="Label11" runat="server" Text="Branch Stock Report" CssClass="text-white"></asp:Label>
        </div>

    </div>

    <div style="width: 100%; position: absolute;">
        <div>
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
            <div id="tabs" style="width: 80%; font-size: 14px;">
                <ul>
                    <li><a href="#tabs-1">
                        <asp:Image ID="Image16" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" />&nbsp;ส่งข้อมูล Excel</a></li>
                    <li><a href="#tabs-2">
                        <asp:Image ID="Image19" runat="server" ImageUrl="~/img/paper-plane.png" Width="23px" Height="25px" />&nbsp;ตรวจสอบการส่งข้อมูล</a></li>
                    <li><a href="#tabs-3">
                        <asp:Image ID="Image17" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Suggest Order Allocate Report</a></li>
                    <li><a href="#tabs-4">
                        <asp:Image ID="Image18" runat="server" ImageUrl="~/img/report.png" Width="23px" Height="25px" />&nbsp;Branch Stock Report</a></li>
                </ul>

                <div id="tabs-1">
                    <div class="">

                        <div class="">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">ส่งข้อมูล Excel</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <table>
                                                    <tr style="white-space: nowrap;">
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label2" runat="server" Text="สาขา : "></asp:Label>&nbsp;</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSalesArea" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label1" runat="server" Text="วันที่ : "></asp:Label>&nbsp;</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDocDate" runat="server" Width="120px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtDocDate" runat="server" ErrorMessage="*" ControlToValidate="txtDocDate" ValidationGroup="SendData" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="เลขที่ PO : "></asp:Label>&nbsp;
                                                        </td>
                                                        <td style="white-space: nowrap;">
                                                            <asp:TextBox ID="txtPONo" runat="server" Width="80px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="7"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtPONo" runat="server" ErrorMessage="*" ControlToValidate="txtPONo" ValidationGroup="SendData" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </td>

                                                    </tr>

                                                    <tr>
                                                        <td colspan="5" style="text-align: left;">
                                                            <p style="color: red; font-weight: bold;">***ฟอร์แมต PO คือ YYMMXXX ตัวอย่าง YY คือ ปี เช่น 20, MM คือ เดือน เช่น 07, XXX คือ ตัวเลข เช่น 001***</p>
                                                        </td>
                                                    </tr>

                                                </table>

                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" /><label>เลือกไฟล์ Excel รายงานสินค้าคงเหลือรวม</label>
                                                <div class="input-group">
                                                    <div class="custom-file">
                                                        <asp:FileUpload ID="FileUpload1" runat="server" Height="50px" />

                                                    </div>
                                                </div>

                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" /><label>เลือกไฟล์ Excel รายงานสั่งสินค้า (PO)</label>
                                                <div class="input-group">
                                                    <div class="custom-file">
                                                        <asp:FileUpload ID="FileUpload2" runat="server" Height="50px" />

                                                    </div>
                                                </div>

                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <a id="btnSave" onclick="return alert_confirm_save();" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary">
                                        <asp:Image ID="imgSave" runat="server" ImageUrl="~/img/paper-plane.png" Width="23px" Height="25px" />&nbsp;ส่งข้อมูล
                                    </a>
                                    <%--<asp:Button ID="btnUpload" runat="server" CssClass="btn btn-outline-primary" Text="ส่งข้อมูล" OnClick="btnUpload_Click" ValidationGroup="SendData" />--%>
                                </div>

                            </div>
                        </div>

                    </div>
                </div>
                <div id="tabs-2">
                    <div style="width: 100%;" runat="server" id="tb2">
                        <div class="modal-content">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 100%">
                                        <div class="modal-header">
                                            <table>
                                                <tr style="white-space: nowrap;">
                                                    <td style="text-align: left">
                                                        <asp:Label ID="Label9" runat="server" Text="สถานะการส่ง : "></asp:Label>&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlDocStatus" runat="server" Height="25px" Width="100px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Label10" runat="server" Text="สาขา : "></asp:Label>&nbsp;
                                                    
                                                        <asp:DropDownList ID="ddlSalesAreaS" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Label4" runat="server" Text="วันที่ : "></asp:Label>&nbsp;
                                                    
                                                        <asp:TextBox ID="txtStatusDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtStatusDate" runat="server" ErrorMessage="*" ControlToValidate="txtStatusDate" ValidationGroup="SStatus" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        &nbsp;&nbsp;
                                                        <asp:Label ID="Label5" runat="server" Text="เลขที่ PO : "></asp:Label>&nbsp;
                                                    
                                                        <asp:TextBox ID="txtPOS" runat="server" Width="80px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="7"></asp:TextBox>

                                                        &nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnSearchStatus" runat="server" OnClick="btnSearchStatus_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SStatus">
                                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <div class="modal-body" style="width: 100%; font-size: 14px;">
                                            <asp:GridView ID="grdStatus" runat="server" AutoGenerateColumns="false" PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="50" AllowPaging="true" OnRowCommand="grdStatus_RowCommand"
                                                CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdSOA_Sorting" OnPageIndexChanging="grdSOA_PageIndexChanging" OnRowDataBound="grdStatus_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="No" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="BranchID" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="BranchName" HeaderText="ชื่อสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="DocDate" HeaderText="วันที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="PONo" HeaderText="เลขที่ PO" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="SendStatus" HeaderText="สถานะการส่งข้อมูล" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="DocStatus" HeaderText="สถานะเอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="TotalQty" HeaderText="จำนวนที่ศูนย์สั่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="TotalChestQty" HeaderText="Stock คงเหลือ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="CubicCar" HeaderText="จำนวนรถ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                    <asp:BoundField DataField="Cubic" HeaderText="Cubic ทั้งหมด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                    <asp:BoundField DataField="RefSAPPO" HeaderText="รหัส SAP" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="UpdateBy" HeaderText="แก้ไขโดย" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="UpdateDate" HeaderText="วันที่แก้ไขล่าสุด" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />

                                                    <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                        CommandName="poEdit" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                        HeaderText="แก้ไข PO"
                                                        ImageUrl="~/img/document.png" />
                                                    <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                        CommandName="whEdit" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                        HeaderText="สินค้าคงเหลือ"
                                                        ImageUrl="~/img/search.png" />
                                                    <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                                        CommandName="report" DataTextField="PK" ControlStyle-CssClass="imgWidthEdit"
                                                        HeaderText="รายงาน"
                                                        ImageUrl="~/img/report.png" />

                                                    <asp:TemplateField HeaderText="ลบ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <a id="btnRemove" onclick="return removePO(<%# Eval("PK").ToString()%>);" href="#" title="" style="width: 40px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-user btn-block">
                                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/bucket.png" Width="23px" Height="20px" />
                                                            </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                    <td colspan="5">
                                        <div class="modal-header">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 100px; text-align: left">สาขา : 
                                                    </td>
                                                    <td style="width: 180px; text-align: left">
                                                        <asp:DropDownList ID="ddlSalesAreaDT" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    </td>
                                                    <td style="width: 100px; text-align: left">Stock Cover Days (End Month) :  </td>
                                                    <td colspan="3" style="width: 180px; text-align: left">
                                                        <asp:DropDownList ID="ddlStockCD" runat="server" Height="25px" Width="100px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                                    </td>
                                                    <td colspan="2"></td>

                                                </tr>
                                                <tr>

                                                    <td style="width: 100px; text-align: left">วันที่เอกสาร : </td>
                                                    <td style="width: 180px; text-align: left">
                                                        <asp:TextBox ID="txtDocDateS" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtDocDateS" runat="server" ErrorMessage="*" ControlToValidate="txtDocDateS" ValidationGroup="SOA" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </td>
                                                    <td style="width: 100px; text-align: left">
                                                        <asp:Label ID="Label6" runat="server" Text="เลขที่ PO : "></asp:Label>&nbsp;
                                                    </td>
                                                    <td style="width: 180px; text-align: left">
                                                        <asp:TextBox ID="txtPONoR" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>

                                                    </td>
                                                    <td style="width: 180px; text-align: left; white-space: nowrap">
                                                        <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SOA">
                                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td style="width: 180px; text-align: left; white-space: nowrap">

                                                        <asp:LinkButton ID="btnExportExcel" runat="server" Visible="false" OnClick="btnExportExcel_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="SOA">
                                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                                        </asp:LinkButton>

                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div class="modal-body" style="width: 100%; font-size: 14px;">

                                            <asp:GridView ID="grdSOA" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="200" AllowPaging="true"
                                                CellPadding="4" DataKeyNames="No" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="grdSOA_Sorting" OnPageIndexChanging="grdSOA_PageIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="No" HeaderText="ลำดับ" SortExpression="No" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="Location Number" HeaderText="Location Number" SortExpression="Location Number" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="Location Name" HeaderText="Location Name" SortExpression="Location Name" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="เลขที่ PO" HeaderText="เลขที่ PO" SortExpression="เลขที่ PO" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="วันที่เอกสาร" HeaderText="วันที่เอกสาร" SortExpression="วันที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="Item Number" HeaderText="Item Number" SortExpression="Item Number" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="Item Description" HeaderText="Item Description" SortExpression="SalesArea" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="จำนวนที่ศูนย์สั่ง" HeaderText="จำนวนที่ศูนย์สั่ง" SortExpression="จำนวนที่ศูนย์สั่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="สินค้าคงเหลือในคลัง" HeaderText="สินค้าคงเหลือในคลัง" SortExpression="สินค้าคงเหลือในคลัง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="Est. Order (MU)" HeaderText="Est. Order (MU)" SortExpression="Est. Order (MU)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="Total Stock" HeaderText="Total Stock" SortExpression="Total Stock" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                                    <asp:BoundField DataField="เฉลี่ย/วัน" HeaderText="เฉลี่ย/วัน" SortExpression="เฉลี่ย/วัน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />
                                                    <asp:BoundField DataField="Stock Cover Days (End Month)" HeaderText="Stock Cover Days (End Month)" SortExpression="Stock Cover Days (End Month)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
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
                                    <td>
                                        <div class="modal-header">
                                            <table style="width: 70%;">
                                                <tr>
                                                    <td style="width: 230px; text-align: left">สาขา :&nbsp;&nbsp;<asp:DropDownList ID="ddlSalesArea_BSR" runat="server" Height="25px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesArea_BSR_SelectedIndexChanged"></asp:DropDownList>
                                                        &nbsp;&nbsp&nbsp;&nbsp
                                                <asp:LinkButton ID="btnSearchBSR" runat="server" OnClick="btnSearchBSR_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="BSR">
                                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/img/search.png" Width="23px" />&nbsp;ค้นหา
                                                </asp:LinkButton>

                                                        <asp:LinkButton ID="btnExportExcelBSR" runat="server" Visible="false" OnClick="btnExportExcelBSR_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary" ValidationGroup="BSR">
                                                            <asp:Image ID="Image15" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                                        </asp:LinkButton>

                                                    </td>

                                                </tr>

                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="modal-body" style="width: 100%; font-size: 14px;">

                                            <asp:GridView ID="grdBSR" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                                HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="200" AllowPaging="true"
                                                CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdBSR_PageIndexChanging" OnRowDataBound="grdBSR_RowDataBound">
                                                <Columns>

                                                    <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                    <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="ชื่อสาขา" HeaderText="ชื่อสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="วันที่เอกสาร" HeaderText="วันที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="รหัสสินค้า" HeaderText="รหัสสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="ชื่อสินค้า" HeaderText="ชื่อสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="หน่วยใหญ่" HeaderText="หน่วยใหญ่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="หน่วย(ใหญ่)" HeaderText="หน่วย(ใหญ่)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="หน่วยย่อย" HeaderText="หน่วยย่อย" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="หน่วย(ย่อย)" HeaderText="หน่วย(ย่อย)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                    <asp:BoundField DataField="มูลค่าคงเหลือ" HeaderText="มูลค่าคงเหลือ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />

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
    </div>


    <div id="dialog-edit-po" title="แก้ไข PO" style="display: none; font-size: 14px;" class="save-dialog-message">
        <table style="width: 100%;">
            <tr style="white-space: nowrap;">

                <td style="width: 180px; text-align: left; white-space: nowrap;">
                    <table>
                        <tr style="white-space: nowrap;">
                            <td>
                                <asp:Label ID="lblCurDocStatus" runat="server" Text="สถานะเอกสารปัจจุบัน : "></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurDocStatus" runat="server" Width="150px" BackColor="Green" Font-Bold="true" ReadOnly="true" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td style="width: 100px; text-align: left">
                                <asp:Label ID="Label7" runat="server" Text="สถานะเอกสาร : "></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPODocStatus" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:CheckBox ID="chkAdminApprove" runat="server" Text="อนุมัติ" Visible="false" />

                            </td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblRefSAPPO" runat="server" Visible="false" Text="RefSAP : "></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefSAPPO" runat="server" Width="300px" Visible="false" ViewStateMode="Enabled" EnableViewState="true" MaxLength="200"></asp:TextBox>
                            </td>

                        </tr>

                    </table>

                </td>
                <td style="width: 10px; text-align: left"></td>
                <td style="white-space: nowrap;">
                    <div class="pull-right">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnRefreshPO" runat="server" OnClick="btnRefreshPO_Click" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/img/refresh.png" Width="23px" />&nbsp;Refresh
                                    </asp:LinkButton>

                                </td>
                                <td>
                                    <a id="btnSavePO" runat="server" href="#" title="" style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                        class="btn btn-primary btn-user btn-block" onclick="return savePO();">
                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />บันทึก
                                    </a>

                                </td>

                                <td>
                                    <asp:LinkButton ID="ExportPO" runat="server" OnClick="ExportPO_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image9" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                    </asp:LinkButton>

                                </td>
                                <td>
                                    <asp:LinkButton ID="ExportToSAP" runat="server" Visible="false" OnClick="ExportToSAP_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image10" runat="server" ImageUrl="~/img/icons8-sap-480.png" Width="25px" />&nbsp;ส่ง SAP
                                    </asp:LinkButton>

                                </td>
                                <td>
                                    <asp:LinkButton ID="EXCrytalReport" runat="server" Visible="false" OnClick="EXCrytalReport_Click" OnClientClick="return closeDialog();" Style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image14" runat="server" ImageUrl="~/img/printer.png" Width="25px" />&nbsp;พิมพ์
                                    </asp:LinkButton>
                                    <%--<a id="EXCrytalReport" runat="server" href="#" title="" style="width: 100px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                        class="btn btn-primary btn-user btn-block" onclick="return ShowPrint(2);">
                                        <asp:Image ID="Image14" runat="server" ImageUrl="~/img/printer.png" Width="23px" />พิมพ์
                                    </a>--%>
                                </td>
                            </tr>
                        </table>
                    </div>

                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="width: 100%; font-size: 14px;">
                        <asp:GridView ID="grdEditPO" runat="server" AutoGenerateColumns="false" PagerStyle-CssClass="pager"
                            HeaderStyle-CssClass="header_grid" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="grdEditPO_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="ชื่อสาขา" HeaderText="ชื่อสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="วันที่เอกสาร" HeaderText="วันที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="เลข PO" HeaderText="เลข PO" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="กลุ่มสินค้า" HeaderText="กลุ่มสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="รหัสสินค้า" HeaderText="รหัสสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="รายการสินค้า" HeaderText="รายการสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="หน่วย" HeaderText="หน่วย" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                <asp:TemplateField HeaderText="จำนวนสั่ง" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOrderQty" runat="server" Text='<%# Bind("จำนวนสั่ง") %>' MaxLength="8" Width="80px"></asp:TextBox>
                                        <asp:HiddenField ID="hfPOPK" runat="server" Value='<%# Bind("PK") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Stock คงเหลือ" HeaderText="Stock คงเหลือ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />

                                <asp:BoundField DataField="Cubic" HeaderText="Cubic" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" />

                                <asp:BoundField DataField="ราคาศูนย์ขาย" HeaderText="ราคาศูนย์ขาย" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />


                                <asp:BoundField DataField="จำนวนเงิน" HeaderText="จำนวนเงิน" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="หมายเหตุ" HeaderText="หมายเหตุ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                            </Columns>
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>

    </div>


    <div id="dialog-edit-wh" title="สินค้าคงเหลือ" style="display: none; font-size: 14px;" class="save-dialog-message">
        <table style="width: 100%;">
            <tr style="white-space: nowrap;">
                <td style="width: 100px; text-align: left">
                    <asp:Label ID="Label8" runat="server" Text="สถานะเอกสาร : " Visible="false"></asp:Label>&nbsp;
                </td>
                <td style="width: 180px; text-align: left">
                    <asp:DropDownList ID="ddlWHDocStatus" runat="server" Visible="false" Height="25px" Width="180px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                </td>
                <td style="width: 10px; text-align: left"></td>
                <td>
                    <div class="pull-right">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnRefreshWH" runat="server" Visible="false" OnClick="btnRefreshWH_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image11" runat="server" ImageUrl="~/img/refresh.png" Width="23px" />&nbsp;Refresh
                                    </asp:LinkButton>

                                </td>
                                <td>
                                    <a id="btnSaveWH" runat="server" href="#" title="" visible="false" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                        class="btn btn-primary btn-user btn-block" onclick="return saveWH();">
                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />บันทึก
                                    </a>

                                </td>

                                <td>
                                    <asp:LinkButton ID="ExportWH" runat="server" OnClick="ExportWH_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                        <asp:Image ID="Image12" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />&nbsp;Export
                                    </asp:LinkButton>

                                </td>
                            </tr>
                        </table>
                    </div>

                </td>

            </tr>
            <tr>
                <td colspan="4">
                    <div style="width: 100%; font-size: 14px;">
                        <asp:GridView ID="grdEditWH" runat="server" AutoGenerateColumns="false" PagerStyle-CssClass="pager"
                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" DataKeyNames="PK" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="grdEditWH_RowDataBound">
                            <Columns>

                                <asp:BoundField DataField="ลำดับ" HeaderText="ลำดับ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                <asp:BoundField DataField="รหัสสาขา" HeaderText="รหัสสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="ชื่อสาขา" HeaderText="ชื่อสาขา" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="วันที่เอกสาร" HeaderText="วันที่เอกสาร" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="รหัสสินค้า" HeaderText="รหัสสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="ชื่อสินค้า" HeaderText="ชื่อสินค้า" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                <asp:BoundField DataField="หน่วยใหญ่" HeaderText="หน่วยใหญ่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                <%-- <asp:TemplateField HeaderText="หน่วยใหญ่" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtChestQty" runat="server" Text='<%# Bind("หน่วยใหญ่") %>' MaxLength="8" Width="80px" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfWHPK" runat="server" Value='<%# Bind("PK") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:BoundField DataField="หน่วย(ใหญ่)" HeaderText="หน่วย(ใหญ่)" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />

                                <asp:BoundField DataField="มูลค่าคงเหลือ" HeaderText="มูลค่าคงเหลือ" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />

                            </Columns>
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                            <PagerStyle CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <div id="dialog-report-po" title="พิมพ์รายงาน" style="display: none; font-size: 14px;" class="save-dialog-message">
        <table style="width: 100%;">
            <%-- <tr style="white-space: nowrap;">
                <td style="width: 100px; text-align: left">
                    <asp:LinkButton ID="btnPrintReport" runat="server" OnClick="btnPrintReport_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                        <asp:Image ID="Image13" runat="server" ImageUrl="~/img/report.png" Width="23px" />&nbsp;สร้างรายงาน
                    </asp:LinkButton>
                </td>
            </tr>--%>
            <tr style="white-space: nowrap;">
                <td style="width: 100px; text-align: left">
                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None"
                        HasRefreshButton="True" OnLoad="CrystalReportViewer1_Load" OnUnload="CrystalReportViewer1_Unload" HasToggleGroupTreeButton="False" HasPageNavigationButtons="False" Width="100%" />
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

    <div id="dialog-confirm-send" title="ตรวจสอบ PO ให้ถูกต้อง ก่อนกดส่งข้อมูลทุกครั้ง" style="display: none" class="save-dialog-message">
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

        $("#<%=txtDocDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtDocDateS.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtStatusDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

    </script>

</asp:Content>

