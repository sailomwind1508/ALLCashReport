<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransportationPlan.aspx.cs" Inherits="TransportationPlan" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
        }

        function openPopup() {

            $('#dialog-edit').dialog({
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
                    $('#wait_dialog').hide();
                    $('#wait_dialog').dialog("close");
                    $('#wait_dialog').dialog("destroy");
                    //location.reload();
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
            position: absolute;
            top: 80px;
            left: 0px;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 130px;
            left: 13%;
            width: 70%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
            .header {
                position: absolute;
                top: 80px;
                left: 6%;
                width: 100%;
                height: 50px;
                font-size: x-large;
                font-weight: 700;
                text-align: center;
            }

            .line4_footer {
                position: absolute;
                top: 130px;
                left: 18%;
                width: 70%;
            }
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

        .centerRow1 {
            height: 18px;
            white-space: nowrap !important;
            text-align: center;
            vertical-align: top;
            color:blue;
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
            font-size: 12px;
            border-color: cornflowerblue;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 12px;
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

    <div class="header bg-gradient-primary">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/delivery-truck.png" Width="50px" />
            <asp:Label ID="Label4" runat="server" Text="Transportation Planning" CssClass="text-white"></asp:Label>
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
                    <li><a href="#tabs-1">Adjust ข้อมูลการขนส่ง</a></li>
                    <li><a href="#tabs-2">รายงาน</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%;">
                        <table style="width: 100%;" id="tblLayout" runat="server">
                            <tr style="white-space: nowrap;">
                                <td>
                                    <table>
                                        <tr>
                                            <td>เลือกจำนวน Route : </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRouteAmt" runat="server" Height="30px" Width="230px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlRouteAmt_SelectedIndexChanged"></asp:DropDownList>

                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td>
                                    <div style="width: 100%; font-size: 14px;">

                                        <asp:GridView ID="grdR1" runat="server" AutoGenerateColumns="false" CssClass="mydatagrid" PagerStyle-CssClass="pager"
                                            HeaderStyle-CssClass="header_grid" RowStyle-CssClass="rows" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" PageSize="15" AllowPaging="true"
                                            CellPadding="4" DataKeyNames="ลำดับ" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true" AllowSorting="true">
                                            <Columns>
                                                <asp:BoundField DataField="TransDate" HeaderText="วันที่ขึ้นของ"  ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:TemplateField HeaderText="TARGET QTY" SortExpression="TARGET_QTY" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTARGET_QTY" runat="server" Text='<%# Bind("TARGET_QTY") %>' MaxLength="8" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Customer" HeaderText="ร้านค้า" SortExpression="Customer" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="VisitDate" HeaderText="วันที่เยี่ยม" SortExpression="VisitDate" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="centerRow" />
                                                <asp:BoundField DataField="Target" HeaderText="เป้า" SortExpression="Target" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                                <asp:BoundField DataField="Inv" HeaderText="Invoice" SortExpression="Inv" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                            </Columns>
                                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="White" CssClass="header-center" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>


                                    </div>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
            <div id="tabs-2">
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr style="white-space: nowrap;">
                            <td style="text-align: right">
                                <asp:Label ID="Label15" runat="server" Text="วันที่ขึ้นของ : "></asp:Label>&nbsp;</td>
                            <td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTranDate" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtTranDate" runat="server" ErrorMessage="*" ControlToValidate="txtTranDate" ValidationGroup="SaveTranp" ForeColor="Red"></asp:RequiredFieldValidator>

                            </td>

                            <td style="text-align: left">
                                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="SearchDT">
                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>


                        <tr style="white-space: nowrap;">
                            <td colspan="3">
                                <div style="width: 100%; font-size: 14px;">
                                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
                                </div>
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

    <div id="dialog-edit" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

        </p>
    </div>

    <script type="text/javascript">

        $("#<%=txtTranDate.ClientID %>").datepicker({
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


        });

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
