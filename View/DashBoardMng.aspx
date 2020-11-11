<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DashBoardMng.aspx.cs" Inherits="View_DashBoardMng" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

        <script type="text/javascript">
        $(function () {

            $("#ContentPlaceHolder1_txtPrice").keyup(function () {
                var $this = $(this);
                $this.val($this.val().replace(/[^-\d.]/g, ''));
            });

            $("[id*=btnSave]").click(function () {
                var _date = $("[id*=txtDate]").val();
                var _price = $("[id*=txtPrice]").val();

                if (_date != undefined && _date != "" && _price != undefined && _price != "") {
                    $('#dialog-c-save').dialog({
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
                else {
                    alert("กรุณากรอกข้อมูลให้ครบถ้วน!!!");
                    $("[id*=txtDate]").focus();
                }
            });

            $("[id*=btnRemove]").click(function () {
                var _date = $("[id*=txtDate]").val();
                if (_date != undefined && _date != "") {
                    $('#dialog-c-remove').dialog({
                        resizable: false,
                        height: 200,
                        width: 400,
                        modal: true,
                        buttons: {
                            "Remove": function () {
                                __doPostBack("remove", "");
                                $(this).dialog("close");
                            },
                            Cancel: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                else {
                    alert("กรุณาเลือกวันที่ ที่ต้องการลบข้อมูล!!!");
                    $("[id*=txtDate]").focus();
                }
            });
        });

        function showSaveResultMsg() {
            $('[id*=dialog-save]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function showRemoveResultMsg() {
            $('[id*=dialog-remove]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function closeDialog() {
            try {
                setTimeout(function () {
                    location.reload();
                }, 10000);
            } catch (e) {
                alert(e);
            }

        }

       

    </script>
    <style>
        .header-center {
            text-align: center;
            white-space: nowrap !important;
            width: 80px;
            font-size: 13px;
            height: 50px;
        }

        th
        {
          border: 1px solid black;
          width: 100px;
          overflow: hidden;
        }

        .rowStyle {
            white-space: nowrap;
            width: 30px;
            font-size: 13px;
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
                            <h6 class="m-0 font-weight-bold" style="color: black;">วันที่ : </h6>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" Width="100px" ViewStateMode="Enabled" EnableViewState="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="*" ControlToValidate="txtDate" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">รายการ : </h6>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrder" runat="server" Width="150px"></asp:DropDownList>
                        </td>
                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">ประเภท : </h6>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:DropDownList ID="ddlType" runat="server" Width="150px"></asp:DropDownList>
                        </td>
                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">ราคา : </h6>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrice" runat="server" Width="150px" ViewStateMode="Enabled" EnableViewState="true" MaxLength="12" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ErrorMessage="*" ControlToValidate="txtPrice" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>

                        </td>
                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">สาเหตุ : </h6>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:DropDownList ID="ddlRemark" runat="server" Width="150px"></asp:DropDownList>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <%--<asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; background-color: brown; color: white;" class="btn btn-primary btn-user btn-block" ValidationGroup="Save">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />บันทึก
                            </asp:LinkButton>--%>
                            <a id="btnSave" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;" class="btn btn-primary btn-user btn-block">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" Height="25px" />&nbsp;บันทึก
                            </a>
                        </td>
                        <td>&nbsp;&nbsp;</td>
                        <td>
                            <%-- <asp:LinkButton ID="btnRemove" runat="server" OnClick="btnRemove_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; background-color: brown; color: white;" class="btn btn-primary btn-user btn-block">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/document.png" Width="23px" />ลบข้อมูล
                            </asp:LinkButton>--%>
                            <a id="btnRemove" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; " class="btn btn-primary btn-user btn-block">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/img/document.png" Width="23px" Height="25px" />&nbsp;ลบข้อมูล
                            </a>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="card-header py-3">
                <table>

                    <tr style="white-space: nowrap;">

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
                        <td>&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; " class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                            </asp:LinkButton>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="card-body">
                <h6 class="m-0 font-weight-bold" style="color: black;">
                    <asp:Literal ID="lblTitle" runat="server"></asp:Literal></h6>
                <div class="table-responsive">
                    <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="true" class="" Width="" CellSpacing="0"
                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdData_RowDataBound">
                        <Columns>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                        <HeaderStyle BackColor="#9BC2E6" Font-Bold="True" ForeColor="Black" CssClass="header-center" Width="50px" />
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

            <div class="card-header py-3">
                <table>

                    <tr style="white-space: nowrap;">

                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">Month : </h6>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:DropDownList ID="ddlMonth2" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <h6 class="m-0 font-weight-bold" style="color: black;">Year : </h6>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:DropDownList ID="ddlYear2" runat="server"></asp:DropDownList>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="btnSearch2" runat="server" OnClick="btnSearch2_Click" Style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white; " class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/img/search.png" Width="23px" />Search
                            </asp:LinkButton>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="card-body">
                <h6 class="m-0 font-weight-bold" style="color: black;">
                    <asp:Literal ID="lblTitle2" runat="server"></asp:Literal></h6>
                <div class="table-responsive">
                    <asp:GridView ID="grdData2" runat="server" AutoGenerateColumns="true" class="" Width="" CellSpacing="0"
                        BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px"
                        EmptyDataText="No records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="grdData_RowDataBound">
                        <Columns>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#1F438F" />
                        <HeaderStyle BackColor="#FFCCFF" Font-Bold="True" ForeColor="Black" CssClass="header-center" Width="50px" />
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

    <div id="dialog-save" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Save successful!
        </p>

    </div>

    <div id="dialog-remove" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Save successful!
        </p>

    </div>

    <div id="dialog-c-save" title="Warring!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to save data?</p>
    </div>

    <div id="dialog-c-remove" title="Warring!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to remove data?</p>
    </div>

    <script type="text/javascript">
        $("#ContentPlaceHolder1_txtDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });//.datepicker('option', 'dateFormat', 'dd/mm/yy');

    </script>

    
</asp:Content>

