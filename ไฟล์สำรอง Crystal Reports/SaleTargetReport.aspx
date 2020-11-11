<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SaleTargetReport.aspx.cs" Inherits="SaleTargetReport" %>

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
            $('[id*=grdSaleTGReport]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": false,
                "sPaginationType": "full_numbers",
                "pageLength": 100

            });

            //$('[id*=grdEditSTG]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            //    "responsive": true,
            //    "sPaginationType": "full_numbers"
            //});

            var pageMode = $('[id*=hfCurrentPage]').val();
            if (pageMode != null && pageMode != undefined && pageMode != "") {
                if (pageMode == "TB1") {
                    $('#tabs').tabs({ active: 0 });
                }
                else if (pageMode == "TB2") {
                    $('#tabs').tabs({ active: 1 });
                }
            }

        });

        function closeDialog() {
            try {
                setTimeout(function () {
                    location.reload();
                    //var divDialog = $('[id*=wait_dialog]');
                    //divDialog.css('display', 'none');
                    //divDialog.css("visibility", "hidden");
                    ////$(".ui-dialog:visible").find(".dialog").dialog("close");
                    //divDialog.hide();
                    //$('.ui-dialog-content').css('display', 'none');
                    ////$("#wait_dialog").dialog('close');
                    ////$(this).dialog("close");
                    ////$(this).closest('.ui-dialog-content').dialog('close'); 
                    ////$('[id*=wait_dialog]').dialog('close');
                }, 10000);
            } catch (e) {
                alert(e);
            }

        }

        function ActiveTab(obj) {

            if (obj == 1) {
                $('[id*=hfCurrentPage]').val("TB2");
            }
            else if (obj == 2) {
                $('[id*=hfCurrentPage]').val("TB1");
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
                        window.location.href = "SaleTargetReport.aspx";
                    }
                }
            });


        }

        function ImportSalesTarget() {

            var path = $("#txtImportFile").val();
            alert(path);
            __doPostBack("import_target", path);
        }

        function SaveTG() {

            $('#dialog-confirm-save').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    "Save": function () {
                        $('[id*=hfCurrentPage]').val("TB2");

                        __doPostBack("save_target", "");

                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

    </script>

    <style type="text/css">
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

        .line4_footer {
            position: absolute;
            top: 150px;
            left: 13%;
            width: 70%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
        .header {
            position: absolute;
            top: 100px;
            left: 6%;
            width: 100%;
            height: 50px;
            font-size: x-large;
            font-weight: 700;
            text-align: center;
        }

        .line4_footer {
            position: absolute;
            top: 150px;
            left: 18%;
            width: 70%;
        }
        }
    </style>
    <style type="text/css">
        .header-center {
            text-align: center;
            white-space: pre-wrap !important;
            width: 80px;
            font-size:12px;
        }

        .imgWidth {
            Width: 26px;
            display: block;
            margin-left: auto;
            margin-right: auto;
        }

        .rowStyle {
            white-space: nowrap;
            width: 80px;
            font-size:14px;
            font-family: Tahoma;
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
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Sales Target Management"></asp:Label>
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
                    <li><a href="#tabs-1">Report</a></li>
                    <li><a href="#tabs-2">Update Target</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%;">
                        <div>

                            <asp:Label ID="Label13" runat="server" Text="รูปแบบรายงาน : "></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlReportType" runat="server" Height="25px" Width="301px" class="dropdown-divider" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"></asp:DropDownList>

                            <asp:Label ID="Label3" runat="server" Text="เดือน : "></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlSTGMonth" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlSTGMonth_SelectedIndexChanged"></asp:DropDownList>

                            <asp:Label ID="Label4" runat="server" Text="ปี : "></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlSTGYear" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlSTGYear_SelectedIndexChanged"></asp:DropDownList>

                            <asp:CheckBox ID="chkExportAllSale" runat="server" Text="สร้างรายงานของ sale ทุกคน" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnCheckedChanged="chkExportAllSale_CheckedChanged" />

                            <asp:UpdatePanel ID="up1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="Label1" runat="server" Text="พนักงาน : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlEmployee" runat="server" Height="25px" Width="228px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged"></asp:DropDownList>

                                    <asp:Label ID="Label6" runat="server" Text="ร้านค้า : "></asp:Label>
                                    <asp:DropDownList ID="ddlCustomer" runat="server" Height="25px" Width="290px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                    <asp:Label ID="Label5" runat="server" Text="วันเยี่ยม : "></asp:Label>
                                    <asp:DropDownList ID="ddlVisitDate" runat="server" Height="25px" Width="80px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                    <asp:Label ID="Label7" runat="server" Text="สินค้า : "></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlProduct" runat="server" Height="25px" Width="301px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div>
                            <div style="float: left; width: 150px; padding: 5px;">
                                <asp:LinkButton ID="btnReport" runat="server" OnClick="btnReport_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                </asp:LinkButton>
                            </div>
                            <div id="divExportReport" runat="server" visible="true" style="float: right; width: 150px; padding: 5px;">

                                <asp:LinkButton ID="linkExportReport" runat="server" OnClick="linkExportReport_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                    <asp:Image ID="ImagelinkExportReport" runat="server" ImageUrl="~/img/report.png" Width="23px" />ออกรายงาน
                                </asp:LinkButton>
                            </div>
                            <br />
                            <br />
                            <br />
                        </div>
                    </div>

                    <div style="width: 100%; font-size: 14px;">
                        <asp:GridView ID="grdSaleTGReport" runat="server" AutoGenerateColumns="true"
                            Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                            DataKeyNames="SKU_ID" EmptyDataText="No records Found" ShowHeaderWhenEmpty="true">
                            <Columns>
                            </Columns>
                            <PagerStyle CssClass="pagination-ys" />
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" CssClass="header-center" Width="80px" />
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
                <div id="tabs-2">
                    <div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="พนักงาน : "></asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlED_Emp" runat="server" Height="25px" Width="230px" class="dropdown-divider" OnSelectedIndexChanged="ddlED_Emp_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="เดือน : "></asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlED_Month" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="ปี : "></asp:Label>&nbsp;
                                        <asp:DropDownList ID="ddlED_Year" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAllSalseReport" runat="server" Text="สร้างรายงานของ sale ทุกคน" ViewStateMode="Enabled" EnableViewState="true" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnED_Search" runat="server" OnClick="btnED_Search_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />ค้นหา
                                        </asp:LinkButton>
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div style="float: left; width: 150px; padding: 5px;">
                            <button style="margin-bottom: 10px;" type="button" class="btn btn-primary btn-user btn-block excelButtom" data-toggle="modal" data-target="#myModal">
                                <asp:Image ID="Image6" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />Import Excel
                            </button>

                        </div>
                        <div style="float: right; width: 450px; padding: 10px;">
                            <table>
                                <tr style="white-space: nowrap;">
                                    <td>
                                        <asp:LinkButton ID="lnkExportEditTarget" runat="server" Visible="false" OnClick="lnkExportEditTarget_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/report.png" Width="23px" />ออกรายงาน
                                        </asp:LinkButton>

                                    </td>
                                    <td>
                                        <asp:LinkButton ID="linkSumReportTarget" runat="server" Visible="false" OnClick="linkSumReportTarget_Click" OnClientClick="return closeDialog();" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/img/report.png" Width="23px" />รายงานแบบสรุป
                                        </asp:LinkButton>

                                    </td>
                                    <td>
                                        <a id="btnSaveTG" runat="server" href="#" title="" visible="false" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                            class="btn btn-primary btn-user btn-block" onclick="return SaveTG();">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />บันทึก
                                        </a>

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div>
                        <asp:GridView ID="grdEditSTG" runat="server" AutoGenerateColumns="false"
                            BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                            CellPadding="4" DataKeyNames="ROW_NO" EmptyDataText="No records Found" Width="100%" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="No." SortExpression="ROW_NO" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblROW_NO" runat="server" Text='<%# Eval("ROW_NO").ToString() == "0" ? "" : Eval("ROW_NO").ToString() %>'></asp:Label>
                                        <asp:HiddenField ID="hfSKU_ID" runat="server" Value='<%# Bind("SKU_ID") %>' />
                                        <asp:HiddenField ID="hfSALE_ID" runat="server" Value='<%# Bind("SALE_ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SKU NAME" SortExpression="SKU_NAME" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKU_NAME" runat="server" Text='<%# Bind("SKU_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TARGET QTY" SortExpression="TARGET_QTY" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTARGET_QTY" runat="server" Text='<%# Bind("TARGET_QTY") %>' MaxLength="8" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TARGET PRICE" SortExpression="TARGET_PRICE" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPRICE" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("TARGET_PRICE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BALANCE QTY" SortExpression="BALANCE_QTY" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBALANCE_QTY" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("BALANCE_QTY")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BALANCE PRICE" SortExpression="BALANCE_PRICE" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBALANCE_PRICE" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("BALANCE_PRICE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TOTAL QTY" SortExpression="TOTAL_QTY" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTOTAL_QTY" runat="server" Text='<%# String.Format("{0:#,##0}", Eval("TOTAL_QTY")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TOTAL PRICE" SortExpression="TOTAL_PRICE" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                                    <ItemStyle Wrap="false" HorizontalAlign="Center" CssClass="rowStyle"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTOTAL_PRICE" runat="server" Text='<%# String.Format("{0:#,##0.00}", Eval("TOTAL_PRICE")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" CssClass="header-center" />
                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                            <SortedAscendingCellStyle BackColor="#EDF6F6" />
                            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                            <SortedDescendingCellStyle BackColor="#D6DFDF" />
                            <SortedDescendingHeaderStyle BackColor="#002876" />
                        </asp:GridView>
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
    <%--    <div id="dialog-import-file" title="Message" style="display: none" class="save-dialog-message">
        <table>
            <tr>
                <td>
                    <label for="avatar">กรุณาเลือก excel sales target:</label>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
     
                      <asp:LinkButton ID="btnImport" runat="server" OnClick="btnUpload_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/file.png" Width="23px" />Upload
                    </asp:LinkButton>

           
                     <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

    </div>--%>

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
                                                <label>Choose excel target</label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label11" runat="server" Text="เดือน : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlExcelMonth" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>

                                                <asp:Label ID="Label12" runat="server" Text="ปี : "></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlExcelYear" runat="server" Height="25px" Width="150px" class="dropdown-divider" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">เลือกข้อมูลที่ต้องการนำเข้า : </td>
                                        </tr>
                                        <tr style="white-space: nowrap">
                                            <td>

                                                <asp:CheckBox ID="chkcustomers_master" runat="server" Text="sheet customers_master" Checked="true" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chksales_visit_date" runat="server" Text="sheet sales_visit_date" Enabled="false" />
                                            </td>

                                        </tr>
                                        <tr style="white-space: nowrap">

                                            <td>
                                                <asp:CheckBox ID="chkproducts_master" runat="server" Text="sheet products_master" Checked="true" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chksales_target" runat="server" Text="sheet sales_target" Enabled="false" />
                                            </td>
                                        </tr>

                                        <tr style="white-space: nowrap">
                                            <td>
                                                <asp:CheckBox ID="chksales_master" runat="server" Text="sheet sales_master" Checked="true" />

                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkusers_master" runat="server" Text="sheet users_master" Checked="true" />
                                            </td>
                                        </tr>
                                        <tr style="white-space: nowrap">
                                            <td>
                                                <asp:CheckBox ID="chkNextSaletarget" runat="server" Text="เตรียม target ให้ sale" Checked="true" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkimport_sale_target" runat="server" Text="sheet import_sale_target" />
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

       <%-- $("#<%=txtVisitDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        }); --%>

        $(document).ready(function () {

            prepareCustomerCheckbox();
            prepareProductCheckbox();

            $("#<%=chkcustomers_master.ClientID %>").click(function () {
                prepareCustomerCheckbox();
            });

            $("#<%=chkproducts_master.ClientID %>").click(function () {
                prepareProductCheckbox();

            });

           <%-- $('[id*=chkExportAllSale]').click(function () {
                var _chkExportAllSale = $("#<%=chkExportAllSale.ClientID %>");

                if (_chkExportAllSale.checked) {
                    $('[id*=linkExportReport]').hide();
                }
                else {
                    $('[id*=linkExportReport]').show();
                }
            });--%>
        });

        function prepareCustomerCheckbox() {
            var chkcustomers_master = $("#<%=chkcustomers_master.ClientID %>");
            $("#<%=chksales_visit_date.ClientID %>").prop("checked", chkcustomers_master.prop("checked"));

        }
        function prepareProductCheckbox() {
            var chkproducts_master = $("#<%=chkproducts_master.ClientID %>");
            $("#<%=chksales_target.ClientID %>").prop("checked", chkproducts_master.prop("checked"));
        }

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
    </script>
</asp:Content>

