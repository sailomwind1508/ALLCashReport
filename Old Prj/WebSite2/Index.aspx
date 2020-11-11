<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function showValidateMsg() {
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    </script>
    <link href="CSS/responsive-body.css" rel="stylesheet" />
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <div class="topnav_body" style="text-align: center">
        <table style="width: 100%; text-align: center">
            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="User Name : "></asp:Label>&nbsp;&nbsp;&nbsp;
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtUser" runat="server" Height="30px" Width="175px"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUser" ValidationGroup="Login" ForeColor="Red" ErrorMessage="Please enter user name"></asp:RequiredFieldValidator>
                </td>
            </tr>

            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Password : "></asp:Label>&nbsp;&nbsp;&nbsp;
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Height="30px" Width="175px" onkeydown="if (event.keyCode == 13) { return btnLogin_Click; }"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPass" ValidationGroup="Login" ForeColor="Red" ErrorMessage="Please enter password"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: center">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" Height="33px" Width="78px" OnClick="btnLogin_Click" ValidationGroup="Login" Class="button" />
                    <br />
                    <br />
                    <br />
                </td>

            </tr>

        </table>
    </div>
    <div id="dialog-message" title="Warning" style="display: none">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
            Please check user name and password!
        </p>

    </div>
    <script>
        (function () {
            var age = document.getElementById('txtPass');
            age.addEventListener('keypress', function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    document.getElementById('btnLogin').click();
                    
                }
            });
        }());
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

