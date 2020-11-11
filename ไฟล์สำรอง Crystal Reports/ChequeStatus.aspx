<%@ Page Title="Cheque Status Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChequeStatus.aspx.cs" Inherits="ChequeStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <link href="jquery-ui-1.12.1.custom/jquery-ui.css" rel="stylesheet">

    <script type="text/javascript">

        $(function () {
            var rowCount = $('[id*=grdChequeStatus] tr').length;

            if (rowCount > 1) {
                $("[id*=linkSave]").css("display", "block");
            }
            else {
                $("[id*=linkSave]").css("display", "none");
            }
        });

        function showValidateMsg() {
            $('[id*=dialog-message]').dialog({
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

        $("[id*=linkSave]").live("click", function () {
            //e.preventDefault();
            $('#dialog-confirm').dialog({
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
        });

        // function Confirm() {
        //    var confirm_value = document.createElement("INPUT");
        //    confirm_value.type = "hidden";
        //    confirm_value.name = "confirm_value";

        //     //$("#dialog-confirm").dialog({
        //     //    resizable: false,
        //     //    height: "auto",
        //     //    width: 400,
        //     //    modal: true,
        //     //    buttons: {
        //     //        "Do you want to save data?": function () {
        //     //            confirm_value.value = "Yes";
        //     //            //$(this).dialog("close");
        //     //        },
        //     //        Cancel: function () {
        //     //            confirm_value.value = "No";
        //     //            //$(this).dialog("close");
        //     //        }
        //     //    }
        //     //});

        //    if (confirm("Do you want to save data?")) {
        //        confirm_value.value = "Yes";
        //    } else {
        //        confirm_value.value = "No";
        //    }
        //    document.forms[0].appendChild(confirm_value);
        //}

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "#C2D69B";
                }
                else {
                    row.style.backgroundColor = "white";
                }
            }
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function MouseEvents(objRef, evt) {
            var checkbox = objRef.getElementsByTagName("input")[0];
            if (evt.type == "mouseover") {
                objRef.style.backgroundColor = "orange";
            }
            else {
                if (checkbox.checked) {
                    objRef.style.backgroundColor = "aqua";
                }
                else if (evt.type == "mouseout") {
                    if (objRef.rowIndex % 2 == 0) {
                        //Alternating Row Color
                        objRef.style.backgroundColor = "#C2D69B";
                    }
                    else {

                        objRef.style.backgroundColor = "white";
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        .collapsible {
            background-color: #486DDA;
            color: white;
            cursor: pointer;
            padding: 5px;
            width: 100%;
            border: none;
            border-color: #486DDA;
            text-align: left;
            outline: none;
        }

            .active, .collapsible:hover {
                background-color: #2E59D6;
                border-color: #486DDA;
            }

        .content {
            padding: 0 5px;
            width: 100%;
            display: none;
            overflow: hidden;
        }

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

        .line1 {
            position: absolute;
            top: 130px;
            left: 20%;
            width: 65%;
            height: 30px;
            text-align: center;
        }

        .line6_footer {
            position: absolute;
            top: 290px;
            left: 13%;
            width: 70%;
        }

        @media screen and (max-width: 1366px) { /* when the width is less than 30em, make both of the widths 100% */

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

            .line1 {
                position: absolute;
                top: 130px;
                left: 20%;
                width: 65%;
                height: 30px;
                text-align: center;
            }

            .line6_footer {
                position: absolute;
                top: 290px;
                left: 18%;
                width: 70%;
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
        }


        @media screen and (max-width: 1024px) { /* when the width is less than 30em, make both of the widths 100% */

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

            .line1 {
                position: absolute;
                top: 150px;
                left: 23%;
                width: 65%;
                height: 30px;
                text-align: center;
            }

            .line6_footer {
                position: absolute;
                top: 290px;
                left: 23%;
                width: 70%;
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
        }
    </style>
    <style type="text/css">
        .header-center {
            text-align: center;
            white-space: nowrap !important;
        }

        .btnLink:link, .btnLink:visited {
            padding: 14px 25px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            width: 150px;
            color: #fff;
            background-color: #4e73df;
            border-color: #4e73df;
        }

        .btnLink:hover, .btnLink:active {
            background-color: blue;
        }

        td {
            white-space: nowrap;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <div class="header">
        <asp:Label ID="Label2" runat="server" Text="Cheque Status Report"></asp:Label>
    </div>
    <div class="line1">

        <table border="0" style="width: 100%; font-size: 14px; color:black;">
            <tr>

                <td style="text-align: left; width: 200px">
                    <asp:Label ID="Label1" runat="server" Text="บริษัท : "></asp:Label>
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:DropDownList ID="ddlCompany" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:Label ID="Label4" runat="server" Text="สถานะเช็ค : "></asp:Label>
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:DropDownList ID="ddlStatus" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td></td>
                <td></td>

            </tr>
            <tr>
                <td style="text-align: left; width: 200px">
                    <asp:Label ID="Label12" runat="server" Text="Check Sum. : "></asp:Label>&nbsp;&nbsp;&nbsp;
                    
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:TextBox ID="txtSearchCheckSum" runat="server" Height="25px" Width="200px" MaxLength="15" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>

                </td>
                <td style="text-align: left; width: 200px">
                    <asp:Label ID="Label13" runat="server" Text="Statement Status : "></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                <td style="text-align: left; width: 200px">
                    <asp:DropDownList ID="ddlStatement" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true"></asp:DropDownList></td>
                <td></td>
                <td></td>
            </tr>
            <tr>

                <td style="text-align: left; width: 150px">
                    <asp:Label ID="Label5" runat="server" Text="Doc. Date From : "></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:TextBox ID="txtStartDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*" ControlToValidate="txtStartDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:Label ID="Label6" runat="server" Text="Doc. Date To : "></asp:Label>
                </td>
                <td style="text-align: left; width: 200px">
                    <asp:TextBox ID="txtEndDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" ControlToValidate="txtEndDate" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>

                </td>
                <td style="text-align: left"></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 120px; text-align: left">
                    <asp:CheckBox ID="chkShowAll" runat="server" Text="แสดงทั้งหมด" Checked="true" />

                </td>
                <td style="width: 150px; text-align: left">
                    <asp:LinkButton ID="Button1" runat="server" OnClick="btnReport_Click" Style="width: 150px;" class="btn btn-primary btn-user btn-block" ValidationGroup="Report">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/search.png" Width="23px" Height="25px" />Search
                    </asp:LinkButton>
                </td>
                <td></td>

                <td style="width: 150px; text-align: left">
                    <a id="linkSave" href="#" title="" style="width: 150px;" class="btn btn-primary btn-user btn-block">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/floppy-disk.png" Width="23px" Height="25px" />Save
                    </a>

                </td>
                <td style="width: 155px; text-align: left">
                    <asp:LinkButton ID="linkExportReport" runat="server" OnClick="linkExportReport_Click" OnClientClick="return closeDialog();" Visible="false" Style="width: 150px;" class="btn btn-primary btn-user btn-block">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/excel_img.png" Width="23px" Height="25px" />Export Report
                    </asp:LinkButton>

                </td>
                <td></td>

            </tr>
        </table>
        <button type="button" class="collapsible">เงื่อนไขอื่น ๆ </button>

        <div class="content">
            <table border="0" style="width: 100%;font-size: 14px; color:black;">
                <tr>
                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label10" runat="server" Text="Doc. No. From: "></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtDocNo" runat="server" Height="25px" Width="200px" MaxLength="10" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label18" runat="server" Text="Doc. No. To: "></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtDocNoTo" runat="server" Height="25px" Width="200px" MaxLength="10" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    </td>
                    <td style="text-align: left; width: 200px"></td>
                    <td style="text-align: left; width: 200px"></td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 200px;">
                        <asp:Label ID="Label11" runat="server" Text="Cheque No From. : "></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtCheque" runat="server" Height="25px" Width="200px" MaxLength="10" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>

                    </td>
                    <td style="text-align: left; width: 200px;">
                        <asp:Label ID="Label17" runat="server" Text="Cheque No To. : "></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtChequeTo" runat="server" Height="25px" Width="200px" MaxLength="10" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>
                    </td>
                    <td style="text-align: right; width: 224px; white-space: nowrap;"></td>
                    <td></td>
                </tr>
                <tr>

                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label3" runat="server" Text="Statment Date From: "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtSStatmentDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>

                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label7" runat="server" Text="Statment Date To : "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtEStatmentDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>

                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label8" runat="server" Text="Receive Date From: "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtSReceiveDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>

                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label9" runat="server" Text="Receive Date To : "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtEReceiveDate" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>

                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label14" runat="server" Text="Check Date From: "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtCheckDateFrom" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox>

                    </td>
                    <td style="text-align: left; width: 200px">
                        <asp:Label ID="Label15" runat="server" Text="Check Date To : "></asp:Label></td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtCheckDateTo" runat="server" Height="25px" Width="200px" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtEndDateClass"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 200px;">
                        <asp:Label ID="Label16" runat="server" Text="Card Code : "></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                    <td style="text-align: left; width: 200px">
                        <asp:TextBox ID="txtCardCode" runat="server" Height="25px" Width="200px" MaxLength="15" ViewStateMode="Enabled" EnableViewState="true" CssClass="txtStartDateClass"></asp:TextBox></td>
                    <td style="text-align: left; width: 200px;"></td>
                    <td style="text-align: left; width: 200px"></td>
                    <td style="text-align: right; width: 224px; white-space: nowrap;"></td>
                    <td></td>
                </tr>

            </table>
        </div>
    </div>

    <div class="line6_footer" style="font-size: 14px; color:black;">
        <asp:GridView ID="grdChequeStatus" runat="server" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
            CellPadding="4" DataKeyNames="DocNo" AllowPaging="true" ShowHeaderWhenEmpty="True"
            AllowSorting="true" PageSize="20" OnPageIndexChanging="grdChequeStatus_PageIndexChanging" OnRowCreated="grdChequeStatus_RowCreated" OnSorting="grdChequeStatus_Sorting" OnRowCommand="grdChequeStatus_RowCommand"
            OnRowDataBound="grdChequeStatus_RowDataBound">

            <Columns>
                <asp:TemplateField>

                    <HeaderTemplate>

                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />

                    </HeaderTemplate>

                    <ItemTemplate>
                        <asp:CheckBox ID="chkChequeStatus" runat="server" onclick="Check_Click(this)" />
                        <asp:HiddenField ID="hfChequeStatus" runat="server" Value='<%# Bind("Status") %>' />
                        <%--<asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />--%>
                    </ItemTemplate>

                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Current Cheque Status" SortExpression="ChequenoTemp" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                    </HeaderTemplate>
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkChequeStatus" runat="server" />
                        <asp:HiddenField ID="hfChequeStatus" runat="server" Value='<%# Bind("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Status" SortExpression="U_chequestatus" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblU_chequestatus" runat="server" Text='<%# Bind("U_chequestatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cheque" SortExpression="ChequenoTemp" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:TextBox ID="txtChequeNo" runat="server" Text='<%# Bind("ChequenoTemp") %>' MaxLength="8" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SAPCheque." SortExpression="ChequeNoSAP" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblChequeNoSAP" runat="server" Text='<%# Bind("ChequeNoSAP") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ReceiveDate" SortExpression="ReceiveDate" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:TextBox ID="txtReceiveDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval (Container.DataItem, "ReceiveDate")) %>' MaxLength="10" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Statement" SortExpression="StatementDate" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:TextBox ID="txtStatementDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval (Container.DataItem, "StatementDate")) %>' MaxLength="10" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Check Sum." SortExpression="CheckSum" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="lblCheckSum" runat="server" Text='<%# String.Format("{0, 0:N2}",DataBinder.Eval (Container.DataItem, "CheckSum")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Doc. No." SortExpression="DocNo" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblDocNo" runat="server" Text='<%# Bind("DocNo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Trans. Id" SortExpression="TransId" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblTransId" runat="server" Text='<%# Bind("TransId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Doc. Date" SortExpression="DocDate" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblDocDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval (Container.DataItem, "DocDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Code" SortExpression="CardCode" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblCardCode" runat="server" Text='<%# Bind("CardCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Name" SortExpression="CardName" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblCardName" runat="server" Text='<%# Bind("CardName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Doc. Type" SortExpression="DocType" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblDocType" runat="server" Text='<%# Bind("DocType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Check Date" SortExpression="CheckDate" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblCheckDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval (Container.DataItem, "CheckDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bank Num." SortExpression="BankNum" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblBankNum" runat="server" Text='<%# Bind("BankNum") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch" SortExpression="Branch" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Company" SortExpression="Company" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comments" SortExpression="Comments" ItemStyle-Wrap="false" HeaderStyle-CssClass="header-center">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <PagerStyle CssClass="pagination-ys" />
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
        </asp:GridView>

    </div>

    <script type="text/javascript">

        $("#<%=txtStartDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtEndDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $("#<%=txtSStatmentDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

        $("#<%=txtEStatmentDate.ClientID %>").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtStatementDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtReceiveDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtSReceiveDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtEReceiveDate]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtCheckDateFrom]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });
        $('[id*=txtCheckDateTo]').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy'
        });

    </script>

    <script>
        var coll = document.getElementsByClassName("collapsible");
        var i;

        for (i = 0; i < coll.length; i++) {
            coll[i].addEventListener("click", function () {
                this.classList.toggle("active");
                var content = this.nextElementSibling;
                var footer = $(".line6_footer");

                if (content.style.display === "block") {
                    content.style.display = "none";

                    footer.css("top", "290px");
                } else {
                    content.style.display = "block";
                    footer.css("top", "470px");
                }
            });
        }
    </script>

    <div id="dialog-message" title="Message" style="display: none" class="save-dialog-message">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Save successful!
        </p>

    </div>

    <div id="dialog-confirm" title="Warring!" style="display: none" class="save-dialog-message">
        <p><span class="ui-icon ui-icon-alert" style="float: left; margin: 12px 12px 20px 0;"></span>Do you want to save data?</p>
    </div>

</asp:Content>

