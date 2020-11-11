<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AccountManagement.aspx.cs" Inherits="AccountManagement" %>

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
            $('[id*=grdAccMng]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": true,
                "sPaginationType": "full_numbers"
            });

            //$('[id*=grdUser]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
            //    "responsive": true,
            //    "sPaginationType": "full_numbers"
            //});

            //$('[id*=dialog-message]').dialog({
            //    modal: true,
            //    buttons: {
            //        Ok: function () {
            //            $(this).dialog("close");
            //        }
            //    }
            //});

            var pageMode = $('[id*=hfCurrentPage]').val();
            if (pageMode != null && pageMode != undefined && pageMode != "") {
                if (pageMode == "DP" || pageMode == "SP") {
                    $('#tabs').tabs({ active: 0 });
                }
                else if (pageMode == "SU" || pageMode == "EU" || pageMode == "DU") {
                    $('#tabs').tabs({ active: 1 });
                }
                else if (pageMode == "EP" || pageMode == "VP") {
                    $('#tabs').tabs({ active: 2 });
                }
            }

        });

        function showSaveMsg(obj) {
            $('[id*=dialog-message-save]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $('#tabs').tabs({ active: obj });
        }

        function showDeleteMsg(obj) {
            $('[id*=dialog-message-del]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $('#tabs').tabs({ active: obj });
        }

        function ValidateSave(obj) {
            $('[id*=dialog-validate-save]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            if (obj == 1) {
                $('[id*=hfCurrentPage]').val("SU");
            }
            else if (obj == 2) {
                $('[id*=hfCurrentPage]').val("VP");
            }
        }

        function ValidateDelete(obj) {
            $('[id*=dialog-validate-delete]').dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });

            $('[id*=hfCurrentPage]').val("DU");
        }

        function SaveUser() {
            $('#dialog-confirm-save').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    "Save": function () {
                        $('[id*=hfCurrentPage]').val("SU");

                        __doPostBack("save_user", "");



                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function DelUser() {
            $('#dialog-confirm-delete').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    "Remove": function () {
                        $('[id*=hfCurrentPage]').val("DU");
                        __doPostBack("delete_user", "");
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function SavePer() {
            $('#dialog-confirm-save').dialog({
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    "Save": function () {
                        $('[id*=hfCurrentPage]').val("SP");
                        __doPostBack("save_per", "");
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function editItem(uniqueID, itemID) {

            var param = itemID + "|EDIT";
            $('[id*=hfCurrentPage]').val("EP");
            __doPostBack(param, "");

            //$("#dialog-confirm-edit").dialog({
            //    title: 'Warning',

            //    resizable: false,
            //    height: 250,
            //    width: 350,
            //    modal: true,
            //    buttons: {
            //        "Edit": function () {
            //            //$('a[href="#tabs-2"]').click();
            //            $('[id*=hfCurrentPage]').val("EP");
            //            __doPostBack(param, "");
            //            $(this).dialog("close");

            //        },
            //        "Cancel": function () { $(this).dialog("close"); }
            //    }
            //});

            $('#dialog-confirm-edit').dialog('open');
            return false;
        }

        function deleteItem(uniqueID, itemID) {

            var param = itemID + "|DEL";

            $("#dialog-confirm").dialog({
                title: 'Warning',

                resizable: false,
                height: 250,
                width: 350,
                modal: true,
                buttons: {
                    "Delete": function () {
                        $('[id*=hfCurrentPage]').val("DP");
                        __doPostBack(param, "");
                        $(this).dialog("close");

                    },
                    "Cancel": function () { $(this).dialog("close"); }
                }
            });

            $('#dialog-confirm').dialog('open');
            return false;
        }

        function editItem_user(uniqueID, itemID) {

            var param = itemID + "|EDIT_U";
            $('[id*=hfCurrentPage]').val("EU");
            __doPostBack(param, "");

            //$("#dialog-confirm-edit").dialog({
            //    title: 'Warning',

            //    resizable: false,
            //    height: 250,
            //    width: 350,
            //    modal: true,
            //    buttons: {
            //        "Edit": function () {
            //            //$('a[href="#tabs-2"]').click();
            //            $('[id*=hfCurrentPage]').val("EU");
            //            __doPostBack(param, "");
            //            $(this).dialog("close");

            //        },
            //        "Cancel": function () { $(this).dialog("close"); }
            //    }
            //});

            //$('#dialog-confirm-edit').dialog('open');
            return false;
        }

        function deleteItem_user(uniqueID, itemID) {

            var param = itemID + "|DEL_U";

            $("#dialog-confirm").dialog({
                title: 'Warning',

                resizable: false,
                height: 250,
                width: 350,
                modal: true,
                buttons: {
                    "Delete": function () {
                        $('[id*=hfCurrentPage]').val("DU");
                        __doPostBack(param, "");
                        $(this).dialog("close");

                    },
                    "Cancel": function () { $(this).dialog("close"); }
                }
            });

            $('#dialog-confirm').dialog('open');
            return false;
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

        .line4_footer {
            position: absolute;
            top: 150px;
            left: 13%;
            width: 100%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */
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
                left: 17%;
                width: 100%;
                text-align: center;
            }
        }
    </style>
    <style type="text/css">
        .header-center {
            text-align: center;
            white-space: nowrap !important;
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

    <div class="header">
        <asp:Label ID="lblAccMngHeader" runat="server" Text="Account Management"></asp:Label>
    </div>
    <div class="line4_footer">
        <div style="width: 100%; font-size: 14px; color:black;">
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
            <div id="tabs" style="width: 100%; height: 100%">
                <ul>
                    <li><a href="#tabs-1">Account List</a></li>
                    <li><a href="#tabs-2">Add/Edit User</a></li>
                    <li><a href="#tabs-3">Add/Edit Permission</a></li>
                </ul>

                <div id="tabs-1">
                    <div style="position: absolute; left: 35%; width: 400px; color: white;">
                        <asp:LinkButton ID="linkAddUser" runat="server" OnClick="linkAddUser_Click" Style="width: 160px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                            class="btn btn-primary btn-user btn-block">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/img/plus.png" Width="23px" />Add
                        </asp:LinkButton>
                    </div>
                    <div style="position: absolute; left: 47%; width: 155px;">
                        <asp:LinkButton ID="linkExportReport" runat="server" OnClick="linkExportReport_Click" Style="width: 160px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                            class="btn btn-primary btn-user btn-block">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" />Export Excel
                        </asp:LinkButton>
                    </div>
                    <br />
                    <br />
                    <asp:GridView ID="grdAccMng" runat="server" AutoGenerateColumns="false" class="table table-striped"
                        BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                        DataKeyNames="PerPK" EmptyDataText="No records Found" ShowHeaderWhenEmpty="True" Width="100%" OnRowCommand="grdAccMng_RowCommand" OnRowDataBound="grdAccMng_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                            <asp:BoundField DataField="Password" HeaderText="Password" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                            <asp:BoundField DataField="PageName" HeaderText="Page Name" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                            <asp:BoundField DataField="PageDesc" HeaderText="Page Description" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                            <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                CommandName="imgEdit" DataTextField="PerPK" ControlStyle-CssClass="imgWidthEdit"
                                HeaderText="Edit"
                                ImageUrl="~/img/document.png" />
                            <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                CommandName="imgRemove" DataTextField="PerPK" ControlStyle-CssClass="imgWidthDel"
                                HeaderText="Remove"
                                ImageUrl="~/img/x-button.png" />

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
                <div id="tabs-2">
                    <table style="width: 100%; height: 150px">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="lblUserName" runat="server" Text="User Name : "></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtUserName" runat="server" Width="250px"></asp:TextBox><asp:Label ID="lblVUserName" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>

                            <td style="text-align: right">
                                <asp:Label ID="lblPassword" runat="server" Text="Password : "></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtPassword" runat="server" Width="250px"></asp:TextBox><asp:Label ID="lblVPassword" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                <asp:HiddenField ID="hfUserPK" runat="server" />
                                <div style="position: absolute; left: 35%; width: 400px; color: white;">

                                    <a id="btnSave_User" runat="server" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                        class="btn btn-primary btn-user btn-block" onclick="return SaveUser();">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />Save
                                    </a>

                                    <%--<asp:Button ID="btnSave_User" runat="server"  class="btn btn-primary btn-user btn-block" Text="Save" Width="150px" ValidationGroup="AddAcc" />--%>
                                </div>

                                <div style="position: absolute; left: 47%; width: 155px;">

                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-primary btn-user btn-block" Text="Cancel" Width="150px" />
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                                <br />
                                <asp:GridView ID="grdUser" runat="server" AutoGenerateColumns="false" class="table table-striped"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    DataKeyNames="UserPK" EmptyDataText="No records Found" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdUser_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                        <asp:BoundField DataField="Password" HeaderText="Password" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center" ItemStyle-CssClass="leftRow" />
                                        <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                            CommandName="imgEdit_User" DataTextField="UserPK" ControlStyle-CssClass="imgWidthEdit"
                                            HeaderText="Edit"
                                            ImageUrl="~/img/document.png" />
                                        <asp:ButtonField ButtonType="Image" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center"
                                            CommandName="imgRemove_User" DataTextField="UserPK" ControlStyle-CssClass="imgWidthDel"
                                            HeaderText="Remove"
                                            ImageUrl="~/img/x-button.png" />

                                    </Columns>
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                    <SortedDescendingHeaderStyle BackColor="#002876" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tabs-3">
                    <table style="width: 100%; height: 150px">
                        <tr>
                            <td style="width: 10px;"></td>
                            <td style="width: 100px; text-align: right">
                                <asp:Label ID="lblPageName" runat="server" Text="Page Name : "></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                            <td style="width: 300px; text-align: left; white-space: nowrap;">
                                <asp:TextBox ID="txtPageName" runat="server" Width="350px"></asp:TextBox><asp:Label ID="lblVPageName" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 5%;"></td>
                        </tr>
                        <tr>
                            <td style="width: 10px;"></td>
                            <td style="width: 100px; text-align: right">
                                <asp:Label ID="lblPageDesc" runat="server" Text="Page Description : "></asp:Label>&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left; white-space: nowrap;">
                                <asp:TextBox ID="txtPageDesc" runat="server" Width="350px"></asp:TextBox><asp:Label ID="lblVPageDesc" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 5%;"></td>
                        </tr>
                        <tr>
                            <td style="width: 10px;"></td>
                            <td style="width: 100px; text-align: right">
                                <asp:Label ID="lblUserR" runat="server" Text="User : "></asp:Label>&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 300px; text-align: left; white-space: nowrap;">
                                <asp:DropDownList ID="ddlUser" runat="server" Height="25px" Width="200px"></asp:DropDownList>
                            </td>
                            <td style="width: 5%;"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:HiddenField ID="hdfPermissionPK" runat="server" />
                                <div style="position: absolute; left: 25%; width: 155px;">
                                    <a id="btnSave_Per" runat="server" href="#" title="" style="width: 150px; font-family: Verdana,Arial,sans-serif; font-size: 1em; color: white;"
                                        class="btn btn-primary btn-user btn-block" onclick="return SavePer();">
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" />Save
                                    </a>
                                    <%-- <asp:Button ID="btnSave_Per" runat="server"  class="btn btn-primary btn-user btn-block" Text="Save" Width="150px" ValidationGroup="AddPermis" />--%>
                                </div>
                                <div style="position: absolute; left: 37%; width: 155px;">
                                    <asp:Button ID="btnCancel_Per" runat="server" OnClick="btnCancel_Per_Click" class="btn btn-primary btn-user btn-block" Text="Cancel" Width="150px" />

                                </div>
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfCurrentPage" runat="server" />
    </div>

    <div id="dialog-message-save" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Save successful!
        </p>

    </div>

    <div id="dialog-message-del" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Remove successful!
        </p>

    </div>

    <div id="dialog-confirm" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to remove this record?</p>
    </div>

    <div id="dialog-confirm-edit" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to edit this record?</p>
    </div>

    <div id="dialog-confirm-save" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to save?</p>
    </div>

    <div id="dialog-confirm-delete" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to remove this data?</p>
    </div>

    <div id="dialog-validate-save" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Please check data before save!</p>
    </div>

    <div id="dialog-validate-delete" title="Warning!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Please check data before remove!</p>
    </div>

</asp:Content>

